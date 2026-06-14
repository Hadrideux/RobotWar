using UnityEngine;

[CreateAssetMenu(fileName = "ErrorNetworkData", menuName = "Scriptable Objects/ErrorNetworkData")]
public class NetworkEffectData : ScriptableObject
{
    [SerializeField] private string effectName = "";
    [SerializeField] private EStatEffectedType statAffected = EStatEffectedType.NONE;
    [SerializeField] private ECategoryEffectType categoryEffect = ECategoryEffectType.NONE;

    [Range(0f, 1f)]
    [SerializeField] private float thresholdEffect = 0;

    [Range(0f, 2f)]
    [SerializeField] private float statModifier = 0;
    [SerializeField] private int duration = 0;

    [SerializeField] private Sprite imageEffect = null;

    [Header("Trigger Effect"), Range(0f, 1f)]
    [SerializeField] private float procProbability = 0;
    [SerializeField] private int cooldown = 0;


    #region PROPERTIES
    public string EffectName
    {
        get => effectName;
        set => effectName = value;
    }
    public EStatEffectedType StatAffected
    {
        get => statAffected;
        set => statAffected = value;
    }
    public ECategoryEffectType CategoryEffect
    {
        get => categoryEffect;
        set => categoryEffect = value;
    }

    public float ThresholdEffect
    {
        get => thresholdEffect;
        set => thresholdEffect = value;
    }
    public int EffectDuration
    {
        get => duration;
        set => duration = value;
    }

    public float ProcProbability
    {
        get => procProbability;
        set => procProbability = value;
    }

    public float StatModifier
    {
        get => statModifier;
        set => statModifier = value;
    }

    public int Cooldown
    {
        get => cooldown;
        set => cooldown = value;
    }

    public Sprite ImageEffect
    {
        get => imageEffect;
        set => imageEffect = value;
    }
    #endregion PROPERTIES
}
