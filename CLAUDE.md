# RobotWar — Contexte projet pour Claude Code

> Compilé depuis le Projet Claude "RobotWar" (claude.ai) et l'historique de conversations, pour transfert vers Claude Code. À placer à la racine du repo.

## Vue d'ensemble

RobotWar est un prototype de jeu RTS solo développé sous **Unity 6.4**, par Hadrien — **non-développeur professionnel** — dans une démarche de démonstration de compétences plutôt que de produit commercial. Un **Game Design Document (GDD)** sert de spec de référence ; les décisions d'architecture sont validées par rapport à lui.

Systèmes cœur :
- Plusieurs catégories d'unités
- Double système de ressources : **Réquisition** et **Modules IA**
- Bâtiments de production d'unités
- Système probabiliste de bonus/malus IA lié à des seuils de charge réseau
- Cinq types de dégâts
- Points de capture

Les échanges sur ce projet se font en français.

## Comment travailler avec Hadrien sur ce projet

- **Pas un développeur pro** → privilégier des solutions simples, lisibles, directes : appels singleton directs, patterns MonoBehaviour classiques, abstraction minimale.
- Éviter les patterns avancés (EventBus, SOLID strict, découplage complexe) sauf demande explicite. L'objectif est un prototype fonctionnel, pas une architecture production-grade.
- Il préfère un **apprentissage guidé** à des solutions clés en main : il veut comprendre et construire lui-même.
- Une approche **socratique** (questions ciblées, guider vers la conclusion) fonctionne bien — il retrouve seul ses inversions logiques (checks de faction, incrément/décrément) quand on le pousse à ré-examiner ses conditions. Il demande parfois des réponses plus directes quand il est bloqué : lire le contexte et adapter.
- Corriger les bugs bloquants d'abord, refactorer ensuite — les problèmes non-bloquants sont notés et différés.
- Décisions pragmatiques : *"don't touch it if it works"*.
- Humour autodérision fréquent quand il identifie ses propres erreurs.

## Conventions du projet

- **ScriptableObjects** pour toutes les définitions de données (unités, bâtiments, munitions, effets réseau), avec des fenêtres Editor custom pour la création d'assets.
- Pattern **`InitBuild()`** (méthode virtuelle) plutôt que `Start()` pour l'initialisation des bâtiments — clarté d'intention.
- Repo **git-versionné**, Hadrien y travaille avec Claude Code.

## Système économique (Réquisition)

Pas un jeu d'automatisation façon Factorio (pas de mines/chaînes de production). Le joueur reçoit de la **Réquisition** périodiquement via un **ascenseur spatial**, dépensée pour construire bâtiments/unités.

Architecture validée :
```
SpaceElevator (timer) → PlayerResources.Add() → PlayerResources (tampon)
                                                        ↓
                                          ConstructionManager.TrySpend() → build / fail
```
Découplage intentionnel : `SpaceElevator` ne connaît que `PlayerResources` (alimente) ; `ConstructionManager` ne connaît que `PlayerResources` (dépense) ; aucun couplage direct entre les deux. Permet d'ajouter facilement une 2ᵉ source de réquisition (event bonus, capture de point stratégique) sans toucher au reste.

## État actuel — Systèmes de commande et de comportement des unités (travail actif)

- `CommandSystemController` avec méthode `ExecuteOrder` distribuant des `OrderData` aux unités via `IOrderReceiver`.
- State machine : `AUnitState` (classe abstraite, `Enter/Update/Exit`) avec états concrets `IdleState`, `MovingState`, `AttackState`.
- Architecture par interfaces : `ITargetableObject`, `ISelectable`, `IOrderReceiver`.
- `OrderData` : struct avec constructeurs spécialisés par type d'ordre (MOVETO, ATTACK, STOP).
- ✅ Résolu : dead code / branche `MOVETO` inaccessible dans `ExecuteOrder`, corrigé via `TryGetComponent<ITargetableObject>()`.
- ✅ Résolu : null reference dans `AttackState.Enter` causée par des sources de vérité conflictuelles pour la cible d'attaque.
- 🔴 **Bug ouvert** : les unités se déplacent jusqu'au contact mêlée avec leur cible au lieu de s'arrêter à portée d'attaque — probable condition booléenne inversée dans `AttackState.Update()` combinant `targetDistance >= AttackRange` avec `!unit.NavMeshAgent.isStopped`.

**Prochaine étape** : résoudre ce bug de portée d'attaque, puis compléter une revue d'architecture Game Design / Gameplay Programming plus large (demandée mais différée en attendant la résolution des bugs actifs).

## IA ennemie (planifiée, non implémentée)

