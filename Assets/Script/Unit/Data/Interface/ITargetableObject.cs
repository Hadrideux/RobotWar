using UnityEngine;

public interface ITargetableObject
{
    public EFactionType FactionObject { get; set; }
    public GameObject TargetObject {  get;}
}

public enum EFactionType
{
    NONE,
    ALLY,
    IA,
}