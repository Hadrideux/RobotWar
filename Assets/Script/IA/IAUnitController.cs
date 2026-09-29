using UnityEngine;

public class IAUnitController : MonoBehaviour
{
    #region METHODES
    #region MONO
    // Use this for initialization
    void Start()
    {
        IAManager.Instance.OnAttack += Attack;
        IAManager.Instance.OnDefend += DefendBase;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        IAManager.Instance.OnAttack -= Attack;
        IAManager.Instance.OnDefend -= DefendBase;
    }
    void OnApplicationQuit()
    {
        IAManager.Instance.OnAttack -= Attack;
        IAManager.Instance.OnDefend -= DefendBase;
    }
    #endregion MONO
    private void Attack()
    {
        foreach (AUnitClass unit in IAManager.Instance.AttackUnit)
        {

        }
    }

    private void DefendBase()
    {
        foreach (AUnitClass unit in IAManager.Instance.DefenseUnit)
        {

        }
    }
    #endregion METHODES
}