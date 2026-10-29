using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{

    #region ATTRIBUTS
    [Header("Manager")]
    [SerializeField] private UnitManager unitManager = null;
    [Space(5)]
    [Header("Data")]
    [ShowInInspector] private NetworkData networkData = null;
    [SerializeField] private EFactionType controllerFaction = EFactionType.NONE;
    [Space(5)]
    [Header("Effect")]
    [SerializeReference] private Dictionary<NetworkEffectData, float> effectInCooldown = new Dictionary<NetworkEffectData, float>();
    [SerializeField] private float procInterval = 0.25f;
   
    #endregion ATTRIBUTS

    #region METHODES
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        networkData = NetworkManager.Instance.GetNetworkData(controllerFaction);
        unitManager = UnitManager.Instance;

        StartCoroutine(NetworkLoad());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEffectCooldown();
    }
    #endregion MONO

    #region ENUMERATOR

    private IEnumerator NetworkLoad()
    {
        while (true)
        {
            HandleNetworkEffect(networkData.PercentLoad);
            yield return new WaitForSeconds(NetworkManager.Instance.ProcInterval);
        }
    }
    #endregion ENUMERATOR

    public void HandleNetworkEffect(float percent)
    {
        for (int i = 0; i <= NetworkManager.Instance.NetworkEffect.Length - 1; i++)
        {
            float threshold = NetworkManager.Instance.NetworkEffect[i].ThresholdEffect;
            if (threshold <= percent)
            {
                ApplyEffect(NetworkManager.Instance.NetworkEffect[i]);
            }
        }
    }

    public void Shuffle(List<AUnitClass> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void ApplyEffect(NetworkEffectData effect)
    {
        if (effectInCooldown.ContainsKey(effect))
            return; 

        if (Random.Range(0f, 1f) > effect.ProcProbability)
            return; 
        effectInCooldown.Add(effect, effect.Cooldown); 
        
        List<AUnitClass> unitEligible = unitManager.GetElligibleUnits(EFactionType.ALLY, effect.StatAffected);
        if (unitEligible.Count == 0)
            return;

        int count = Random.Range(1, unitEligible.Count + 1);

        Shuffle(unitEligible);
        for (int i = 0; i < count; i++)
            unitEligible[i].EffectComponent.ApplyIAEffect(effect);
    }

    private void UpdateEffectCooldown()
    {
        foreach(NetworkEffectData effect in new List<NetworkEffectData>(effectInCooldown.Keys))
        {
            effectInCooldown[effect] -= Time.deltaTime;

            if (effectInCooldown[effect] <= 0f)
            {
                effectInCooldown.Remove(effect);
            }
        }
    }
    #endregion METHODES

}
