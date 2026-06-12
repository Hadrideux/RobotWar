using UnityEngine;
using UnityEngine.AI;

public abstract class AUnitClass : MonoBehaviour
{
    #region ATTRIBUTS
    [Header("Body")]
    [SerializeField] protected NavMeshAgent navMeshAgent = null;

    [SerializeField] protected GameObject turretBody = null;
    [SerializeField] protected Transform spawnShellPoint = null;

    [Header("Statistique")]
    [SerializeField] protected UnitData unitData = null;

    [SerializeField] protected float currentHealth = 0;
    [SerializeField] protected int currentArmor = 0;
    [SerializeField] protected float currentSpeed = 0;

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

    public ShellController ShellController => shellController;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public EffectComponent EffectComponent => effectComponent;

    public bool IsFreezeUnit => isFreezeUnit;
    public bool IsPeacfully => isPeacfully;


    #endregion PROPERTIES

    #region MONO
    void Start()
    {
        InitUnit();
    }

    void OnDestroy()
    {
        UnitManager.Instance.UnitDestroyed(this);
        NetworkManager.Instance.CurrentLoad -= UnitData.NetworkCost;

        Debug.Log($"Unit destroy : {this}");

        UnitManager.Instance.ActiveUnits.Remove(this);
    }
    #endregion MONO

    #region METHODE
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
        UnitManager.Instance.ActiveUnits.Add(this);        
    }
    public void UnitDestroyed()
    {
        Destroy(gameObject);
    }

    public void TurnTurret(AUnitClass target)
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
    public void Fire(AUnitClass unit)
    {
        //Modifier pour que se soit un décompte 
        if (Reloading < 0)
        {
            ShellController shell = Instantiate(shellController, spawnShellPoint.position, Quaternion.identity);
            shell.SetDirection(unit.transform.position);

            Reloading = ShellController.AmmoData.ReloadTime;             
        }
        else
        {                             
            Reloading -= Time.deltaTime;
        }
    }

    

    #endregion METHODE

    #region ABSTRACT METHODE

    //Gestion déplacement unité
    abstract public void MovementUnit();

    //Gestion dégat reçu par l'unité
    abstract public void TakeDamage(AmmoData hitData);

    #endregion ABSTRACT METHODE
}