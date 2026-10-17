# RobotWar — Contexte projet pour Claude Code

> Simplifié le 2026-10-17. Les versions complètes de ce fichier et de la mémoire de Claude Code (bugs résolus, journaux de session) sont archivées dans le Design Log Notion : « Archive : fichiers de suivi de session avant simplification ».

## Vue d'ensemble

RobotWar est un prototype de RTS solo sous **Unity 6.4**, développé par Hadrien (**non-développeur professionnel**) pour démontrer des compétences plutôt que sortir un produit. Échanges en français.

Projet Notion : RobotWar

- Le **GDD** est la spec de référence, les décisions d'architecture sont validées contre lui : Notion, projet RobotWar, « Game Design Document » et pages Mécanique (Unité, Bâtiment, Module IA, Entropie, Sélection et Ordre Système).
- Systèmes cœur : catégories d'unités ; double ressource **Réquisition** + **Modules IA** (capacité du réseau) ; bâtiments de production ; bonus/malus probabilistes liés à la charge réseau ; cinq types de dégâts ; points de capture ; entropie et surcadençage du réseau.
- **État au 2026-10-17** : boucle jouable de bout en bout (construire, produire, commander ; l'IA produit, attaque le QG et défend). IA validée (15/15 tests), interactions du joueur 35/46 (plan Notion « Plan de tests : interactions du joueur »).
- **Pas de condition de victoire ou de défaite** : `HeadQuarter.BuildDestroyed()` est vide.

## Comment travailler avec Hadrien

- **Pas un développeur pro** : solutions simples, lisibles, directes (appels singleton directs, MonoBehaviour classiques, abstraction minimale). Pas d'EventBus, de SOLID strict ni de découplage complexe sauf demande.
- Il préfère l'**apprentissage guidé**. L'approche **socratique** marche bien (il retrouve seul ses inversions logiques) ; s'il est bloqué et demande une réponse directe, la donner.
- **Boucle habituelle** : il écrit le correctif, puis dit « regarde ». Relire les fichiers, donner la cause avec les lignes, ne pas modifier son code sans demande, dire quels tests refaire.
- **Le design est son choix** : recommander avec le compromis, puis poser la question. Il a raison sur le design (ex. l'IA réagit même à un bâtiment invulnérable).
- Bugs bloquants d'abord, refactor ensuite ; le non-bloquant est noté et différé. *"Don't touch it if it works."*
- Vocabulaire : « renfort » plutôt que « vague ». Analyses dans Claude Docs ou Notion, pas dans le repo.
- Humour autodérision quand il repère ses erreurs : ton léger.

## Conventions

- **ScriptableObjects** pour toutes les définitions de données, avec des fenêtres Editor custom pour créer les assets.
- Bâtiments : initialisation dans **`ABuildClass.Start()`** (`protected`). Un enfant qui déclare son propre `Start()` **doit appeler `base.Start()`**, sinon réquisition, durabilité et armure ne sont pas initialisées.
- **Une donnée = un seul propriétaire.** `IAManager` décide et possède ses données, les contrôleurs exécutent.
- Une action « une seule fois » se place sur la **transition d'état** (comme `Enter()`), pas derrière un booléen redondant.
- Données par camp : classe C# `[Serializable]` (`NetworkData`, `RequisitionData`) tenue par un singleton unique et lue par `GetXxxData(faction)`, pas deux managers dupliqués.
- Scènes : tests dans `IATestScene` (aussi Build, Effect, Network, Unit). `GameScene` n'a pas encore les contrôleurs réseau ni la jauge.

## Réquisition (économie)

- Le joueur reçoit de la Réquisition périodiquement via un **ascenseur spatial** (prefab `Space Lift`, `EBuildType.SPACEELEVATOR`), dépensée pour construire et produire. Pas d'automatisation façon Factorio.
- **Cible validée** : `SpaceElevator` → `PlayerResources.Add()` ; `ConstructionManager.TrySpend()`. Les deux ne connaissent que `PlayerResources` (2ᵉ source possible sans toucher au reste). Ces classes n'existent pas : le stand-in est `RequisitionManager`.
- **Code** (commit `2e13346`, Joueur et IA unifiés) :
  - `RequisitionData` : stock d'un camp + `OnUpdateRequisition(int)` qui envoie le **stock total**.
  - `RequisitionManager` : singleton unique, `GetRequisitionData(faction)`.
  - `SupplyTower` alimente (et réassigne après capture), `Assembly` dépense. `RequisitionUIController` n'écoute que le joueur.
- **Pas encore** : coût et temps de construction (`BuildData.buildingCost/buildingTime` non lus, bâtiments gratuits). Prévu : `TrySpend(faction, montant)` dans `RequisitionManager`.
- À faire :
  - `GetRequisitionData(NONE)` renvoie `null` (NRE pour une tour neutre).
  - `ABuildClass.requisitionFaction` en `[SerializeField]` par erreur : remettre `[ShowInInspector]`.
  - « Missing Script » (ancien `IARequsitionManager`) dans `RequisitionController.prefab` et `IAManager.prefab`.
  - `PlayerDebugTool` / `IADebugTool` affichent `RequisitionData.ToString()` au lieu de `.RequisitionStock`, et `IADebugTool` lit `ALLY` au lieu de `IA`.

## Commande, sélection, ciblage

- `CommandSystemController.ExecuteOrder` distribue des `OrderData` (MOVETO, ATTACK, STOP) via `IOrderReceiver`. `ReceiveOrder()` recrée un état à chaque appel : **jamais d'ordre à chaque frame**.
- États d'unité (`AUnitState`) : `IdleState` (engage la 1ʳᵉ cible de `ScanningComponent`), `MovingState`, `AttackState` (s'arrête à `AttackRange`, retour `Idle` si la cible disparaît). `ShellController.shellFaction` ignore sa propre faction.
- **Sélection** :
  - `ISelectable.SelectionFaction` ; `ClickSelect` ignore les objets `IA` avec un `return` **avant** `DeselectAll` (la sélection courante est conservée). Les bâtiments neutres restent sélectionnables.
  - Plus tard : l'UI distinguera « voir » (tous) et « commander » (`ALLY`).
  - Le rectangle (`UnitDragComponent`) parcourt `PlayerActiveUnits`.
- **Ciblage** :
  - `ITargetableObject.IsTargetable` : pas d'ordre ni de marqueur au clic droit, `ScanningComponent` ignore la cible.
  - `ABuildClass.isInvulnerable` court-circuite `DurabilityUpdate`, mais l'alerte `BuildManager.OnBuildingAttacked` part quand même. **Invulnérable ≠ ciblable** : l'IA réagit toujours aux coups, une condition du jeu lève l'invulnérabilité.
  - Space Lift : invulnérable, non ciblable par les unités, sélectionnable.
- **Enregistrement des unités** :
  - `UnitManager.UnitAvailable` (liste active + charge réseau) n'est appelé que par `Assembly.SpawnUnitProduction`. **Toute unité du joueur doit sortir d'une usine.**
  - Une unité posée dans la scène n'est pas sélectionnable au rectangle (le clic simple marche) et n'est pas comptée dans la charge réseau, qui devient négative à sa mort.
  - Si une armée de départ est ajoutée, appeler `UnitAvailable` à son apparition. Même défaut, bien réel, pour les renforts IA.
- **Décisions prises, pas encore codées** :
  - Un clic dans le vide **vide** la sélection (sauf Maj).
  - Le rectangle **remplace** la sélection (il l'ajoute aujourd'hui) et ne laisse pas un bâtiment sélectionné avec des unités.
  - UI de choix de production en pause : une seule unité par usine, `isProductionEnabled` se coche sur le prefab (aucun code ne l'active).
  - Zone de construction autour des tours pour certains bâtiments (Warehouse, Module IA probablement).
- **Bugs ouverts** :
  - Groupe de 10 unités vers un point (test 19) : `MovingState` n'arrive jamais quand les places proches sont prises. Piste : un point d'arrivée par unité.
  - Clic à travers l'UI non filtré (tests 43-44) : `EventSystem.current.IsPointerOverGameObject()`.
  - Bonus du Warehouse à clarifier (test 39).
  - STOP = `KeyCode.W`, soit la touche **Z** en AZERTY, donc l'avance de la caméra : à changer.
  - Un ordre STOP remet `Idle`, donc réengagement immédiat (à décider).
  - `IdleState` peut lire un `TargetedObject[0]` tout juste détruit.
- Différé : `AttackState.Update` fait un `SetDestination` par frame hors de portée (ne recalculer que si la cible a bougé de ~1-2 m ou toutes les ~0,25 s, quand le nombre d'unités fera ramer) ; revue d'architecture Game Design / Gameplay Programming plus large.

## IA ennemie

Scripts dans `Assets/Script/IA/`. Outil éditeur `Tools/IA Debug` : forcer renforts, coups, escouades.

- `IAManager` (singleton) : **deux machines à états en parallèle** dans `Update()`.
  - Attaque : `currentState` (`IDLE` ↔ `ATTACKING`).
  - Défense : bool `isBaseUnderAttack` (calme ↔ alerte), qui ne touche jamais `currentState`.
  - Il possède `defenseUnits` / `attackUnits` (défense d'abord jusqu'à `defenseThreshold`, puis attaque), `launchedAttackCount` et `attackThreshold` (`attackThresholdCurve` + `Lerp` min/max).
  - Événements : `OnAttack`, `OnDefend(ABuildClass)`, `OnUnitJoinAttack`, `OnUnitJoinDefense`.
- `IAAttackController` : sur `OnAttack`, ordre ATTACK vers `playerHQ` (`HeadQuarter`) à toutes les unités d'attaque, une fois ; `TransmitOrder` pour les arrivées en cours d'attaque.
- `IADefenseController` : sur `OnDefend`, MOVETO vers le bâtiment touché, puis `IdleState` engage seul. `calmTimer` repart à 0 à chaque coup, l'alerte finit à `calmDuration`.
- `IAReinforcementSpawner` + `ReinforcementData` (Odin) : renforts **gratuits**, toujours en attaque, qui complètent `attackUnits` jusqu'au seuil par tirage pondéré, puis bouclent sur le dernier palier.
- **Intention** : le seuil croît (log) plus vite que la production des `Assembly`, donc les renforts forment une part croissante de chaque attaque. Une attaque peut durer tant que des renforts la complètent (pression continue, à playtester).
- À faire / bugs connus :
  - `UpdateState(state)` change l'état ET exécute sa logique (réentrant, Hadrien préfère cette forme) : si `attackThreshold == 0`, récursion infinie. Ne jamais mettre `minAttackThreshold` à 0.
  - `DEFENDING` et `responseTime` inutilisés.
  - Retirer les logs de debug de `MovingState` / `IdleState` ; garde-fou anti-doublon dans le `Start()` des contrôleurs (`FindObjectsByType<…>().Length > 1` → `LogError`).
  - `GameManager.prefab` contient un `IAManager` non configuré ; le spawner de `GameScene` a `reinforcementDatas` vide.
  - Multiplicateurs de stats des renforts non appliqués ; un seul point d'apparition.
  - Une unité en `AttackState` ignore les ennemis croisés en chemin (pistes : tir en mouvement ; « attack-move » via `EOrderType.AUTONOMOUS`, inutilisé).
  - `ScanningComponent` utilise `AttackRange` comme portée de vision.
  - Ciblage étape 2 : QG ou unité joueur connue la plus proche (portée de vision et connaissance partagée à trancher).
- Idée long terme validée : **arbre de comportement** (`IAManager` = blackboard, contrôleurs = actions) quand l'IA devra gérer d'autres priorités (prendre des tours, protéger les `Assembly`, se replier). Options : Unity Behavior, Behavior Designer, NodeCanvas, ou maison (~150 lignes).

## Réseau (charge et effets de surcharge)

- `NetworkData` (`[Serializable]`) : état d'un camp (`maxLoad`, `currentLoad`, `PercentLoad`, `OnLoadChange(float)` déclenché par les setters).
- `NetworkManager` : `playerNetwork` / `IANetwork` (`GetNetworkData(faction)`) et le tableau `NetworkEffect`, **commun aux deux camps**.
- `ModuleIA` augmente la **capacité** (`MaxLoad += bonusMaxLoad`). `UnitManager` met `CurrentLoad` à jour.
- `NetworkController` (joueur) / `IANetworkController` (IA) : un contrôleur par camp, `NetworkData` en `[ShowInInspector]` (un `[SerializeField]` créerait une copie).
- **Règle des effets** :
  - Une coroutine lance le dé de chaque effet toutes les `procInterval` s (0,25) tant que la charge dépasse le seuil **de cet effet**.
  - `ProcProbability` = chance **par tick** (valeurs faibles pour les effets violents) ; un tirage raté est retenté au tick suivant.
  - Le cooldown (un minuteur par effet) ne démarre qu'au déclenchement. Nombre d'unités touchées : de 1 à toutes les éligibles.
  - Pas de cumul sur une unité (`currentEffect != null` bloque les nouveaux). Seule la vitesse est gérée.
- **À faire** :
  - 🔴 `EffectComponent.currentEffect` n'est jamais remis à `null` : une unité ne subit qu'**un seul effet de sa vie**.
  - 🔴 `IANetworkController` applique les effets aux unités **du joueur** (`GetElligibleUnits` parcourt `playerActiveUnits` sans faction ; `iaActiveUnits` n'est jamais rempli).
  - Renforts IA hors `UnitAvailable` : absents de `iaActiveUnits`, charge IA négative à leur mort.
  - `NetworkController.networkData` en `[SerializeField]` par erreur.
  - Curseurs de charge des outils de debug : une copie réécrite à chaque `OnGUI` écrase la charge (écrire `network.CurrentLoad = IntSlider(..., network.CurrentLoad, ...)`).
  - `IANetworkController` duplique `NetworkController` (`0.25f` en dur).
  - `GameScene` / `PersistentScene` sans contrôleurs réseau ni `NetworkUIController`.
- Analyse détaillée : Claude Doc « RobotWar — Analyse des effets de surcharge réseau ».

## Capture (Supply Tower)

Timer dans `Update()` : `ScanForCapture` (OverlapSphere) calcule `captureMultiplier`, `CaptureTimer` fait avancer la progression et change le propriétaire au-delà de `captureDuration`.

- **Règles décidées (2026-10-13)** :
  - Comptage **absolu** par faction (ALLY / IA), indépendant du propriétaire.
  - Multiplier = dominante / dominée (10 contre 5 = ×2) ; dénominateur `Mathf.Max(dominée, 1)`.
  - Égalité ou zone vide = **contestation**, multiplier 0. Bonus « aucun ennemi » différé.
  - Le nouveau propriétaire est la faction **dominante** stockée par `ScanForCapture`, à la place du `switch` ALLY ↔ IA de `CaptureTimer` (qui laisse une tour `NONE` en `NONE`).
- **État : 🚧 WIP.**
  - L'égalité donne `captureMultiplier = 1` : une zone vide fait progresser la capture, les tours changent de camp seules.
  - `x / 0` donne `Infinity` : capture instantanée sans adversaire (une division `float` par zéro ne lève pas d'exception).
  - La faction dominante n'est pas stockée ; `GetComponent<AUnitClass>()` n'est pas testé contre `null`.
- Questions ouvertes : tour déjà à la faction dominante ? Progression si la domination change en cours de capture ?

## Écart GDD / code (constaté le 2026-10-12)

Dans le GDD, pas dans le code :

- **Entropie** (structurelle et opérationnelle, sacrifice, refroidissement).
- **Surcadençage**, température et surchauffe du réseau, désactivation d'unités au-delà de 100 % de charge.
- Effets réseau autres que la vitesse (dégâts, précision, rechargement) et malus à partir de ~65 % de charge.
- **4 types de dégâts sur 5** : seul `PHYSIQUE` existe, et l'armure est copiée sur l'unité mais jamais utilisée.
- Unités du GDD (infanterie en escouades, véhicules, aviation, soutien) : le code a 4 types génériques (`MECHA`, `TANK`, `AIRCRAFT`, `ANTIAIR`).
- Coût, temps et zone de construction ; ordres défense, patrouille, reconnaissance ; condition de victoire ou de défaite.

## Leçons techniques Unity/C#

- **SO = config en lecture seule** ; l'état runtime va sur les MonoBehaviours (« Stats vs State » : `UnitData` / `AUnitClass`).
- **Source unique de vérité** : un champ injecté et un état lu au runtime (ex. `currentTarget` vs `unit.Order.OrderTarget`) donnent des bugs difficiles à tracer.
- `OnDestroy()` pour le cleanup, quel que soit le mode de destruction ; `UnitDestroyed()` ne fait que `Destroy(gameObject)`.
- `== null` sur une variable **typée interface** ne détecte pas un objet Unity détruit : retirer l'objet des listes à sa destruction.
- Une **propriété et son champ** qui ne diffèrent que par la casse se confondent : `=> IsTargetable` s'appelle lui-même (StackOverflow). Remplacer les stubs `NotImplementedException` générés par Visual Studio.
- Un champ ajouté à une classe dont les prefabs sont déjà sérialisés prend la valeur de l'initialiseur : choisir le défaut pour les existants.
- Comportement bizarre sur une action pilotée par événement : chercher d'abord un **écouteur ou composant en double** dans la scène (les `Debug.Log` avec pile d'appels aident).
- Renommer un `[SerializeField]` sans `[FormerlySerializedAs]` perd les valeurs de l'Inspector. Un `float` ne vaut presque jamais exactement 0 : tester `<= 0`.
- `[SerializeField]` sur une classe `[Serializable]` = une copie par composant ; Unity ne sérialise pas les `Dictionary` (Odin `[ShowInInspector]`). L'Inspector écrit dans le champ sans passer par le setter (aucun événement).
- `Physics.OverlapSphere` : suivre les sorties à la main (diff contre une liste stockée, liste de suppression + flag `isFound`).
- Projectile : direction = `target.position - transform.position`, prefab configuré après `Instantiate`, vélocité déjà en unités/seconde (pas de `Time.deltaTime`). `Camera.main` exige le tag `MainCamera` : préférer une référence injectée.
- Coroutine pour un rythme fixe (`while (true) { …; yield return new WaitForSeconds(x); }`, thread principal, s'arrête sans boucle) ; timer dans `Update()` pour un ajustement par frame.
- `Dictionary` + `foreach` sous Mono : ne pas modifier pendant le parcours (`new List<K>(dict.Keys)`). `Random.Range(int, int)` exclut le max, la version `float` l'inclut. `Image.fillAmount` demande le type `Filled` et un sprite.
- Découplage par interfaces : `TryGetComponent<ITargetableObject>()` plutôt que du branching par layer-mask.

## Reporting et sources

- Skill global `/rapport-session` (`~/.claude/skills/rapport-session/`) en fin de session : commit après confirmation, puis rapport dans le Design Log Notion (Catégorie `Log Session`, `Projet` lu dans la ligne `Projet Notion :` ci-dessus). Les plans de test y sont en `Log Prog`.
- Sources : Notion (GDD, pages Mécanique, Design Log, archive) ; Claude Docs (« Unifier les managers Joueur / IA », « Analyse des effets de surcharge réseau », « Contexte projet : CLAUDE.md court + doc d'état »).
- Le **board Miro** du Projet Claude d'origine n'est pas lisible depuis ici : demander son export si besoin.
