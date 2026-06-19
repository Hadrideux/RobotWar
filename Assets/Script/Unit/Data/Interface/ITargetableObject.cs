using UnityEngine;

public interface ITargetableObject
{
     public EFactionType ObjectFaction { get; set; }
}

public enum EFactionType
{
    NONE,
    ALLY,
    IA,
}