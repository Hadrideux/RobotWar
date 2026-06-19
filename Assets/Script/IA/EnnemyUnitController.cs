using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyUnitController : MonoBehaviour
{
    #region METHODES
    #region MONO
    // Use this for initialization
    void Start()
    {
        EnnemyIAManager.Instance.OnAttack += Attack;
        EnnemyIAManager.Instance.OnDefend += DefendBase;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        EnnemyIAManager.Instance.OnAttack -= Attack;
        EnnemyIAManager.Instance.OnDefend -= DefendBase;
    }
    void OnApplicationQuit()
    {
        EnnemyIAManager.Instance.OnAttack -= Attack;
        EnnemyIAManager.Instance.OnDefend -= DefendBase;
    }
    #endregion MONO
    private void Attack()
    {
        foreach (AUnitClass unit in EnnemyIAManager.Instance.AttackUnit)
        {

        }
    }

    private void DefendBase()
    {
        foreach (AUnitClass unit in EnnemyIAManager.Instance.DefenseUnit)
        {

        }
    }
    #endregion METHODES


}