using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{

    #region ATTRIBUTS
    [ShowInInspector] private NetworkData network = null;
    [SerializeField] private UnitManager unitManager = null;

    [SerializeField] private Dictionary<NetworkEffectData, float> cooldownEffect = new Dictionary<NetworkEffectData, float>();
    [SerializeField] private float currentCooldownEffect = 0f;

    #endregion ATTRIBUTS
    #region METHODES
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        network = NetworkManager.Instance.GetNetworkData(EFactionType.ALLY);
        unitManager = UnitManager.Instance;

        network.OnLoadChange += HandleNetworkEffect;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        network.OnLoadChange -= HandleNetworkEffect;
    }

    void OnApplicationQuit()
    {
        network.OnLoadChange -= HandleNetworkEffect;
    }
    #endregion MONO

    public void HandleNetworkEffect(float percent)
    {
        NetworkEffectData[] effect = NetworkManager.Instance.NetworkEffect;
        for (int i = 0; i <= effect.Length - 1; i++)
        {
            float threshold = effect[i].ThresholdEffect;
            if (threshold <= percent)
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
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    public void ApplyEffect(NetworkEffectData effect)
    {
        if (!cooldownEffect.ContainsKey(effect))
            cooldownEffect.Add(effect, effect.Cooldown);

        if (cooldownEffect.ContainsKey(effect) && currentCooldownEffect > 0)
        {
            Debug.Log($"Effet : {effect.EffectName} n'est pas prêt");
            currentCooldownEffect -= Time.deltaTime;
            return;
        }
        if (Random.Range(0f, 1f) > effect.ProcProbability)
        {
            Debug.Log($"Effet : {effect.EffectName} n'as pas proc");
            currentCooldownEffect -= Time.deltaTime;
            return;
        }

        List<AUnitClass> unitEligible = unitManager.GetElligibleUnits(effect.StatAffected);

        if (unitEligible.Count == 0)
            return;

        int count = Mathf.Max(1, Mathf.RoundToInt(unitEligible.Count * effect.ProcProbability));

        Shuffle(unitEligible);
        for (int i = 0; i < count; i++)
            unitEligible[i].EffectComponent.ApplyIAEffect(effect);

        currentCooldownEffect = effect.Cooldown;
    }
    #endregion METHODES

}