FSM avec états `IDLE`, `ATTACKING`, `DEFENDING`, répartie sur les scripts `EnnemyIAManager`, `EnnemyUnitController`, et `Assembly` (production continue d'unités).

Problèmes connus identifiés à l'avance :
- `ChangeIAState()` : coroutine sans `while(true)` manquant.
- `BaseAttacked()` : usage incorrect de `Time.deltaTime` dans une coroutine à tick lent.
- `currentAttackList` jamais peuplée.
- `SpawnUnit()` ne déclenche pas encore l'allocation des unités dans les listes IA.

## Points de capture (Supply Tower)

Approche timer-based dans `Update()`. `captureMultiplier` volontairement **non clampé à zéro** — la présence ennemie inverse la progression de capture. Vitesse de capture modifiée dynamiquement par frame selon le nombre d'unités présentes.

## Leçons techniques Unity/C# (durement acquises)

- **ScriptableObjects = config en lecture seule** ; l'état runtime mutable va sur les MonoBehaviours. Pattern "Stats vs State" : stats de base sur le SO `UnitData`, valeurs runtime (`currentHealth`, `currentArmor`) sur `AUnitClass`.
- **Source unique de vérité** : les conflits entre champs injectés par constructeur et état accédé au runtime (ex. `currentTarget` vs `unit.Order.OrderTarget`) causent des bugs difficiles à tracer.
- **`OnDestroy()`** pour le cleanup — plus fiable que des méthodes de destruction custom car se déclenche quel que soit le mode de destruction ; `UnitDestroyed()` doit uniquement appeler `Destroy(gameObject)`.
- **Découplage par interfaces** efficace sur cette codebase — `TryGetComponent<ITargetableObject>()` plus cohérent architecturalement que le branching par layer-mask.
- **`Physics.OverlapSphere`** nécessite un tracking manuel des sorties — diffing des résultats contre une liste stockée à chaque tick ; une liste de suppression + un flag `isFound` évite les exceptions de modification de collection.
- La direction d'un projectile se calcule comme `target.position - transform.position`, jamais assignée directement comme position ; le prefab doit être configuré après instantiation, pas avant.
- La **vélocité est déjà en unités/seconde** — ne pas la multiplier par `Time.deltaTime`.
- **`Camera.main`** est fragile (nécessite le tag `"MainCamera"`) — une référence caméra injectée est plus robuste.
- **Coroutines vs timers `Update()`** : pour les systèmes tick continus nécessitant un ajustement dynamique par frame, `Update()` avec timer manuel est généralement préférable aux coroutines à l'échelle de ce projet.

## Historique récent additionnel (hors mémoire structurée)

- Debug d'erreurs d'axes de l'Input Manager causées par une modification involontaire lors du setup AZERTY/ZQSD.
- Correction d'un système de jauge UI (`NetworkUIController`) — bugs copier-coller et `sizeDelta` vs `localPosition`.
- Étude approfondie du pipeline d'exécution global Unity (Awake → Start → FixedUpdate → Update → LateUpdate → Rendering).
- Pattern classe utilitaire statique pour le debug de gizmos editor-only (`CollisionGizmos`, guards `#if UNITY_EDITOR`).
- Exploration des templates de script Visual Studio / VS Code, snippets avec tab stops, et raccourcis de régions.

## Automatisation de reporting (en cours de mise en place)

Hadrien met en place un **hook git `post-commit`** qui lance `claude -p` en headless pour analyser chaque commit et écrire un rapport dans Notion :
- Page cible : `Base de Donnée Global / Liste Projet / RobotWar / Design Log / Programming Log`
- Architecture envisagée :
  ```
  .claude-hooks/post-commit-report.sh   (versionné, activé via `git config core.hooksPath .claude-hooks`)
    → claude -p "$(cat prompt-template.md)" \
        --mcp-config notion-mcp.json \
        --allowedTools "Bash(git:*),mcp__notion" \
        --permission-mode acceptEdits
  ```
- Détection automatique du projet (RobotWar vs Exosteam) via le nom du dossier repo ou une variable `.claude-hooks/config`.
- Une base de données Notion dédiée est en cours de conception côté Hadrien pour recevoir ces rapports (schéma de propriétés à caler).

## ⚠️ Non transférable depuis cette surface

- Un **board Miro** est référencé comme faisant partie du contexte complet du Projet Claude RobotWar (documents + architecture visuelle), mais son contenu n'est pas accessible depuis la mémoire ou l'historique de conversation — seul son existence est connue. Si utile à Claude Code, il faudra que Hadrien en partage le lien ou exporte le contenu manuellement.
- Le **GDD complet** (Game Design Document) existe comme document de référence dans le Projet Claude mais son contenu détaillé n'a pas été retrouvé dans les conversations indexées ici — probablement chargé comme pièce jointe du Projet, non capturé en mémoire persistante.
