using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] private AUnitClass[] unitPrefab = null;
    [SerializeField] private int[] unitCount = null;

    [SerializeField] private float delayBetweenSpawns = 0;
    [SerializeField] private float statMultiplierHealth = 0;
    [SerializeField] private float statMultiplierDamage = 0;

    public AUnitClass[] UnitPrefab => unitPrefab;
    public int[] UnitCount => unitCount;
    public float DelayBetweenHealth => delayBetweenSpawns;
    public float StatMulitiplierHealth => statMultiplierHealth;
    public float StatMultiplierDamage => statMultiplierDamage;
}