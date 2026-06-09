using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    [SerializeField] private Transform spawner = null;
    [SerializeField] private Transform unitContainer = null;

    [SerializeField] private NetworkManager networkManager = null;


    #region PROPERTIES

    #endregion PROPERTIES

    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnUnit(AUnitClass spawnUnit)
    {
        Instantiate(spawnUnit, spawner.position, Quaternion.identity, unitContainer);
    }
}
