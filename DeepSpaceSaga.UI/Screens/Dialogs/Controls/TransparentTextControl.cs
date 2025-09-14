namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

public class TransparentTextControl : UserControl
{
    private string _text = string.Empty;
    private Font _font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
    private Color _foreColor = Color.WhiteSmoke;
    private ContentAlignment _textAlign = ContentAlignment.TopLeft;

    public override string Text
    {
        get => _text;
        set
        {
            _text = value ?? string.Empty;
            Invalidate();
        }
    }

    public override Font Font
    {
        get => _font;
        set
        {
            _font = value;
            Invalidate();
        }
    }

    public override Color ForeColor
    {
        get => _foreColor;
        set
        {
            _foreColor = value;
            Invalidate();
        }
    }

    public ContentAlignment TextAlign
    {
        get => _textAlign;
        set
        {
            _textAlign = value;
            Invalidate();
        }
    }

    public int TextOutputSpeedMs { get; set; } = 50;

    public void Clear()
    {
        _text = string.Empty;
        Invalidate();
    }

    public TransparentTextControl()
    {
        // Enable transparency
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.Opaque, false);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.DoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);

        BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // Do not call base.OnPaint to avoid background drawing
        if (!string.IsNullOrEmpty(_text))
        {
            using var brush = new SolidBrush(_foreColor);
            var rect = new Rectangle(0, 0, Width, Height);
            var format = new StringFormat();

            switch (_textAlign)
            {
                case ContentAlignment.TopLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    break;
            }

            e.Graphics.DrawString(_text, _font, brush, rect, format);
        }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // Do not paint background to maintain transparency
    }
}
