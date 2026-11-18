using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class RadialProgress : VisualElement
{
    private float progress = 0f;

    [UxmlAttribute]
    public float Progress
    {
        get => progress;
        set
        {
            progress = Mathf.Clamp01(value);
            MarkDirtyRepaint(); // demande à UI Toolkit de redessiner
        }
    }

    [UxmlAttribute] public float LineWidth { get; set; } = 8f;
    [UxmlAttribute] public Color TrackColor { get; set; } = new Color(1f, 1f, 1f, 0.15f);
    [UxmlAttribute] public Color FillColor { get; set; } = Color.cyan;

    public RadialProgress()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    private void OnGenerateVisualContent(MeshGenerationContext ctx)
    {
        float radius = Mathf.Min(contentRect.width, contentRect.height) * 0.5f - LineWidth * 0.5f;
        if (radius <= 0f) return;

        Vector2 center = contentRect.center;
        Painter2D painter = ctx.painter2D;
        painter.lineWidth = LineWidth;
        painter.lineCap = LineCap.Butt;

        // Le cercle de fond
        painter.strokeColor = TrackColor;
        painter.BeginPath();
        painter.Arc(center, radius, Angle.Degrees(0f), Angle.Degrees(360f));
        painter.Stroke();

        if (progress <= 0f) return;

        // L'arc de progression, qui part du haut (-90°) et tourne dans le sens horaire
        painter.strokeColor = FillColor;
        painter.BeginPath();
        painter.Arc(center, radius, Angle.Degrees(-90f), Angle.Degrees(-90f + 360f * progress));
        painter.Stroke();
    }
}