using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EffectComponent : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] protected CharacterManager characterManager = null;

    [SerializeField] private AUnitClass unit = null;

    [SerializeField] protected Image imageEffect = null;
    [SerializeField] protected GameObject panelEffect = null;


    void Start()
    {
        characterManager = CharacterManager.Instance;
    }
    public void ApplyIAEffect(NetworkEffectData effect)
    {
        //L'unité applique l'effet sur ses stats locales
        StartCoroutine(ApplyTemporaryEffect(effect));
    }

    private IEnumerator ApplyTemporaryEffect(NetworkEffectData effect)
    {
        //Applique le multiplicateur sur la stat concernée
        ModifyStat(effect.StatAffected, effect.StatModifier);

        //Affichage de l'UI de l'effet en cours sur l'unité
        imageEffect.sprite = effect.ImageEffect;
        panelEffect.SetActive(true);
        DisplayEffectUI(characterManager.CharacterController.transform.position);

        if (effect.EffectDuration > 0)
        {
            yield return new WaitForSeconds(effect.EffectDuration);
            ModifyStat(effect.StatAffected, 1);  //Annule l'effet

            imageEffect.sprite = effect.ImageEffect;
            panelEffect.SetActive(false);
            DisplayEffectUI(characterManager.CharacterController.transform.position); ;
        }
    }

    public bool IsEligibleFor(EStatEffectedType effectType)
    {
        switch (effectType)
        {
            case EStatEffectedType.SPEED:
                return unit.UnitData.UnitType != EUnitType.AIRCRAFT;
            default:
                return true;
        }
    }

    public void ModifyStat(EStatEffectedType type, float modifier)
    {
        switch (type)
        {
            case EStatEffectedType.SPEED:
                unit.NavMeshAgent.speed = unit.UnitData.MaxSpeed * modifier;
                break;

            default:
                break;
        }
    }
    public void DisplayEffectUI(Vector3 characterPosition)
    {
        //Quaternion targetRotation = Quaternion.LookRotation(direction);

        panelEffect.transform.LookAt(characterPosition);
    }
}
