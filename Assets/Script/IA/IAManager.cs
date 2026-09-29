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
    [SerializeField] private AnimationCurve thresholdAttackUnitCurve = null;
    [SerializeField] private int attackThreshold = 5;
    [SerializeField] private int minAttackThreshold = 10;
    [SerializeField] private int maxAttackThreshold = 50;

    [SerializeField] private int attackCount = 0;
    [SerializeField] private int maxAttackCount = 10;

    [Header("Defense")]
    [SerializeField] private int defenseUnitThreshold = 10;
    [SerializeField] private float baseUnderAttackWarning = 0f;
    [SerializeField] private float baseUnderAttackwarningRemaining = 0f;
    [SerializeField] private bool baseUnderAttack = false;

    [SerializeField] private float responseTime = 0f;
    #endregion ATTRIBUTS

    #region PROPERTY
    public List<AUnitClass> AttackUnit => attackUnitList;
    public List<AUnitClass> DefenseUnit => defenseUnitList;

    public int AttackThreshold => attackThreshold;
    public int MaxAttackThreshold => maxAttackThreshold;
    public int MinAttackThreshold => minAttackThreshold;


    public int AttackCount => attackCount;
    public int MaxAttackCount => maxAttackCount;
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
        currentState = EIAState.IDLE;
        
        ComputeAttackThreshold();
        StartCoroutine(ChangeIAState());

        UnitManager.Instance.OnUnitProduced += AddUnit;
        UnitManager.Instance.OnUnitDestroyed += RemoveUnit;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator ChangeIAState()
    {
        while (true)
        {
            switch (currentState)
            {
                case EIAState.IDLE:
                    currentState = EIAState.IDLE;
                    AttackAvailable();
                    BaseAttacked(responseTime);
                    yield return new WaitForSeconds(responseTime);
                    break;
                case EIAState.ATTACKING:
                    AttackInProgress();
                    currentState = EIAState.ATTACKING;
                    yield return new WaitForSeconds(responseTime);
                    break;
                case EIAState.DEFENDING:
                    BaseAttacked(responseTime);
                    yield return new WaitForSeconds(responseTime);
                    break;
                default:
                    yield return new WaitForSeconds(responseTime);
                    break;
            }
        }
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

    private void AddUnit(AUnitClass unit)
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

    private void RemoveUnit(AUnitClass unit)
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

    private void AttackInProgress()
    {
        if (attackUnitList.Count == 0)
        {
            currentState = EIAState.IDLE;
        }
        else if (currentState == EIAState.ATTACKING)
        {
            if (onAttack != null)
            {
                onAttack();
            }
        }
    }

    private void AttackAvailable()
    {
        if (attackUnitList.Count >= attackThreshold)
        {
            currentState = EIAState.ATTACKING;
            attackCount ++;

            ComputeAttackThreshold();            
        }
        else if (attackUnitList.Count == 0)
        {
            currentState = EIAState.IDLE;

        }
    }

    private void ComputeAttackThreshold()
    {
        float progress = (float)attackCount / maxAttackCount;
        float shape = thresholdAttackUnitCurve.Evaluate(progress);
        attackThreshold = Mathf.RoundToInt(Mathf.Lerp(minAttackThreshold, maxAttackThreshold, shape));
    }

    private void BaseAttacked(float time)
    {
        if (!baseUnderAttack)
        {
            return;
        }
        else if (baseUnderAttackwarningRemaining >= baseUnderAttackWarning)
        {
            currentState = EIAState.IDLE;
            baseUnderAttackwarningRemaining = 0;
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