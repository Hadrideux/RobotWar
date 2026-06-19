using UnityEngine;

public interface ITargetablObject
{
     public EFactionType ObjectFaction { get; set; }
}

public enum EFactionType
{
    NONE,
    ALLY,
    IA,
}