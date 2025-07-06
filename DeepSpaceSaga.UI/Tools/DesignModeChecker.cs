using System.ComponentModel;
using System.Diagnostics;

namespace DeepSpaceSaga.UI.Tools;

/// <summary>
/// Provides reliable design mode detection for Windows Forms controls
/// </summary>
public static class DesignModeChecker
{
    /// <summary>
    /// Determines if the current process is running in design mode
    /// </summary>
    /// <returns>True if running in design mode, false otherwise</returns>
    public static bool IsInDesignMode()
    {
        // Multiple checks for design mode to ensure reliability
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return true;
            
        // Check if running in Visual Studio or other design environment
        var processName = Process.GetCurrentProcess().ProcessName.ToLower();
        return processName.Contains("devenv") || processName.Contains("designtoolsserver");
    }
} 