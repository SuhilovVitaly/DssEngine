using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Engine.UI.Controls
{
    public sealed partial class ProgressBarVerticalHorizontal : UserControl
    {
        public ProgressBarVerticalHorizontal()
        {
            InitializeComponent();
            
            SetStyle(ControlStyles.UserPaint, true);

            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(e);

            var background = new Rectangle(new Point(0, 0), new Size(Size.Width, Size.Height));
            e.Graphics.FillRectangle(new SolidBrush(BackColor), background);

            var currentWidth = (int)(Width * ((double)_currentValue / _maximum));
            // Create a rectangle that represents the size of the control, minus 1 pixel.
            var area = new Rectangle(new Point(0, 0), new Size(currentWidth, Size.Height));

            // Draw an aqua rectangle in the rectangle represented by the control.
            e.Graphics.FillRectangle(new SolidBrush(_barLineColor), area);
        }

        private int _maximum = 100;
        [Category("Flash"),
         Description("The ending color of the bar.")]
        // The public property BarLineColor accesses _barLineColor.  
        public int Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
                //crlFilledSurface.BackColor = _barLineColor;
                // The Invalidate method calls the OnPaint method, which redraws
                // the control.  
                Invalidate();
            }
        }

        private int _currentValue = 100;
        [Category("Flash"),
         Description("The ending color of the bar.")]
        // The public property BarLineColor accesses _barLineColor.  
        public int CurrentValue
        {
            get
            {
                return _currentValue;
            }
            set
            {
                _currentValue = value;
                //var currentWidth = (int)(Width * ((double)_currentValue / _maximum));
                //crlFilledSurface.Width = currentWidth;
                Invalidate();
            }
        }

        private Color _barLineColor = Color.LimeGreen;
        [Category("Flash"),
         Description("The ending color of the bar.")]
        // The public property BarLineColor accesses _barLineColor.  
        public Color BarLineColor
        {
            get
            {
                return _barLineColor;
            }
            set
            {
                _barLineColor = value;
                //crlFilledSurface.BackColor = _barLineColor;
                // The Invalidate method calls the OnPaint method, which redraws
                // the control.  
                Invalidate();
            }
        }
    }
}
