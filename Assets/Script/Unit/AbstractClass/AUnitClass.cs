using UnityEngine;
using UnityEngine.AI;

public abstract class AUnitClass : MonoBehaviour, ISelectable, IOrderReceiver, ITargetableObject
{
    #region ATTRIBUTS
    [Header("Body")]
    [SerializeField] protected NavMeshAgent navMeshAgent = null;

    [SerializeField] protected GameObject turretBody = null;
    [SerializeField] protected Transform spawnShellPoint = null;

    [SerializeField] protected GameObject selectionObject = null;

    [Header("Statistique")]
    [SerializeField] protected UnitData unitData = null;

    [SerializeField] protected float currentHealth = 0;
    [SerializeField] protected int currentArmor = 0;
    [SerializeField] protected float currentSpeed = 0;

    [SerializeField] protected EFactionType unitFaction = EFactionType.NONE;

    [Header("Ammo")]
    [SerializeField] protected ShellController shellController = null;
    [SerializeField] protected float reloading = 0f;

    [Header("Component")]
    [SerializeField] protected EffectComponent effectComponent = null;

    [Header("Unit Debug")]
    [SerializeField] protected bool isFreezeUnit = false;
    [SerializeField] protected bool isPeacfully = false;

    #endregion ATTRIBUTS

    #region PROPERTIES
    public UnitData UnitData => unitData;
    public ShellController ShellController => shellController;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public EffectComponent EffectComponent => effectComponent;

    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0, unitData.MaxHealth);
    }

    public float Reloading
    {
        get => reloading;
        set => reloading = value;
    }

    public ESelectableType SelectableType => ESelectableType.UNIT;
    public EFactionType ObjectFaction
    {
        get => unitFaction;
        set => unitFaction = value;
    }
    
    public bool IsPeacfully => isPeacfully;


    #endregion PROPERTIES

    #region METHODE
    #region MONO
    void Start()
    {
        InitUnit();
    }

    void OnDestroy()
    {
        UnitManager.Instance.UnitDestroyed(this);
        NetworkManager.Instance.CurrentLoad -= UnitData.NetworkCost;
    }
    #endregion MONO
    #region INTERFACE
    public void Select()
    {
        selectionObject.SetActive(true);
    }

    public void Deselect()
    {
        selectionObject.SetActive(false);
    }

    public void ReceiveOrder(OrderData order)
    {
        switch (order.OrderType)
        {
            case EOrderType.MOVETO:
                MovementUnit(order.OrderDestination);
                break;

            case EOrderType.ATTACK:
                if (order.OrderTarget as AUnitClass)
                {
                    AUnitClass target = order.OrderTarget as AUnitClass;
                    MovementUnit(target.transform.position);
                }
                else if (order.OrderTarget as ABuildClass)
                {
                    ABuildClass target = order.OrderTarget as ABuildClass;
                    MovementUnit(target.transform.position);
                }
                break;

            case EOrderType.STOP:
                navMeshAgent.isStopped = true;
                break;
            default:
                break;
        }
    }
    #endregion
    #region ABSTRACT METHODE

    //Gestion déplacement unité
    abstract public void MovementUnit(Vector3 destination);

    //Gestion dégat reçu par l'unité
    abstract public void TakeDamage(AmmoData hitData);
    #endregion ABSTRACT METHODE
    public void HealthUpdate(float damage)
    {
        currentHealth -= Mathf.Clamp(damage, 0, unitData.MaxHealth);
    }

    public void InitUnit()
    {
        navMeshAgent.speed = unitData.MaxSpeed;
        currentHealth = unitData.MaxHealth;
        currentArmor = unitData.Armor;

        NetworkManager.Instance.CurrentLoad += UnitData.NetworkCost;
        //UnitManager.Instance.ActiveUnits.Add(this);
    }
    public void UnitDestroyed()
    {
        Destroy(gameObject);
    }

    public void TurnTurret(GameObject target)
    {
        Vector3 dirTarget = target.transform.position;

        Vector3 facingTarget = turretBody.transform.forward;
        Vector3 toTarget = (dirTarget - turretBody.transform.position).normalized;

        float dot = Vector3.Dot(facingTarget, toTarget);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        turretBody.transform.rotation = Quaternion.RotateTowards(turretBody.transform.rotation, Quaternion.LookRotation(toTarget), 90 * Time.deltaTime);

        if (angle <= 0.25)
        {
            Fire(target);
        }
    }
    public void Fire(GameObject target)
    {
        //Modifier pour que se soit un décompte 
        if (Reloading < 0)
        {
            ShellController shell = Instantiate(shellController, spawnShellPoint.position, Quaternion.identity);
            shell.SetDirection(target.transform.position);
            

            Reloading = ShellController.AmmoData.ReloadTime;
        }
        else
        {
            Reloading -= Time.deltaTime;
        }
    }
    #endregion METHODE


}