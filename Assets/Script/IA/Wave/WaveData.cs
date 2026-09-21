using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeReference] private Dictionary<AUnitClass, int> unitPrefab = new Dictionary<AUnitClass, int>();
    [SerializeField] private int unitCount = 0;

    [SerializeField] private float statMultiplierHealth = 0;
    [SerializeField] private float statMultiplierDamage = 0;

    public Dictionary<AUnitClass, int> UnitPrefab => unitPrefab;
    public int UnitCount => unitCount;
    public float StatMulitiplierHealth => statMultiplierHealth;
    public float StatMultiplierDamage => statMultiplierDamage;
}