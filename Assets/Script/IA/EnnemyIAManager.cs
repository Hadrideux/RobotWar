using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyIAManager : Singleton<EnnemyIAManager>
{
    #region ATTRIBUTS
    [SerializeField] private EIAState currentState = EIAState.NONE;

    [SerializeField] private List<AUnitClass> defenseUnitList = new List<AUnitClass>();
    [SerializeField] private List<AUnitClass> attackUnitList = new List<AUnitClass>();
    [SerializeField] private List<AUnitClass> currentAttackList = new List<AUnitClass>();

    [SerializeField] private int defenseUnitThreshold = 0;
    [SerializeField] private int attackUnitThreshold = 0;

    [SerializeField] private float baseUnderAttackWarning = 0f;
    [SerializeField] private float baseUnderAttackwarningRemaining = 0f;
    [SerializeField] private bool baseUnderAttack = false;

    [SerializeField] private float responseTime = 0f;

    #endregion ATTRIBUTS

    #region PROPERTY
    public List<AUnitClass> AttackUnit => attackUnitList;
    public List<AUnitClass> DefenseUnit => defenseUnitList;


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
        if(defenseUnitList.Count < defenseUnitThreshold)
        {
            defenseUnitList.Add(unit);
        }
        else
        {
            attackUnitList.Add(unit);
        }
    }

    private void RemoveUnit(AUnitClass unit)
    {
        if (defenseUnitList.Contains(unit))
        {
            defenseUnitList.Remove(unit);
        }
        else if (attackUnitList.Contains(unit) && currentAttackList.Contains(unit))
        {
            attackUnitList.Remove(unit);
            currentAttackList.Remove(unit);
        }
    }

    private void AttackInProgress()
    {
        if(currentAttackList.Count == 0 )
        {
            currentState = EIAState.IDLE;
        }
        else if(currentState == EIAState.ATTACKING)
        {
            if(onAttack != null)
            {
                onAttack();
            }
        }
    }
    private void AttackAvailable()
    {
        if(attackUnitList.Count > attackUnitThreshold)
        {
            currentState = EIAState.ATTACKING;
            currentAttackList = attackUnitList;
        }
        else if(currentAttackList.Count == 0)
        {
            currentState = EIAState.IDLE;
            //attackUnitThreshold += attackUnitThreshold;
        }
    }

    private void BaseAttacked(float time)
    {
        if(!baseUnderAttack)
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