using System.Reflection;
using DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler.CustomCommands;

namespace DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler;

public static class AssignmentDialogExitCommand
{
    public static void Execute(ISessionContextService sessionContext, ICommand command)
    {
        var dialogCommand = command as DialogExitCommand;

        if (dialogCommand == null) return;

        // Get the class name from DialogCommand field, fallback to default
        var dialogCommandClassName = dialogCommand.DialogCommand?.Name ?? "AddingExperienceDialogCommand";

        try
        {
            ICustomDialogCommand? instance = null;

            // First, try to find the class in current assemblies
            var type = Type.GetType($"DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler.CustomCommands.{dialogCommandClassName}");
            
            if (type == null)
            {
                // Try to find the type in all loaded assemblies
                type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => 
                    {
                        try
                        {
                            return assembly.GetTypes();
                        }
                        catch (ReflectionTypeLoadException)
                        {
                            // Skip assemblies that can't be loaded (e.g., SkiaSharp version conflicts)
                            return Array.Empty<Type>();
                        }
                        catch (Exception)
                        {
                            // Skip any other assembly loading issues
                            return Array.Empty<Type>();
                        }
                    })
                    .FirstOrDefault(t => t.Name == dialogCommandClassName && typeof(ICustomDialogCommand).IsAssignableFrom(t));
            }

            if (type != null && typeof(ICustomDialogCommand).IsAssignableFrom(type))
            {
                // Create instance from loaded assembly
                instance = Activator.CreateInstance(type) as ICustomDialogCommand;
            }
            else
            {
                // Try to load from external file
                instance = DynamicClassLoader.LoadFromExternalFile(dialogCommandClassName);
            }

            // Execute the command if instance was created
            instance?.Execute(sessionContext, command);
        }
        catch (Exception)
        {
            // Log error or handle as needed
            // For now, we'll silently continue
        }
    }
}
