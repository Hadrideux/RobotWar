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

Projet Notion : RobotWar

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

## Systèmes de commande et de comportement des unités

- `CommandSystemController` avec méthode `ExecuteOrder` distribuant des `OrderData` aux unités via `IOrderReceiver`.
- State machine : `AUnitState` (classe abstraite, `Enter/Update/Exit`) avec états concrets `IdleState`, `MovingState`, `AttackState`. `AttackState` repasse l'unité en `IdleState` quand sa cible devient `null`.
- Architecture par interfaces : `ITargetableObject`, `ISelectable`, `IOrderReceiver`.
- `OrderData` : classe avec constructeurs spécialisés par type d'ordre (MOVETO, ATTACK, STOP). `ReceiveOrder()` recrée un état à chaque appel → ne jamais envoyer un ordre à chaque frame.
- ✅ Résolu : dead code / branche `MOVETO` inaccessible dans `ExecuteOrder`, corrigé via `TryGetComponent<ITargetableObject>()`.
- ✅ Résolu : null reference dans `AttackState.Enter` causée par des sources de vérité conflictuelles pour la cible d'attaque.
- 🟡 Probablement résolu (à confirmer en jeu) : unités qui avançaient jusqu'au contact au lieu de s'arrêter à portée — `AttackState.Update()` teste maintenant `targetDistance <= AttackRange` puis `ResetPath()`.

Différé : revue d'architecture Game Design / Gameplay Programming plus large.

## IA ennemie (travail actif)

