using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

public class BlurredPictureBox : PictureBox
{
    private float _blurIntensity = 0.8f;
    private int _blurWidth = 100;
    private int _blurSteps = 20;

    public float BlurIntensity
    {
        get => _blurIntensity;
        set
        {
            _blurIntensity = Math.Max(0f, Math.Min(1f, value));
            Invalidate();
        }
    }

    public int BlurWidth
    {
        get => _blurWidth;
        set
        {
            _blurWidth = Math.Max(0, value);
            Invalidate();
        }
    }

    public int BlurSteps
    {
        get => _blurSteps;
        set
        {
            _blurSteps = Math.Max(1, value);
            Invalidate();
        }
    }

    public BlurredPictureBox()
    {
        // Enable double buffering
        SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw, true);

        // Set background to black for better image display
        BackColor = Color.Black;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (Image == null)
        {
            base.OnPaint(e);
            return;
        }

        var rect = new Rectangle(0, 0, Width, Height);

        // Draw the original image with proper scaling
        e.Graphics.DrawImage(Image, rect);

        // Apply progressive blur effect from left edge
        ApplyProgressiveBlur(e.Graphics, rect);
    }

    private void ApplyProgressiveBlur(Graphics g, Rectangle rect)
    {
        // Create a more sophisticated blur effect using multiple layers
        for (int i = 0; i < _blurSteps; i++)
        {
            float progress = (float)i / (_blurSteps - 1);
            int currentWidth = (int)(_blurWidth * progress);

            if (currentWidth <= 0) continue;

            // Calculate alpha based on position and intensity
            int alpha = (int)(255 * _blurIntensity * (1.0f - progress));
            alpha = Math.Max(0, Math.Min(255, alpha));

            // Create gradient brush for this layer
            using var brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(currentWidth, 0),
                Color.FromArgb(alpha, Color.Black),
                Color.Transparent);

            // Create rectangle for this blur layer
            var blurRect = new Rectangle(0, 0, currentWidth, rect.Height);

            // Apply the blur layer
            g.FillRectangle(brush, blurRect);
        }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        Invalidate();
    }
}
