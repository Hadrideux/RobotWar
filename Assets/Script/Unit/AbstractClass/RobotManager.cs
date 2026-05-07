using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : Singleton<RobotManager>
{
    #region ATTRIBUTS

    [Header("Unit Data")]
    //[SerializeField] protected EUnitType _unitType = EUnitType.NONE;

    [SerializeField] private RobotAirCraft _roboatAirCraft = null;
    [SerializeField] private RobotAntiAir  _robotAntiAir = null;
    [SerializeField] private RobotMecha _robotMecha = null;
    [SerializeField] private RobotTank _robotTank = null;

    #endregion ATTRIBUTS

    #region PROPERTIES

    public RobotAirCraft RobotAirCraft => _roboatAirCraft;
    public RobotAntiAir RobotAntiAir => _robotAntiAir;
    public RobotMecha RobotMecha => _robotMecha;
    public RobotTank RobotTank => _robotTank;

    #endregion PROPERTIES

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
