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

    [SerializeField] private int defenseUnitThreshold = 0;
    [SerializeField] private int attackUnitThreshold = 0;

    [SerializeField] private float baseUnderAttackWarning = 0f;
    [SerializeField] private float baseUnderAttackwazrningRemaining = 0f;

    #endregion ATTRIBUTS

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
        switch (currentState)
        {
            case EIAState.IDLE:
                currentState = EIAState.IDLE;
                yield return new WaitForSeconds(1);
                break;
            case EIAState.ATTACKING:
                AttackAvailable();
                currentState = EIAState.ATTACKING;                
                yield return new WaitForSeconds(1);
                break;
            case EIAState.DEFENDING:
                currentState = EIAState.DEFENDING;
                BaseAttacked();
                yield return new WaitForSeconds(1);
                break;
            default:
                yield return new WaitForSeconds(1);
                break;
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
        else if (attackUnitList.Contains(unit))
        {
            attackUnitList.Remove(unit);
        }
    }

    private void AttackAvailable()
    {
        if(attackUnitList.Count > attackUnitThreshold && onAttack != null)
        {
            currentState = EIAState.ATTACKING;
            onAttack();
        }
        else
        {
            currentState = EIAState.IDLE;
        }
    }

    private bool BaseAttacked()
    {
        if (baseUnderAttackwazrningRemaining >= baseUnderAttackWarning)
        {
            currentState = EIAState.IDLE;
            baseUnderAttackwazrningRemaining = 0;

            return false;
        }
        else
        {
            currentState = EIAState.DEFENDING;
            baseUnderAttackwazrningRemaining += Time.deltaTime;

            if(onBaseUnderAttack != null)
            {
                onBaseUnderAttack();
            }

            return true;
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