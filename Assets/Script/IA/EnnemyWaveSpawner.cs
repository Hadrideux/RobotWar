using UnityEngine;

public class EnnemyWaveSpawner : MonoBehaviour
{
    #region ATTRIBUTS
    [Header("Wave")]
    [SerializeField] private WaveData[] waveData = null;
    [SerializeField] private int CurrentWaveIndex = 0;

    [Header("Wave Spawn")]
    [SerializeField] private Transform[] spawnPoint = null;

    [Header("Wave Timer")]
    [SerializeField] private int defaultWaveTimer = 0;
    [SerializeField] private float nextWaveTimer = 0;
    [SerializeField] private float currentWaveTimer = 0;
    [SerializeField] private float multiplierWaveTimer = 0;
    [SerializeField] private int minWaveTimer = 0;
    #endregion

    #region PROPERTIES

    #endregion

    #region METHODES
    #region MONO
    // Use this for initialization
    void Start()
    {
        nextWaveTimer = defaultWaveTimer;

    }

    // Update is called once per frame
    void Update()
    {
        CooldownNewWave();
    }
    #endregion

    private void NextWaveSpawning()
    {

    }

    private void CooldownNewWave()
    {

        if(currentWaveTimer >= nextWaveTimer)
        {
            Debug.Log("New Wave Spawning");

            NextWaveSpawning();
            
            currentWaveTimer = 0;
            nextWaveTimer = Mathf.Clamp(nextWaveTimer * multiplierWaveTimer, minWaveTimer, defaultWaveTimer);            
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