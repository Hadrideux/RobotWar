using UnityEngine;

public class ShellController : MonoBehaviour
{
    [SerializeField] private AmmoData ammoData = null;

    [SerializeField] private Rigidbody rb = null;

    [SerializeField] private Vector3 shellDir = Vector3.zero;

    public AmmoData AmmoData
    {
        get => ammoData;
        set => ammoData = value;
    }

    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void OnTriggerEnter(Collider other)
    {
        AUnitClass unit = other.GetComponentInParent<AUnitClass>();

        if (unit != null)
        {
            unit.TakeDamage(ammoData);
            Destroy(gameObject);
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
    }

    #endregion METHODE
}
//