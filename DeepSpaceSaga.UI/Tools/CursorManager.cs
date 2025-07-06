namespace DeepSpaceSaga.UI.Tools;

public static class CursorManager
{
    private static Cursor _defaultCursor;
    private static Cursor _selectedCursor;

    public static void Initialize()
    {
        LoadDefaultCursor();
        LoadSelectedCursor();
        
        // Set default cursor for the entire application
        if (_defaultCursor != null)
        {
            Application.UseWaitCursor = false;
            Cursor.Current = _defaultCursor;
        }
    }

    public static Cursor DefaultCursor => _defaultCursor ?? Cursors.Default;
    public static Cursor SelectedCursor => _selectedCursor ?? _defaultCursor ?? Cursors.Hand;

    private static void LoadDefaultCursor()
    {
        try
        {
            var cursorPath = Path.Combine(Application.StartupPath, "Images", "cursor.png");
            if (File.Exists(cursorPath))
            {
                using var originalBitmap = new Bitmap(cursorPath);
                using var resizedBitmap = new Bitmap(26, 26);
                using var graphics = Graphics.FromImage(resizedBitmap);
                
                // Use high quality resizing
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                
                graphics.DrawImage(originalBitmap, 0, 0, 26, 26);
                
                var hIcon = resizedBitmap.GetHicon();
                _defaultCursor = new Cursor(hIcon);
            }
        }
        catch
        {
            _defaultCursor = Cursors.Default;
        }
    }

    private static void LoadSelectedCursor()
    {
        try
        {
            var cursorPath = Path.Combine(Application.StartupPath, "Images", "cursor-selected.png");
            if (File.Exists(cursorPath))
            {
                using var originalBitmap = new Bitmap(cursorPath);
                using var resizedBitmap = new Bitmap(26, 26);
                using var graphics = Graphics.FromImage(resizedBitmap);
                
                // Use high quality resizing
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                
                graphics.DrawImage(originalBitmap, 0, 0, 26, 26);
                
                var hIcon = resizedBitmap.GetHicon();
                _selectedCursor = new Cursor(hIcon);
            }
        }
        catch
        {
            _selectedCursor = _defaultCursor;
        }
    }

    public static void SetDefaultCursorForControl(Control control)
    {
        if (_defaultCursor != null)
        {
            control.Cursor = _defaultCursor;
            
            // Apply to all child controls recursively
            foreach (Control child in control.Controls)
            {
                SetDefaultCursorForControl(child);
            }
        }
    }
} 