# RobotWar — Contexte projet pour Claude Code

> Les règles communes (comment travailler avec Hadrien, garde-fous, agents, leçons Unity/C#) sont dans `~/.claude/CLAUDE.md`. L'état du projet est dans Notion, page « État du projet » sous RobotWar ; les décisions sont dans le « Registre des décisions » (Projet = RobotWar). Ce fichier ne garde que ce qui est propre à RobotWar et stable.

Projet Notion : RobotWar

## Vue d'ensemble

Prototype de RTS solo sous **Unity 6.4**, développé pour démontrer des compétences plutôt que sortir un produit.

- Systèmes cœur : catégories d'unités ; double ressource **Réquisition** + **Modules IA** (capacité du réseau) ; bâtiments de production ; bonus/malus probabilistes liés à la charge réseau ; cinq types de dégâts ; points de capture ; entropie et surcadençage du réseau.
- Scripts dans `Assets/Script/`, un dossier par système (Building, IA, Network, Requisition, Unit, UI…). Plugin Odin Inspector.
- Vocabulaire : « renfort » plutôt que « vague ».

## Conventions du projet

- **ScriptableObjects** pour toutes les définitions de données, avec des fenêtres Editor custom pour créer les assets.
- Bâtiments : initialisation dans **`ABuildClass.Start()`** (`protected`). Un enfant qui déclare son propre `Start()` **doit appeler `base.Start()`**, sinon réquisition, durabilité et armure ne sont pas initialisées.
- **Une donnée = un seul propriétaire.** `IAManager` décide et possède ses données, les contrôleurs exécutent.
- Données par camp : classe C# `[Serializable]` (`NetworkData`, `RequisitionData`) tenue par un singleton unique et lue par `GetXxxData(faction)`, pas deux managers Joueur / IA dupliqués.
- `ReceiveOrder()` recrée un état à chaque appel : **jamais d'ordre à chaque frame**.
- Stats vs State : `UnitData` (SO) / `AUnitClass` (runtime). `UnitDestroyed()` ne fait que `Destroy(gameObject)` ; le cleanup va dans `OnDestroy()`.
- Scènes de test : `IATestScene` (aussi Build, Effect, Network, Unit). Outil éditeur `Tools/IA Debug`.

## Autres sources

- Notion : plans de test dans le Design Log (`Log Prog`) ; archive de l'ancien CLAUDE.md : « Archive : fichiers de suivi de session avant simplification ».
- Claude Docs : « Unifier les managers Joueur / IA », « Analyse des effets de surcharge réseau », « Contexte projet : CLAUDE.md court + doc d'état ».
- Le **board Miro** du Projet Claude d'origine n'est pas lisible depuis ici : demander son export si besoin.
