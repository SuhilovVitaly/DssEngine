namespace DeepSpaceSaga.UI.Render.Tools;

public class DrawTools
{
    public static SizeF MeasureString(string label, Font font)
    {
        // Create proper typeface with weight for bold
        var typeface = font.Bold 
            ? SKTypeface.FromFamilyName(font.Name, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
            : SKTypeface.FromFamilyName(font.Name);
        
        using var paint = new SKPaint
        {
            Typeface = typeface,
            TextSize = font.Size,
            IsAntialias = true
        };
        
        SKRect bounds = new();
        paint.MeasureText(label, ref bounds);
        float textHeight = bounds.Height;
        float simpleWidth = paint.MeasureText(label);

        return new SizeF(simpleWidth, textHeight);
    }

    public static void DrawString(IScreenInfo screen, string text, Font font, SpaceMapColor color, RectangleF rectangle)
    {
        using var textPaint = new SKPaint
        {
            Color = color.ToSKColor(),
            TextSize = font.Size,
            IsAntialias = true
        };

        screen.GraphicSurface.DrawText(text, new SKPoint(rectangle.X, rectangle.Y), font.ConvertToSKFont(), textPaint);
    }

    public static void DrawEllipse(IScreenInfo screen, float x, float y, float radius, SpaceMapColor color, float strokeWidth = 1)
    {
        using var gridPaint = new SKPaint
        {
            Color = color.ToSKColor(),
            StrokeWidth = strokeWidth,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke
        };

        screen.GraphicSurface.DrawCircle(x, y, radius, gridPaint);
    }

    public static void FillEllipse(IScreenInfo screen, float x, float y, float radius, SpaceMapColor color)
    {
        using var gridPaint = new SKPaint
        {
            Color = color.ToSKColor(),
            StrokeWidth = 1,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        screen.GraphicSurface.DrawCircle(x, y, radius, gridPaint);
    }

    public static void DrawLine(IScreenInfo screen, SpaceMapColor color, SpaceMapPoint p1, SpaceMapPoint p2)
    {
        using var gridPaint = new SKPaint
        {
            Color = color.ToSKColor(),
            StrokeWidth = 1,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke
        };

        screen.GraphicSurface.DrawLine(p1.ToSkPoint(), p2.ToSkPoint(), gridPaint);
    }

    public static void FillRectangle(IScreenInfo screen, SpaceMapColor color, float x, float y, float width, float height)
    {
        FillRectangle(screen, color, new SpaceMapPoint(x, y), width, height);
    }

    public static void FillRectangle(IScreenInfo screen, SpaceMapColor color, SpaceMapPoint p1, float width, float height)
    {
        using var gridPaint = new SKPaint
        {
            Color = color.ToSKColor(),
            StrokeWidth = 1,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        screen.GraphicSurface.DrawRect(p1.X, p1.Y, width, height, gridPaint);
    }

    public static void DrawRectangle(IScreenInfo screen, SpaceMapColor color, float x, float y, float width, float height)
    {
        DrawRectangle(screen, color, new SpaceMapPoint(x, y), width, height);
    }

    public static void DrawRectangle(IScreenInfo screen, SpaceMapColor color, SpaceMapPoint p1, float width, float height)
    {
        using var gridPaint = new SKPaint
        {
            Color = color.ToSKColor(),
            StrokeWidth = 1,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke
        };

        screen.GraphicSurface.DrawRect(p1.X, p1.Y, width, height, gridPaint);
    }
}
