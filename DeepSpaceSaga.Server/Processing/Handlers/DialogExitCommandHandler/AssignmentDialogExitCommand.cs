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
        var dialogCommandClassName = dialogCommand.DialogCommand ?? "AddingExperienceDialogCommand";

        try
        {
            // Get the type by name from the current assembly
            var type = Type.GetType($"DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler.CustomCommands.{dialogCommandClassName}");
            
            if (type == null)
            {
                // Try to find the type in all loaded assemblies
                type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(t => t.Name == dialogCommandClassName && typeof(ICustomDialogCommand).IsAssignableFrom(t));
            }

            if (type != null && typeof(ICustomDialogCommand).IsAssignableFrom(type))
            {
                // Create instance of the class
                var instance = Activator.CreateInstance(type) as ICustomDialogCommand;
                
                if (instance != null)
                {
                    // Execute the command
                    instance.Execute(sessionContext, command);
                }
            }
        }
        catch (Exception)
        {
            // Log error or handle as needed
            // For now, we'll silently continue
        }
    }
}
