namespace DeepSpaceSaga.UI.CommonControls;

public partial class ControlWindow : UserControl
{
    private bool isResizing = false;
    private bool isDragging = false;
    private Point lastMousePos;

    private string _title = "Window";
    private bool _isResizible = true;
    private bool _isDraggible = true;

    [Category("Window Properties")]
    [Description("The title displayed in the window's title bar")]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            crlWindowTitle.Text = value;
        }
    }

    [Category("Window Properties")]
    public bool IsResizible
    {
        get => _isResizible;
        set
        {
            _isResizible = value;
        }
    }

    [Category("Window Properties")]
    public bool IsDraggible
    {
        get => _isDraggible;
        set
        {
            _isDraggible = value;
        }
    }

    public ControlWindow()
    {
        InitializeComponent();

        // Set initial title
        crlWindowTitle.Text = _title;

        // Add mouse events to the control itself
        this.MouseDown += ControlWindow_MouseDown;
        this.MouseMove += ControlWindow_MouseMove;
        this.MouseUp += ControlWindow_MouseUp;


        // Add mouse events to pictureBox1
        crlTitleBar.MouseDown += TitleBar_MouseDown;
        crlTitleBar.MouseMove += TitleBar_MouseMove;
        crlTitleBar.MouseUp += TitleBar_MouseUp;

        crlWindowTitle.MouseDown += TitleBar_MouseDown;
        crlWindowTitle.MouseMove += TitleBar_MouseMove;
        crlWindowTitle.MouseUp += TitleBar_MouseUp;

        // Add new focus and click events
        this.GotFocus += ControlWindow_BringToFront;
        this.Click += ControlWindow_BringToFront;

        //crlCloseButton.Location = new Point(Width - 8, 8);

        // Add event handlers for all child controls
        foreach (Control control in this.Controls)
        {
            control.MouseDown += ControlWindow_BringToFront;
            control.Click += ControlWindow_BringToFront;
            control.GotFocus += ControlWindow_BringToFront;
        }
    }

    private void ControlWindow_MouseDown(object? sender, MouseEventArgs e)
    {
        if (IsInResizeZone(e.Location))
        {
            isResizing = true;
            lastMousePos = e.Location;
        }
    }

    private void ControlWindow_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_isDraggible == false) return;

        if (IsInResizeZone(e.Location))
        {
            this.Cursor = Cursors.SizeNWSE;
        }
        else
        {
            this.Cursor = Cursors.Default;
        }

        if (isResizing)
        {
            int deltaX = e.X - lastMousePos.X;
            int deltaY = e.Y - lastMousePos.Y;

            this.Width += deltaX;
            this.Height += deltaY;

            lastMousePos = e.Location;
        }
    }

    private void ControlWindow_MouseUp(object? sender, MouseEventArgs e)
    {
        isResizing = false;
    }

    private bool IsInResizeZone(Point mousePos)
    {
        if (_isResizible)
        {
            // Define resize zone (bottom-right corner, 10x10 pixels)
            Rectangle resizeZone = new Rectangle(
                this.Width - 10,
                this.Height - 10,
                10, 10);

            return resizeZone.Contains(mousePos);
        }

        return false;
    }

    private void TitleBar_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            isDragging = true;
            lastMousePos = e.Location;
        }
    }

    private void TitleBar_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_isDraggible == false) return;

        if (isDragging)
        {
            Point currentScreenPos = this.PointToScreen(e.Location);
            Point originalScreenPos = this.PointToScreen(lastMousePos);

            int deltaX = currentScreenPos.X - originalScreenPos.X;
            int deltaY = currentScreenPos.Y - originalScreenPos.Y;

            Location = new Point(Location.X + deltaX, Location.Y + deltaY);
        }
    }

    private void TitleBar_MouseUp(object? sender, MouseEventArgs e)
    {
        isDragging = false;
    }

    private void Event_CloseWindow(object? sender, EventArgs e)
    {
        this.Visible = false;
    }

    private void ControlWindow_BringToFront(object? sender, EventArgs e)
    {
        this.BringToFront();
    }
}
