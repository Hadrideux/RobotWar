using UnityEditor.VersionControl;
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

    [Header("Order Data")]
    [SerializeReference] protected OrderData currentOrder = null;
    [SerializeReference] protected AUnitState currentState = null;

    [Header("Ammo")]
    [SerializeField] protected ShellController shellController = null;
    [SerializeField] protected float reloading = 0f;

    [Header("Component")]
    [SerializeField] protected EffectComponent effectComponent = null;
    [SerializeField] protected ScanningComponent scanningComponent = null;

    [Header("Unit Debug")]
    [SerializeField] protected bool isFreezeUnit = false;


    #endregion ATTRIBUTS

    #region PROPERTIES
    public UnitData UnitData => unitData;
    public ShellController ShellController => shellController;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public EffectComponent EffectComponent => effectComponent;
    public ScanningComponent ScanningComponent => scanningComponent;

    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0, unitData.MaxHealth);
    }

    public ESelectableType SelectableType => ESelectableType.UNIT;
    public EFactionType FactionObject
    {
        get => unitFaction;
        set => unitFaction = value;
    }
    public GameObject TargetObject 
    { 
        get => gameObject;
    }

    public OrderData Order
    {
        get => currentOrder;
        set => currentOrder = value;
    }

    #endregion PROPERTIES

    #region METHODE
    #region MONO
    void Start()
    {
        InitUnit();
        ChangeState(new IdleState());
    }

    void Update()
    {
        if (reloading > 0)
        {
            reloading -= Time.deltaTime;
        }

        if (currentState != null)
        {
            currentState.Update(this);
        }
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
                ChangeState(new MovingState());
                break;

            case EOrderType.ATTACK:
                ChangeState(new AttackState());
                break;

            case EOrderType.AUTONOMOUS:
                break;

            case EOrderType.STOP:
                ChangeState(new IdleState());
                break;

            default:
                break;
        }
    }
    #endregion
    #region ABSTRACT METHODE
    //Gestion dégat reçu par l'unité
    abstract public void TakeDamage(AmmoData hitData);
    #endregion ABSTRACT METHODE

    public void ChangeState(AUnitState newState)
    {
        if(currentState != null)
        {
            currentState.Exit(this);
        }

        currentState = newState;
        currentState.Enter(this);
    }

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
        UnitManager.Instance.UnitAvailable(this);
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
            Debug.Log("Targeted");
        }
    }

    public void TargetUnit()
    {                                 
        if(currentOrder.OrderTarget != null)
        {
            TurnTurret(currentOrder.OrderTarget.TargetObject);
        }
        else
        {
            foreach (ITargetableObject unitTargeted in scanningComponent.TargetedObject)
            {
                float closestTarget = Vector3.Distance(unitTargeted.TargetObject.transform.position, transform.position);

                if (closestTarget <= UnitData.AttackRange)
                {
                    TurnTurret(unitTargeted.TargetObject);
                }
            }
        }
    }
    public void Fire(GameObject target)
    { 
        if (reloading <= 0)
        {
            ShellController shell = Instantiate(shellController, spawnShellPoint.position, Quaternion.identity);
            shell.SetDirection(target.transform.position);

            reloading = ShellController.AmmoData.ReloadTime;

            Debug.Log("Attacking");
        }
    }
    #endregion METHODE
}