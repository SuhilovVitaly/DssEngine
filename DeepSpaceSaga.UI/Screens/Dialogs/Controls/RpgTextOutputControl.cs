using System.ComponentModel;
using System.Text;
using DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;

namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

public partial class RpgTextOutputControl : UserControl
{
    // Event that fires when text output is completed
    public event Action? TextOutputCompleted;
    
    // Event that fires when user presses space after text is completed
    public event Action? SpacePressedAfterCompletion;

    private string fullText = string.Empty;
    private bool isOutputting = false;
    private string[] words = Array.Empty<string>();
    private int currentWordIndex = 0;
    private System.Windows.Forms.Timer outputTimer;

    [Category("RPG Text Output")]
    [Description("Speed of text output in milliseconds between words")]
    public int TextOutputSpeedMs { get; set; } = 100;

    [Category("RPG Text Output")]
    [Description("Full text to display")]
    public new string Text
    {
        get => fullText;
        set
        {
            fullText = value;
            if (!isOutputting)
            {
                StartRpgTextOutput();
            }
        }
    }

    public RpgTextOutputControl()
    {
        InitializeComponent();
        this.Resize += RpgTextOutputControl_Resize;
        
        // Enable transparency support
        //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        //SetStyle(ControlStyles.Opaque, false);
        //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        //SetStyle(ControlStyles.UserPaint, true);
        //SetStyle(ControlStyles.DoubleBuffer, true);
        //SetStyle(ControlStyles.ResizeRedraw, true);
        
        // Set transparent background
        BackColor = Color.Transparent;
        
        // Initialize timer for smooth text output
        outputTimer = new System.Windows.Forms.Timer();
        outputTimer.Tick += OutputTimer_Tick;
        
        // Enable key handling for space key
        this.TabStop = true;
        this.KeyDown += RpgTextOutputControl_KeyDown;
    }

    private void RpgTextOutputControl_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
        {
            if (isOutputting)
            {
                // User pressed space while text is still outputting - skip to end
                SkipTextOutput();
                e.Handled = true;
            }
            else
            {
                // User pressed space after text is completed - simulate first button click
                SpacePressedAfterCompletion?.Invoke();
                e.Handled = true;
            }
        }
    }

    private void RpgTextOutputControl_Resize(object? sender, EventArgs e)
    {
        if (lblText != null)
        {
            lblText.Size = this.Size;
        }
    }

    private void OutputTimer_Tick(object? sender, EventArgs e)
    {
        if (currentWordIndex < words.Length)
        {
            // Add next word
            if (currentWordIndex > 0)
            {
                lblText.Text += " " + words[currentWordIndex];
            }
            else
            {
                lblText.Text = words[currentWordIndex];
            }
            
            System.Diagnostics.Debug.WriteLine($"RpgTextOutputControl: Added word {currentWordIndex + 1}/{words.Length}: '{words[currentWordIndex]}'");
            
            currentWordIndex++;
            
            // Check if we're done
            if (currentWordIndex >= words.Length)
            {
                outputTimer.Stop();
                isOutputting = false;
                System.Diagnostics.Debug.WriteLine("RpgTextOutputControl: Text output completed");
                TextOutputCompleted?.Invoke();
            }
        }
    }

    private void StartRpgTextOutput()
    {
        if (string.IsNullOrEmpty(fullText) || isOutputting)
        {
            System.Diagnostics.Debug.WriteLine($"RpgTextOutputControl: Not starting output - fullText: '{fullText}', isOutputting: {isOutputting}");
            return;
        }

        isOutputting = true;
        currentWordIndex = 0;
        lblText.Text = "";
        
        // Split text into words
        words = fullText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        System.Diagnostics.Debug.WriteLine($"RpgTextOutputControl: Starting output with {words.Length} words, interval: {TextOutputSpeedMs}ms");
        
        // Configure and start timer
        outputTimer.Interval = TextOutputSpeedMs;
        outputTimer.Start();
    }

    public void Clear()
    {
        fullText = string.Empty;
        words = Array.Empty<string>();
        currentWordIndex = 0;
        lblText.Text = string.Empty;
        isOutputting = false;
        outputTimer.Stop();
    }

    public void SetText(string text)
    {
        fullText = text;
        if (!isOutputting)
        {
            StartRpgTextOutput();
        }
    }

    /// <summary>
    /// Skips the text output and shows all text immediately
    /// </summary>
    public void SkipTextOutput()
    {
        if (isOutputting)
        {
            // Stop the timer
            outputTimer.Stop();
            isOutputting = false;
            
            // Show all text immediately
            lblText.Text = fullText;
            
            // Fire completion event
            TextOutputCompleted?.Invoke();
        }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // Do not paint background to maintain transparency
    }
}
