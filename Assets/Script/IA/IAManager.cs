using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : Singleton<IAManager>
{
    #region ATTRIBUTS
    [SerializeField] private EIAState currentState = EIAState.NONE;

    [SerializeField] private List<AUnitClass> defenseUnits = new List<AUnitClass>();
    [SerializeField] private List<AUnitClass> attackUnits = new List<AUnitClass>();

    [Header("Attack")]
    [SerializeField] private IAAttackController attackController = null;
    [SerializeField] private AnimationCurve attackThresholdCurve = null;
    [Space(5)]
    [SerializeField] private int attackThreshold = 5;
    [SerializeField] private int minAttackThreshold = 10;
    [SerializeField] private int maxAttackThreshold = 50;
    [Space(5)]
    [SerializeField] private int launchedAttackCount = 0;
    [SerializeField] private int attackCountAtMaxThreshold = 10;
    [Space(5)]  
    [SerializeField] private ABuildClass playerHQ = null;

    [Header("Defense")]
    [SerializeField] private IADefenseController defenseController = null;
    [SerializeField] private int defenseThreshold = 10;
    [Space(5)]
    [SerializeField] private float calmDuration = 5f;
    [SerializeField] private float calmTimer = 0f;
    [Space(5)]
    [SerializeField] private bool isBaseUnderAttack = false;
    #endregion ATTRIBUTS

    #region PROPERTY
    public List<AUnitClass> AttackUnits => attackUnits;
    public int AttackThreshold => attackThreshold;

    public List<AUnitClass> DefenseUnits => defenseUnits;
    public int DefenseThreshold => defenseThreshold;

    public int LaunchedAttackCount => launchedAttackCount;
    public int AttackCountAtMaxThreshold => attackCountAtMaxThreshold;

    public ABuildClass PlayerHQ
    {
        get => playerHQ;
        set => playerHQ = value;
    }

    public EIAState CurrentState => currentState;
    public bool IsBaseUnderAttack => isBaseUnderAttack;
    #endregion PROPERTY

    #region EVENT
    private event Action onBaseUnderAttack = null;
    public event Action OnBaseUnderAttack
    {
        add
        {
            onBaseUnderAttack -= value;
            onBaseUnderAttack += value;
        }
        remove
        {
            onBaseUnderAttack -= value;
        }
    }

    private event Action<AUnitClass> onUnitJoinAttack = null;
    public event Action<AUnitClass> OnUnitJoinAttack
    {
        add
        {
            onUnitJoinAttack -= value;
            onUnitJoinAttack += value;
        }
        remove
        {
            onUnitJoinAttack -= value;
        }
    }
    private event Action onAttack = null;
    public event Action OnAttack
    {
        add
        {
            onAttack -= value;
            onAttack += value;
        }
        remove
        {
            onAttack -= value;
        }
    }
    private event Action<ABuildClass> onDefend = null;
    public event Action<ABuildClass> OnDefend
    {
        add
        {
            onDefend -= value;
            onDefend += value;
        }
        remove
        {
            onDefend -= value;
        }
    }

    #endregion  EVENT

    #region METHODES
    #region MONO
    // Use this for initialization
    void Start()
    {
        UpdateState(EIAState.IDLE);

        HandleAttackThreshold();

        UnitManager.Instance.OnUnitProduced += AddUnit;

        BuildManager.Instance.OnBuildingAttacked += BaseUnderAttacked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState(currentState);

        if (isBaseUnderAttack)
        {
            if (calmTimer >= calmDuration)
            {
                calmTimer = 0;
                isBaseUnderAttack = false;
            }
            else
            {
                calmTimer += Time.deltaTime;
            }
        }
        
    }

    void OnDestroy()
    {
        UnitManager.Instance.OnUnitProduced -= AddUnit;

        BuildManager.Instance.OnBuildingAttacked -= BaseUnderAttacked;
    }
    private void OnApplicationQuit()
    {
        UnitManager.Instance.OnUnitProduced -= AddUnit;

        BuildManager.Instance.OnBuildingAttacked -= BaseUnderAttacked;
    }
    #endregion MONO

    public void UpdateState(EIAState state)
    {
        currentState = state;

        switch (state)
        {
            case EIAState.IDLE:
                
                AttackAvailable();
                break;
            case EIAState.ATTACKING:

                AttackFinished();
                break;
            default:
                break;
        }
    }
    public void AddUnit(AUnitClass unit)
    {
        if (defenseUnits.Count < defenseThreshold)
        {
            AddToDefense(unit);
        }
        else
        {
            AddToAttack(unit);
        }
    }
    public void RemoveUnit(AUnitClass unit)
    {
        RemoveFromDefense(unit);
        RemoveFromAttack(unit);
    }
    public void AddToAttack(AUnitClass unit)
    {
        attackUnits.Add(unit);
        
        if (currentState == EIAState.ATTACKING)
        {
            if(onUnitJoinAttack != null)
            {
                onUnitJoinAttack(unit);
            }
        }
    }
    public void AddToDefense(AUnitClass unit)
    {
        defenseUnits.Add(unit);
    }
    public void RemoveFromAttack(AUnitClass unit)
    {
        attackUnits.Remove(unit);
    }
    public void RemoveFromDefense(AUnitClass unit)
    {
        defenseUnits.Remove(unit);
    }

    private void AttackFinished()
    {
        if (attackUnits.Count == 0)
        {
            UpdateState(EIAState.IDLE);
        }
    }
    private void AttackAvailable()
    {
        if (attackUnits.Count >= attackThreshold)
        {
            UpdateState(EIAState.ATTACKING);

            if (onAttack != null)
            {
                onAttack();
            }

            launchedAttackCount++;
            HandleAttackThreshold();
        }
    }
    private void HandleAttackThreshold()
    {
        float progress = (float)launchedAttackCount / attackCountAtMaxThreshold;
        float shape = attackThresholdCurve.Evaluate(progress);
        attackThreshold = Mathf.RoundToInt(Mathf.Lerp(minAttackThreshold, maxAttackThreshold, shape));
    }

    private void BaseUnderAttacked(ABuildClass build)
    {
        if(build.FactionObject != EFactionType.IA)
        {
            return;
        }

        calmTimer = 0;

        if (!isBaseUnderAttack)
        {
            isBaseUnderAttack = true;

            if (onDefend != null)
            {
                onDefend(build);
            }
        }
    }
    #endregion METHODES
}

public enum EIAState
{
    NONE,
    IDLE,
    ATTACKING,
    DEFENDING,
}