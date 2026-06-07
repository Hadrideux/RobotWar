using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;
    [SerializeField] private UnitManager unitManager = null;

    [SerializeField] private Dictionary<NetworkEffectData, float> cooldownEffect = new Dictionary<NetworkEffectData, float>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        networkManager = NetworkManager.Instance;
        unitManager = UnitManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        HandleNetworkEffect();
    }

    public void HandleNetworkEffect()
    {
        NetworkEffectData[] effect = networkManager.NetworkEffect;
        for(int i=0; i <= effect.Length-1; i++)
        {
            float threshold = effect[i].ThresholdEffect;
            if(threshold <= NetworkPercentLoad())
            {
                Debug.Log($"Effect cast : {effect[i]}");
                ApplyEffect(effect[i]);
            }
            else
            {
                Debug.Log($"effet : {effect[i].EffectName} n'est pas actif");
            }
        }
    }
    public void Shuffle(List<AUnitClass> list)
    {
        for (int i = 0; i < list.Count; i++) 
        {
            int j = Random.Range(0, i+1);
            (list[i], list[j]) = (list[j], list[i]);
        }                                             
    }
    public void ApplyEffect(NetworkEffectData effect)
    {
        if(!cooldownEffect.ContainsKey(effect))
            cooldownEffect.Add(effect, effect.Cooldown);

        if (cooldownEffect.ContainsKey(effect) && Time.time < cooldownEffect[effect])
        {
            Debug.Log($"Effet : {effect.EffectName} n'est pas prêt");
            return;
        }
        if(Random.Range(0f, 1f) > effect.ProcProbability)
        {
            Debug.Log($"Effet : {effect.EffectName} n'as pas proc");
            return;
        }

        List<AUnitClass> unitEligible = unitManager.GetElligibleUnits(effect.StatAffected);

        if (unitEligible.Count == 0) 
            return;

        int count = Mathf.Max(1, Mathf.RoundToInt(unitEligible.Count * effect.ProcProbability));
        Debug.Log(count);

        Shuffle(unitEligible);
        for (int i = 0; i < count; i++)
            unitEligible[i].EffectComponent.ApplyIAEffect(effect);

        cooldownEffect[effect] = Time.time + effect.Cooldown;
    }
    public float NetworkPercentLoad()
    {
        float currentLoad = networkManager.CurrentLoad;
        int maxLoad = networkManager.CurrentMaxLoad;

        return currentLoad / maxLoad;        
    }
}