Scripts dans `Assets/Script/IA/` :
- `IAManager` (singleton) : **deux machines à états en parallèle** dans `Update()` — attaque via `currentState` (`IDLE` ↔ `ATTACKING`, `UpdateState(currentState)`), défense via le bool `isBaseUnderAttack` (calme ↔ alerte). La défense ne touche jamais `currentState`. Possède les listes `defenseUnits` / `attackUnits` (remplies via `UnitManager.OnUnitProduced` : défense en priorité jusqu'à `defenseThreshold`, puis attaque), `launchedAttackCount` et `attackThreshold` (courbe `attackThresholdCurve` + `Lerp` min/max, progression = attaques lancées / `attackCountAtMaxThreshold`). Communique avec les contrôleurs par événements `OnAttack` / `OnDefend(ABuildClass)`.
- `Attack/IAAttackController` : abonné à `OnAttack` ; `LaunchAttack()` crée l'`OrderData` ATTACK vers `playerHQ` (`HeadQuarter : ABuildClass`) et l'envoie une fois ; `TransmitOrder(unit)` pour les renforts arrivés pendant une attaque.
- `Defense/IADefenseController` : abonné à `OnDefend` ; `LaunchDefense(building)` envoie un `MOVETO` vers la position du bâtiment attaqué aux `DefenseUnits` (à l'arrivée : `IdleState` → engagement automatique).
- Défense : `IAManager` écoute `BuildManager.OnBuildingAttacked` (filtre faction IA) → `calmTimer = 0` à chaque coup, ordre envoyé seulement au premier coup ; `calmTimer` avance dans `Update`, alerte terminée quand `calmTimer >= calmDuration`.
- `Reinforcement/IAReinforcementSpawner` + `ReinforcementData` (Odin) : renforts gratuits qui complètent `attackUnits` jusqu'au seuil, tirage pondéré, boucle sur le dernier palier.
- Côté unités : `IdleState` engage automatiquement la 1ʳᵉ cible de `ScanningComponent.TargetedObject` ; `AttackState` passe `currentTarget` à `TargetUnit(target)` (ne lit plus `currentOrder`) et ne tire qu'à portée ; `ShellController.shellFaction` (réglée dans `Fire()`) ignore les cibles de même faction.
- `IARequsitionManager` (alimenté par `SupplyTower`, dépensé par `Assembly` côté IA) et `IANetworkManager` (charge réseau IA, séparée de celle du joueur).

**Règle d'architecture validée** : `IAManager` **décide** et **possède ses données** (état, listes, compteurs) ; les contrôleurs **exécutent** (construire/envoyer les ordres, détecter). Une donnée = un seul propriétaire. Une action « une seule fois » se place **sur la transition d'état** (comme `Enter()`), pas derrière un booléen redondant avec l'état.

État : attaque et défense écrites et en parallèle, **à tester en jeu** (prochaine étape : session de tests de toutes les mécaniques).

À faire / bugs connus :
- `UpdateState(state)` change l'état ET exécute sa logique ; `AttackAvailable`/`AttackFinished` l'appellent depuis `UpdateState` (réentrant). Hadrien préfère garder cette forme (lisible) — risque connu : si `attackThreshold == 0` → récursion infinie IDLE↔ATTACKING (StackOverflow). Alternative proposée : `ChangeState()` / `UpdateState()` séparés comme dans `AUnitClass`.
- Ménage : `DEFENDING` dans l'enum et `responseTime` inutilisés.
- `IdleState` : `TargetedObject[0]` peut pointer un objet tout juste détruit (ordre des `Update` non garanti) → `MissingReferenceException` possible, à surveiller en test. Un ordre `STOP` remet en Idle → réengagement immédiat (à décider).
- ✅ Résolu : `Assembly` — spawn à la chaîne (le minuteur avançait pendant l'attente de réquisition). Désormais mini-FSM `isProductionStart` : en attente → `TryStartProduction()` vérifie ET débite la réquisition (paiement au début) → en production, minuteur → `SpawnUnitProduction()`. `BuildDestroyed()` ne lève plus d'exception (vide).
- ✅ Résolu : `RemoveUnit` appelé deux fois pour les unités IA → abonnement de `IAManager` à `OnUnitDestroyed` supprimé, seul l'appel direct dans `UnitManager.UnitDestroyed` reste (cohérent avec les appels directs à `NetworkManager`/`IANetworkManager`). L'événement `OnUnitDestroyed` est toujours déclaré mais n'est plus déclenché ni écouté.
- Multiplicateurs de stats des renforts non appliqués ; tous les renforts spawnent sur un seul point.
- Engagement en chemin : une unité en `AttackState` va droit sur sa cible et ignore les ennemis croisés (concerne l'IA qui fonce sur le QG). Pistes : (a) tir en mouvement — la tourelle vise l'ennemi le plus proche pendant que le châssis suit l'ordre ; (b) ordre « attack-move » via `EOrderType.AUTONOMOUS` (inutilisé).
- Portée de vision > portée de tir (`ScanningComponent` utilise `AttackRange` : des attaquants à longue portée ne sont pas vus par les défenseurs).
- Ciblage étape 2 (QG ou unité joueur connue la plus proche ; questions ouvertes : portée de vision, connaissance partagée).

Idée long terme (validée) : **refondre l'IA en arbre de comportement** (Sélecteur / Séquence / Parallèle, `IAManager` = blackboard, contrôleurs = actions) quand elle devra gérer plus de priorités avant d'attaquer le joueur (prendre des Supply Towers, protéger les Assembly, se replier…). Options : package officiel Unity Behavior, assets (Behavior Designer, NodeCanvas), ou implémentation maison (~150 lignes). Belle pièce de portfolio.

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

## Automatisation de reporting

Skill global **`/rapport-session`** (`~/.claude/skills/rapport-session/`) à lancer en fin de session : commit git de fin de session (après confirmation), puis entrée dans la base Notion commune `Base de Donnée Global / Design Log` (Catégorie `Log Session`, propriété `Projet` lue depuis la ligne `Projet Notion :` ci-dessus). Remplace l'ancienne idée de hook git `post-commit` + `claude -p`, qui ne voyait que le diff et pas la conversation.

## ⚠️ Non transférable depuis cette surface

- Un **board Miro** est référencé comme faisant partie du contexte complet du Projet Claude RobotWar (documents + architecture visuelle), mais son contenu n'est pas accessible depuis la mémoire ou l'historique de conversation — seul son existence est connue. Si utile à Claude Code, il faudra que Hadrien en partage le lien ou exporte le contenu manuellement.
- Le **GDD complet** (Game Design Document) existe comme document de référence dans le Projet Claude mais son contenu détaillé n'a pas été retrouvé dans les conversations indexées ici — probablement chargé comme pièce jointe du Projet, non capturé en mémoire persistante.
