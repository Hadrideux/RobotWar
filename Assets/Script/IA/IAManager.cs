using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : Singleton<IAManager>
{
    #region ATTRIBUTS
    [SerializeField] private EIAState currentState = EIAState.NONE;

    [SerializeField] private List<AUnitClass> defenseUnitList = new List<AUnitClass>();
    [SerializeField] private List<AUnitClass> attackUnitList = new List<AUnitClass>();

    [Header("Attack")]
    [SerializeField] private IAAttackController attackController = null;
    [SerializeField] private AnimationCurve thresholdAttackUnitCurve = null;
    [Space(5)]
    [SerializeField] private int attackUnitThreshold = 5;
    [SerializeField] private int minAttackUnitThreshold = 10;
    [SerializeField] private int maxAttackUnitThreshold = 50;
    [Space(5)]
    [SerializeField] private int attackCount = 0;
    [SerializeField] private int maxAttackCount = 10;
    [Space(5)]  
    [SerializeField] private ABuildClass playerHQ = null;

    [Header("Defense")]
    [SerializeField] private IADefenseController defenseController = null;
    [SerializeField] private int defenseUnitThreshold = 10;
    [Space(5)]
    [SerializeField] private float baseUnderAttackWarning = 0f;
    [SerializeField] private float baseUnderAttackwarningRemaining = 0f;
    [Space(5)]
    [SerializeField] private bool baseUnderAttack = false;
    [SerializeField] private float responseTime = 0f;
    #endregion ATTRIBUTS

    #region PROPERTY
    public List<AUnitClass> AttackUnit => attackUnitList;
    public List<AUnitClass> DefenseUnit => defenseUnitList;

    public int AttackUnitThreshold => attackUnitThreshold;
    public int MaxAttackThreshold => maxAttackUnitThreshold;
    public int MinAttackThreshold => minAttackUnitThreshold;

    public int AttackCount => attackCount;
    public int MaxAttackCount => maxAttackCount;

    public bool BaseUnderAttack
    {
        get => baseUnderAttack;
        set => baseUnderAttack = value;
    }

    public ABuildClass PlayerHQ => playerHQ;
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
    private event Action onDefend = null;
    public event Action OnDefend
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
        UnitManager.Instance.OnUnitDestroyed += RemoveUnit;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState(currentState); 
    }

    void OnDestroy()
    {
        UnitManager.Instance.OnUnitProduced -= AddUnit;
        UnitManager.Instance.OnUnitDestroyed -= RemoveUnit;
    }
    private void OnApplicationQuit()
    {
        UnitManager.Instance.OnUnitProduced -= AddUnit;
        UnitManager.Instance.OnUnitDestroyed -= RemoveUnit;
    }
    #endregion MONO

    public void UpdateState(EIAState state)
    {
        currentState = state;

        switch (state)
        {
            case EIAState.IDLE:
                
                AttackAvailable();
                BaseUnderAttacked(responseTime);
                break;
            case EIAState.ATTACKING:
                AttackFinished();
                break;
            case EIAState.DEFENDING:

                BaseUnderAttacked(responseTime);
                break;
            default:
                break;
        }
    }
    public void AddUnit(AUnitClass unit)
    {
        if (defenseUnitList.Count < defenseUnitThreshold)
        {
            RefillBackLine(unit);
        }
        else
        {
            RefillFrontLine(unit);
        }
    }
    public void RemoveUnit(AUnitClass unit)
    {
        if (defenseUnitList.Contains(unit))
        {
            RemoveFromBackLine(unit);
        }
        
        if (attackUnitList.Contains(unit))
        {
            RemoveFromFrontLine(unit);
        }
    }
    public void RefillFrontLine(AUnitClass unit)
    {
        attackUnitList.Add(unit);
        
        if (currentState == EIAState.ATTACKING)
        {
            attackController.TransmitOrder(unit);
        }
    }
    public void RefillBackLine(AUnitClass unit)
    {
        defenseUnitList.Add(unit);
    }
    public void RemoveFromFrontLine(AUnitClass unit)
    {
        attackUnitList.Remove(unit);
    }
    public void RemoveFromBackLine(AUnitClass unit)
    {
        defenseUnitList.Remove(unit);
    }

    private void AttackFinished()
    {
        if (attackUnitList.Count == 0)
        {
            UpdateState(EIAState.IDLE);
        }
    }
    private void AttackAvailable()
    {
        if (attackUnitList.Count >= attackUnitThreshold)
        {
            currentState = EIAState.ATTACKING;

            attackController.LaunchAttack();

            attackCount++;
            HandleAttackThreshold();
        }
    }
    private void HandleAttackThreshold()
    {
        float progress = (float)attackCount / maxAttackCount;
        float shape = thresholdAttackUnitCurve.Evaluate(progress);
        attackUnitThreshold = Mathf.RoundToInt(Mathf.Lerp(minAttackUnitThreshold, maxAttackUnitThreshold, shape));
    }
    private void BaseUnderAttacked(float time)
    {
        if (!baseUnderAttack)
        {
            return;
        }
        else if (baseUnderAttackwarningRemaining >= baseUnderAttackWarning)
        {
            currentState = EIAState.IDLE;
            baseUnderAttackwarningRemaining = 0;
            baseUnderAttack = false;
        }
        else if (currentState != EIAState.DEFENDING)
        {
            currentState = EIAState.DEFENDING;
            baseUnderAttackwarningRemaining += time;

            if (onDefend != null)
            {
                onDefend();
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