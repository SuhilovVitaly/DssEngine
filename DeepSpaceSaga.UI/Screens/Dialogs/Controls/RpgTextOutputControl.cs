using System.ComponentModel;

namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

public partial class RpgTextOutputControl : UserControl
{
    private string fullText = string.Empty;
    private bool isOutputting = false;

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
    }

    private void RpgTextOutputControl_Resize(object sender, EventArgs e)
    {
        if (lblText != null)
        {
            lblText.Size = this.Size;
        }
    }

    private async void StartRpgTextOutput()
    {
        if (string.IsNullOrEmpty(fullText) || isOutputting)
            return;

        isOutputting = true;
        lblText.Text = "";
        
        try
        {
            var words = fullText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var word in words)
            {
                lblText.Text += word + " ";
                await Task.Delay(TextOutputSpeedMs);
            }
        }
        catch (Exception ex)
        {
            // Log error if needed
            lblText.Text = fullText; // Fallback to immediate display
        }
        finally
        {
            isOutputting = false;
        }
    }

    public void SetText(string text)
    {
        fullText = text;
        if (!isOutputting)
        {
            StartRpgTextOutput();
        }
    }

    public void Clear()
    {
        fullText = string.Empty;
        lblText.Text = string.Empty;
        isOutputting = false;
    }
}
