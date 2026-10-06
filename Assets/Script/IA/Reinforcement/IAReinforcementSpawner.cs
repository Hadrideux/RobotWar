using System.Collections.Generic;
using UnityEngine;

public class IAReinforcementSpawner : MonoBehaviour
{
    #region ATTRIBUTS

    [SerializeField] private IAManager IAManager = null;
    [Header("Reinforcement")]
    [SerializeField] private ReinforcementData[] reinforcementDatas = null;
    [SerializeField] private int currentReinforcementIndex = 0;

    [Header("Reinforcement Spawn")]
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] protected Transform unitContainer = null;

    [Header("Reinforcement Timer")]
    [SerializeField] private AnimationCurve reinforcementDurationCurve = null;
    [Space(5)]
    [SerializeField] private int defaultReinforcementTimer = 60;
    [Space(5)]
    [SerializeField] private float reinforcementDuration = 0;
    [SerializeField] private float reinforcementTimer = 0;
    [SerializeField] private int minReinforcementTimer = 20;
    #endregion

    #region PROPERTIES

    #endregion

    #region METHODES
    #region MONO
    // Use this for initialization
    void Start()
    {
        reinforcementDuration = defaultReinforcementTimer;
        IAManager = IAManager.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        ReinforcementCooldown();
    }
    #endregion

    private void HandleReinforcementDuration()
    {
        float progress = (float)IAManager.LaunchedAttackCount / IAManager.AttackCountAtMaxThreshold;
        float shape = reinforcementDurationCurve.Evaluate(progress);
        reinforcementDuration = Mathf.Lerp(defaultReinforcementTimer, minReinforcementTimer, shape);
    }

    public void ReinforcementSpawning()
    {
        int missing = Mathf.Max(0, IAManager.AttackThreshold - IAManager.AttackUnits.Count);
        ReinforcementData data = reinforcementDatas[currentReinforcementIndex];

        for (int i = 0; i < missing; i++)
        {
            AUnitClass unitPrefab = RandomReinforcementUnit(data);

            if (unitPrefab == null)
            {
                return;
            }
            else
            {
                AUnitClass reinforcement = Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity, unitContainer);
                reinforcement.FactionObject = EFactionType.IA;
                //reinforcement.CurrentHealth *= waveData[i].StatMulitiplierHealth;
                //reinforcement. *= waveData[i].;                
                IAManager.AddToAttack(reinforcement);
            }
        }

        NextReinforcementData();
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
    private void ReinforcementCooldown()
    {
        if(reinforcementTimer >= reinforcementDuration)
        {
            reinforcementTimer = 0;
            ReinforcementSpawning();
            HandleReinforcementDuration();
        }
        else
        {
            reinforcementTimer += Time.deltaTime;
        }
    }

    private void NextReinforcementData()
    {
        currentReinforcementIndex = Mathf.Clamp(currentReinforcementIndex + 1, 0, reinforcementDatas.Length - 1);
    }
    #endregion
}