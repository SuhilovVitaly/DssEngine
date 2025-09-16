using System.Reflection;
using System.Runtime.Loader;
using DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler.CustomCommands;

namespace DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler;

/// <summary>
/// Helper class for loading custom dialog command classes from external files
/// </summary>
public static class DynamicClassLoader
{
    private static readonly string ExternalCommandsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods", "General", "DialogCommands");

    /// <summary>
    /// Loads a custom dialog command class from external DLL file
    /// </summary>
    /// <param name="className">Name of the class to load</param>
    /// <returns>Instance of ICustomDialogCommand or null if not found</returns>
    public static ICustomDialogCommand? LoadFromExternalFile(string className)
    {
        try
        {
            // Try to find DLL file in ExternalCommands directory
            var dllPath = Path.Combine(ExternalCommandsPath, $"{className}.command");
            
            if (!File.Exists(dllPath))
            {
                return null;
            }

            // Create a new AssemblyLoadContext for this assembly
            var loadContext = new CustomAssemblyLoadContext();
            
            try
            {
                // Load the assembly
                var assembly = loadContext.LoadFromAssemblyPath(dllPath);
                
                // Find the class that implements ICustomDialogCommand
                var type = assembly.GetTypes()
                    .FirstOrDefault(t => t.Name == className && typeof(ICustomDialogCommand).IsAssignableFrom(t));
                
                if (type == null)
                {
                    return null;
                }
                
                // Create instance of the class
                return Activator.CreateInstance(type) as ICustomDialogCommand;
            }
            finally
            {
                // Unload the context to free memory
                loadContext.Unload();
            }
        }
        catch (Exception ex)
        {
            // Return null if loading fails
            return null;
        }
    }

    /// <summary>
    /// Checks if external file exists for the given class name
    /// </summary>
    /// <param name="className">Name of the class to check</param>
    /// <returns>True if external file exists</returns>
    public static bool ExternalFileExists(string className)
    {
        var dllPath = Path.Combine(ExternalCommandsPath, $"{className}.dll");
        return File.Exists(dllPath);
    }
}

/// <summary>
/// Custom AssemblyLoadContext for loading external assemblies
/// </summary>
public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public CustomAssemblyLoadContext() : base(isCollectible: true)
    {
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // Use default loading behavior
        return null;
    }
}
