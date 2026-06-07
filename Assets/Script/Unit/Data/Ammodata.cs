using UnityEngine;

[CreateAssetMenu(fileName = "UnitAmmo", menuName = "Scriptable Objects/UnitAmmo")]
public class AmmoData : ScriptableObject
{
    [SerializeField] private ShellController shellPrefab = null;
    [SerializeField] private EAmmoType ammoType = EAmmoType.NONE;

    [SerializeField] private int damage = 0;
    [SerializeField] private float speed = 0f;

    [SerializeField] private float reloadTime = 0f;

    public ShellController ShellPrefab
    {
        get => shellPrefab;
        set => shellPrefab = value;

    }

    public EAmmoType AmmoType
    {
        get => ammoType;
        set => ammoType = value;
    }

    public float ReloadTime
    {
        get => reloadTime;
        set => reloadTime = value;
    }

    public int Damage
    {
        get => damage;
        set => damage = value; 
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    
}
