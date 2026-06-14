using UnityEngine;

public abstract class ABuildClass : MonoBehaviour, ISelectable
{
    #region ATTRIBUTS

    [Header("Component")]
    [SerializeField] protected PlaceableObjectComponent placeableComponent = null;

    [SerializeField] protected GameObject selectionObject = null;

    [Header("Statistique")]
    [SerializeField] protected BuildData buildData = null;

    [SerializeField] protected float currentDurability = 0;
    [SerializeField] protected int currentArmor = 0;

    [Header("Buildding Type")]
    [SerializeField] protected EBuildType buildType = EBuildType.NONE;

    public ESelectableType SelectableType => ESelectableType.BUILDING;
    public EBuildType BuildType => buildType;

    public BuildData BuildData => buildData;
    public float CurrentDurability => currentDurability;

    #endregion ATTRIBUTS

    #region METHODE
    #region MONO

    // Start is called before the first frame update
    void Start()
    {
        InitBuild();
    }

    #endregion MONO
    #region ABSTRACT
    abstract protected void BuildDestroyed();


    #endregion ABSTRACT
    #region INTERFACE
    public void Select()
    {
        selectionObject.SetActive(true);
    }

    public void Deselect()
    {
        selectionObject.SetActive(false);
    }
    #endregion

    public virtual void InitBuild()
    {
        currentDurability = buildData.MaxDurability;
        currentArmor = buildData.Armor;

        buildType = buildData.BuildType;
    }

    public void TakeDamage(AmmoData hitData)
    {
        switch (hitData.AmmoType)
        {
            case (EAmmoType.PHYSIQUE):
                DurabilityUpdate(hitData.Damage);

                if (currentDurability <= 0)
                {
                    BuildDestroyed();
                }
                break;

            default:
                break;
        }
    }

    protected void DurabilityUpdate(float damage)
    {
        currentDurability -= Mathf.Clamp(damage, 0, buildData.MaxDurability);
    }



    #endregion METHODE


}
