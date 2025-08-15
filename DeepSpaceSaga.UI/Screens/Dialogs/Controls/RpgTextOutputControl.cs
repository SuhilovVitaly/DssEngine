using System.ComponentModel;
using System.Text;
using DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;

namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

public partial class RpgTextOutputControl : UserControl
{
    // Event that fires when text output is completed
    public event Action? TextOutputCompleted;

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
    public string Text
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
        
        // Enable double buffering to reduce flickering
        SetStyle(ControlStyles.DoubleBuffer | 
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.UserPaint | 
                ControlStyles.ResizeRedraw, true);
        
        // Initialize timer for smooth text output
        outputTimer = new System.Windows.Forms.Timer();
        outputTimer.Tick += OutputTimer_Tick;
        
        // Enable key handling for space key
        this.TabStop = true;
        this.KeyDown += RpgTextOutputControl_KeyDown;
    }

    private void RpgTextOutputControl_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space && isOutputting)
        {
            // User pressed space - skip text output
            SkipTextOutput();
            e.Handled = true;
        }
    }

    private void RpgTextOutputControl_Resize(object sender, EventArgs e)
    {
        if (lblText != null)
        {
            lblText.Size = this.Size;
        }
    }

    private void OutputTimer_Tick(object sender, EventArgs e)
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
            
            currentWordIndex++;
            
            // Check if we're done
            if (currentWordIndex >= words.Length)
            {
                outputTimer.Stop();
                isOutputting = false;
                TextOutputCompleted?.Invoke();
            }
        }
    }

    private void StartRpgTextOutput()
    {
        if (string.IsNullOrEmpty(fullText) || isOutputting)
            return;

        isOutputting = true;
        currentWordIndex = 0;
        lblText.Text = "";
        
        // Split text into words
        words = fullText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
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
}
