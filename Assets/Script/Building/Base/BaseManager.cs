using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : Singleton<BaseManager>
{
    #region ATTRIBUTS

    [Header("Base Life")]
    [SerializeField] private float _baseHealthMax = 100;
    [SerializeField] private float _baseHealthMin = 0;
    [SerializeField] private float _baseHealthCurrent = 0;

    [Header("Ressource")]
    [SerializeField] private int _allyRessource = 0;
    [SerializeField] private int _ennemyRessource = 0;

    #endregion ATTRIBUTS

    #region PROPERTIES

    public float BaseHealthCurrent
    {
        get => _baseHealthCurrent;
        set
        {
            _baseHealthCurrent = value;
        }
    }

    public int AllyRessource
    {
        get => _allyRessource;
        set
        {
            _allyRessource = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }

    public int EnnemyRessource
    {
        get => _ennemyRessource;
        set
        {
            _ennemyRessource = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }

    #endregion PROPERTIES

    #region EVENT
    #endregion EVENT

    #region MONO

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion MONO
}