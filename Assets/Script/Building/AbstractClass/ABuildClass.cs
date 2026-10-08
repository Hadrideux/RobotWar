using UnityEngine;

public abstract class ABuildClass : MonoBehaviour, ISelectable, ITargetableObject
{
    #region ATTRIBUTS
    [Header("Manager")]
    [SerializeField] protected RequisitionManager requisitionManager = null;
    [SerializeField] protected IARequsitionManager IARequsition = null;

    [Header("Component")]
    [SerializeField] protected PlaceableObjectComponent placeableComponent = null;

    [SerializeField] protected GameObject selectionObject = null;

    [Header("Statistique")]
    [SerializeField] protected BuildData buildData = null;

    [SerializeField] protected float currentDurability = 0;
    [SerializeField] protected int currentArmor = 0;

    [Header("Buildding Type")]
    [SerializeField] protected EBuildType buildType = EBuildType.NONE;
    [SerializeField] protected EFactionType buildFaction = EFactionType.NONE;

    #endregion ATTRIBUTS

    #region PROPERTY
    #region INTERFACE 
    public EFactionType ObjectFaction
    {
        get => buildFaction;
        set => buildFaction = value;
    }
    #endregion INTERFACE

    public ESelectableType SelectableType => ESelectableType.BUILDING;
    public EBuildType BuildType => buildType;

    public BuildData BuildData => buildData;
    public float CurrentDurability => currentDurability;

    public EFactionType FactionObject
    {
        get => buildFaction;
        set => buildFaction = value;
    }

    public GameObject TargetObject => gameObject;

    #endregion PROPERTY

    #region METHODE
    #region MONO

    // Start is called before the first frame update
    void Start()
    {
        if (buildFaction == EFactionType.ALLY)
        {
            requisitionManager = RequisitionManager.Instance;
        }
        else if (buildFaction == EFactionType.IA)
        {
            IARequsition = IARequsitionManager.Instance;
        }

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
    public void InitBuild()
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
                
                break;

            default:
                break;
        }

        BuildManager.Instance.BuildignAttacked(this);
    }

    protected void DurabilityUpdate(float damage)
    {
        currentDurability -= Mathf.Clamp(damage, 0, buildData.MaxDurability);

        if (currentDurability <= 0)
        {
            BuildDestroyed();
        }
    }



    #endregion METHODE


}
