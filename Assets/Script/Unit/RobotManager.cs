using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : Singleton<RobotManager>
{
    [Header("Unit Data")]
    [SerializeField] private RobotAirCraft _roboatAirCraft = null;
    [SerializeField] private RobotAntiAir  _robotAntiAir = null;
    [SerializeField] private RobotMecha _robotMecha = null;
    [SerializeField] private RobotTank _robotTank = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
