using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReinforcementData", menuName = "Scriptable Objects/ReinforcementData")]
public class ReinforcementData : SerializedScriptableObject
{
    [OdinSerialize] private Dictionary<AUnitClass, int> reinforcementUnit = new Dictionary<AUnitClass, int>();
    

    [SerializeField] private float statMultiplierHealth = 0;
    [SerializeField] private float statMultiplierDamage = 0;

    public Dictionary<AUnitClass, int> ReinforcementUnit => reinforcementUnit;
    public float StatMulitiplierHealth => statMultiplierHealth;
    public float StatMultiplierDamage => statMultiplierDamage;
}