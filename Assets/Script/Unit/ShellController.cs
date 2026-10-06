using UnityEngine;

public class ShellController : MonoBehaviour
{
    [SerializeField] private AmmoData ammoData = null;
    [SerializeField] private int shellLifeTime = 0;

    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private Vector3 shellDir = Vector3.zero;

    [SerializeField] private EFactionType shellFaction = EFactionType.NONE;

    #region PROPERTIES
    public AmmoData AmmoData
    {
        get => ammoData;
        set => ammoData = value;
    }
    public EFactionType ShellFaction
    {
        get => shellFaction;
        set => shellFaction = value;
    }
    #endregion PROPERTIES

    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, shellLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void OnTriggerEnter(Collider other)
    {
        ITargetableObject target = other.GetComponentInParent<ITargetableObject>();

        if (target != null)
        {
            if (target.FactionObject == shellFaction)
            {
                return;
            }

            if (target as AUnitClass)
            {
                AUnitClass unit = target as AUnitClass;

                unit.TakeDamage(ammoData);
                Destroy(gameObject);
            }
            else if (target as ABuildClass)
            {
                ABuildClass build = target as ABuildClass;

                build.TakeDamage(ammoData);
                Destroy(gameObject);
            }
        }
    }

    #endregion MONO

    #region METHODE
    private void Moving()
    {
        rb.linearVelocity = shellDir * ammoData.Speed;
    }

    public void SetDirection(Vector3 direction)
    {
        shellDir = (direction - transform.position).normalized;
        transform.LookAt(direction);
    }

    #endregion METHODE
}
//