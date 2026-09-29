using System.Collections.Generic;
using UnityEngine;

public class IAReinforcementSpawner : MonoBehaviour
{
    #region ATTRIBUTS

    [SerializeField] private IAManager IAManager = null;
    [Header("Wave")]
    [SerializeField] private ReinforcementData[] reinforcementDatas = null;
    [SerializeField] private int currentWaveIndex = 0;

    [Header("Wave Spawn")]
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] protected Transform unitContainer = null;

    [Header("Wave Timer")]
    [SerializeField] private AnimationCurve thresholdDeltaTimer = null;
    [SerializeField] private int defaultWaveTimer = 60;
    [SerializeField] private int minWaveTimer = 20;
    [SerializeField] private float nextWaveTimer = 0;
    [SerializeField] private float currentWaveTimer = 0;
    #endregion

    #region PROPERTIES

    #endregion

    #region METHODES
    #region MONO
    // Use this for initialization
    void Start()
    {
        nextWaveTimer = defaultWaveTimer;
        IAManager = IAManager.Instance;

    }

    // Update is called once per frame
    void Update()
    {

        CooldownNewWave();
    }
    #endregion

    private void ReinforcementUnitTreshold()
    {
        float progress = (float)IAManager.AttackCount / IAManager.MaxAttackCount;
        float shape = thresholdDeltaTimer.Evaluate(progress);
        nextWaveTimer = Mathf.Lerp(defaultWaveTimer, minWaveTimer, shape);
    }

    private void WaveSpawning()
    {
        int missing = Mathf.Max(IAManager.AttackThreshold - IAManager.AttackUnit.Count, 0);
        ReinforcementData data = reinforcementDatas[currentWaveIndex];
           
        for(int i = 0; i < missing; i++)
        {
            AUnitClass unit = RandomReinforcementUnit(data);

            if (unit  == null)
            {
                currentWaveIndex++;
                return;

            }
            else
            {
                AUnitClass reinforcement = Instantiate(unit, spawnPoint.position, Quaternion.identity, unitContainer);
                reinforcement.FactionObject = EFactionType.IA;
                //reinforcement.CurrentHealth *= waveData[i].StatMulitiplierHealth;
                IAManager.RefillFrontLine(reinforcement);
            }
        }
        
        currentWaveIndex = Mathf.Clamp(currentWaveIndex + 1, 0, reinforcementDatas.Length - 1);
    }

    private AUnitClass RandomReinforcementUnit(ReinforcementData reinforcementData)
    {
        int totalweight = 0;
        int roll = 0;
        
        foreach (int weight in reinforcementData.ReinforcementUnit.Values)
        {
            totalweight += weight;

            
        }

        roll = Random.Range(0, totalweight);

        foreach (KeyValuePair<AUnitClass, int> pair in reinforcementData.ReinforcementUnit)
        {
            roll -= pair.Value;

            if(roll < 0)
            {
                return pair.Key;
            }
        }

        return null;
    }
    private void CooldownNewWave()
    {
        if(currentWaveTimer >= nextWaveTimer)
        {
            WaveSpawning();
            
            currentWaveTimer = 0;
            ReinforcementUnitTreshold();
        }
        else
        {
            currentWaveTimer += Time.deltaTime;
        }
    }

    private void SetupNextWave()
    {

    }
    #endregion
}