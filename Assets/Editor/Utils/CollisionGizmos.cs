// CollisionGizmos.cs
#if UNITY_EDITOR
using UnityEngine;

public static class CollisionGizmos
{
    // Collider générique (auto-détecte le type)
    public static void Draw(Collider col, Color color, bool filled = false)
    {
        if (col == null) return;
        Gizmos.color = color;

        switch (col)
        {
            case SphereCollider sphere:
                DrawSphere(sphere, filled);
                break;
            default:
                Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
                break;
        }
    }

    public static void DrawSphere(SphereCollider sphere, bool filled = false)
    {
        var worldCenter = sphere.transform.TransformPoint(sphere.center);
        var worldRadius = sphere.radius * Mathf.Max(
            sphere.transform.lossyScale.x,
            sphere.transform.lossyScale.y,
            sphere.transform.lossyScale.z
        );

        if (filled) Gizmos.DrawSphere(worldCenter, worldRadius);
        else Gizmos.DrawWireSphere(worldCenter, worldRadius);
    }

    // Bonus : affiche un label de debug au-dessus
    public static void DrawLabel(Vector3 position, string text, Color color)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.Label(position + Vector3.up * 0.2f, text);
#endif
    }
}
#endif