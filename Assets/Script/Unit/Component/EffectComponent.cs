using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EffectComponent : MonoBehaviour
{
    #region ATTRIBUTS
    [SerializeField] private AUnitClass unit = null;

    [SerializeField] private Image imageEffect = null;
    [SerializeField] private GameObject panelEffect = null;

    [SerializeField] private Coroutine currentEffect = null;
    #endregion

    #region PROPERTIES
    #endregion

    #region METHODE
    #region MONO
    #endregion MONO

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

        if (effect.EffectDuration > 0)
        {
            yield return new WaitForSeconds(effect.EffectDuration);
            ModifyStat(effect.StatAffected, 1);  //Annule l'effet

            imageEffect.sprite = effect.ImageEffect;
            panelEffect.SetActive(false);
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

    #endregion METHODE


}
