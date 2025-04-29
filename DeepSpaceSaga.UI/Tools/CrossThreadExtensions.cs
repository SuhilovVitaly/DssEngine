using System.Diagnostics;

namespace DeepSpaceSaga.UI.Tools;

public static class CrossThreadExtensions
{
    public static void PerformSafely(this Control target, Action action)
    {
        if (target.InvokeRequired)
        {
            try
            {
                target.Invoke(action);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error {e.Message}");
            }
        }
        else
        {
            action();
        }
    }

    public static void PerformSafely<T1>(this Control target, Action<T1> action, T1 parameter)
    {
        try
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, parameter);
            }
            else
            {
                action(parameter);
            }
        }
        catch(Exception e) 
        {
            Debug.WriteLine($"Error {e.Message}");
        }
    }

    public static void PerformSafely<T1, T2>(this Control target, Action<T1, T2> action, T1 p1, T2 p2)
    {
        try
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, p1, p2);
            }
            else
            {
                action(p1, p2);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error {e.Message}");
        }
    }
}
