using System.Reflection;
using System.Runtime.Loader;
using DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler.CustomCommands;
using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Abstractions.Entities;

namespace DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler;

/// <summary>
/// Helper class for loading custom dialog command classes from external files
/// </summary>
public static class DynamicClassLoader
{
    private static readonly string ExternalCommandsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods", "General", "DialogCommands");

    /// <summary>
    /// Loads a custom dialog command class from external file (DLL or C# source)
    /// </summary>
    /// <param name="className">Name of the class to load</param>
    /// <returns>Instance of ICustomDialogCommand or null if not found</returns>
    public static ICustomDialogCommand? LoadFromExternalFile(string className)
    {
        try
        {
            // First try to load from .command file (C# source)
            var commandPath = Path.Combine(ExternalCommandsPath, $"{className}.command");
            if (File.Exists(commandPath))
            {
                return LoadFromSource(commandPath, className);
            }

            // Then try to load from C# source file (.cs.txt)
            var csPath = Path.Combine(ExternalCommandsPath, $"{className}.cs.txt");
            if (File.Exists(csPath))
            {
                return LoadFromSource(csPath, className);
            }

            // Finally try to load from DLL file
            var dllPath = Path.Combine(ExternalCommandsPath, $"{className}.dll");
            if (File.Exists(dllPath))
            {
                return LoadFromDll(dllPath, className);
            }

            return null;
        }
        catch (Exception ex)
        {
            // Log the error for debugging
            Console.WriteLine($"Error loading external file for class '{className}': {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Loads a class from DLL file
    /// </summary>
    private static ICustomDialogCommand? LoadFromDll(string dllPath, string className)
    {
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

    /// <summary>
    /// Loads a class from C# source file by compiling it to a temporary DLL
    /// </summary>
    private static ICustomDialogCommand? LoadFromSource(string sourcePath, string className)
    {
        try
        {
            Console.WriteLine($"Loading source file: {sourcePath}");
            System.Diagnostics.Debug.WriteLine($"Loading source file: {sourcePath}");
            
            // Read the source code
            var sourceCode = File.ReadAllText(sourcePath);
            Console.WriteLine($"Source code length: {sourceCode.Length} characters");
            Console.WriteLine($"Source code content:\n{sourceCode}");
            
            // Create a temporary DLL file path
            var tempDir = Path.Combine(Path.GetTempPath(), "DeepSpaceSaga", "DynamicCommands");
            Directory.CreateDirectory(tempDir);
            var tempDllPath = Path.Combine(tempDir, $"{className}_{Guid.NewGuid():N}.dll");
            
            Console.WriteLine($"Compiling to: {tempDllPath}");
            
            // Use C# compiler to compile the source to DLL
            var compileResult = CompileSourceToDll(sourceCode, tempDllPath);
            
            if (!compileResult)
            {
                Console.WriteLine($"Failed to compile source file {sourcePath}");
                return null;
            }
            
            Console.WriteLine($"Compilation successful, loading assembly...");
            Console.WriteLine($"DLL path: {tempDllPath}");
            Console.WriteLine($"DLL exists: {File.Exists(tempDllPath)}");
            Console.WriteLine($"DLL size: {new FileInfo(tempDllPath).Length} bytes");
            
            // Try different ways to load the assembly
            Assembly? assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(tempDllPath);
                Console.WriteLine($"Assembly loaded with LoadFrom: {assembly.FullName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadFrom failed: {ex.Message}");
                try
                {
                    var assemblyBytes = File.ReadAllBytes(tempDllPath);
                    assembly = Assembly.Load(assemblyBytes);
                    Console.WriteLine($"Assembly loaded with Load: {assembly.FullName}");
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Load also failed: {ex2.Message}");
                    return null;
                }
            }
            
            if (assembly == null)
            {
                Console.WriteLine("Failed to load assembly");
                return null;
            }
            
            // Get all types from the assembly
            Type[] allTypes;
            try
            {
                allTypes = assembly.GetTypes();
                Console.WriteLine($"Found {allTypes.Length} types in assembly:");
                foreach (var t in allTypes)
                {
                    Console.WriteLine($"  - {t.Name} (Namespace: {t.Namespace})");
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"ReflectionTypeLoadException: {ex.Message}");
                allTypes = ex.Types.Where(t => t != null).ToArray()!;
                Console.WriteLine($"Found {allTypes.Length} loadable types:");
                foreach (var t in allTypes)
                {
                    Console.WriteLine($"  - {t.Name} (Namespace: {t.Namespace})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting types: {ex.Message}");
                return null;
            }
            
            // Find the class that implements ICustomDialogCommand
            var type = allTypes
                .FirstOrDefault(t => typeof(ICustomDialogCommand).IsAssignableFrom(t));
            
            if (type == null)
            {
                Console.WriteLine($"No class implementing ICustomDialogCommand found in {className}");
                Console.WriteLine("Available types:");
                foreach (var t in allTypes)
                {
                    Console.WriteLine($"  - {t.Name}: Implements ICustomDialogCommand = {typeof(ICustomDialogCommand).IsAssignableFrom(t)}");
                }
                return null;
            }
            
            Console.WriteLine($"Found implementing type: {type.Name}");
            
            // Create instance of the class
            return Activator.CreateInstance(type) as ICustomDialogCommand;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from source file {sourcePath}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Compiles C# source code to a DLL using dotnet build
    /// </summary>
    private static bool CompileSourceToDll(string sourceCode, string outputPath)
    {
        try
        {
            // Create temporary directory for the project
            var tempDir = Path.GetDirectoryName(outputPath)!;
            var projectName = Path.GetFileNameWithoutExtension(outputPath);
            var tempSourcePath = Path.Combine(tempDir, $"{projectName}.cs");
            var projectPath = Path.Combine(tempDir, $"{projectName}.csproj");
            
            // Write source file
            File.WriteAllText(tempSourcePath, sourceCode);
            
            // Create a simple .csproj file
            var projectContent = $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <OutputType>Library</OutputType>
    <OutputPath>{Path.GetDirectoryName(outputPath)}</OutputPath>
    <AssemblyName>{projectName}</AssemblyName>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""{typeof(ICustomDialogCommand).Assembly.GetName().Name}"">
      <HintPath>{typeof(ICustomDialogCommand).Assembly.Location}</HintPath>
    </Reference>
    <Reference Include=""{typeof(ISessionContextService).Assembly.GetName().Name}"">
      <HintPath>{typeof(ISessionContextService).Assembly.Location}</HintPath>
    </Reference>
    <Reference Include=""{typeof(ICommand).Assembly.GetName().Name}"">
      <HintPath>{typeof(ICommand).Assembly.Location}</HintPath>
    </Reference>
    <Reference Include=""{typeof(CommandStatus).Assembly.GetName().Name}"">
      <HintPath>{typeof(CommandStatus).Assembly.Location}</HintPath>
    </Reference>
    <Reference Include=""{typeof(CommandCategory).Assembly.GetName().Name}"">
      <HintPath>{typeof(CommandCategory).Assembly.Location}</HintPath>
    </Reference>
    <Reference Include=""{typeof(CommandTypes).Assembly.GetName().Name}"">
      <HintPath>{typeof(CommandTypes).Assembly.Location}</HintPath>
    </Reference>
    <Reference Include=""{typeof(GameSession).Assembly.GetName().Name}"">
      <HintPath>{typeof(GameSession).Assembly.Location}</HintPath>
    </Reference>
    <Reference Include=""{typeof(ISessionInfoService).Assembly.GetName().Name}"">
      <HintPath>{typeof(ISessionInfoService).Assembly.Location}</HintPath>
    </Reference>
    <Reference Include=""{typeof(IMetricsService).Assembly.GetName().Name}"">
      <HintPath>{typeof(IMetricsService).Assembly.Location}</HintPath>
    </Reference>
  </ItemGroup>
</Project>";
            
            File.WriteAllText(projectPath, projectContent);
            Console.WriteLine($"Created project file: {projectPath}");
            Console.WriteLine($"Project content:\n{projectContent}");

            // Run dotnet build
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{projectPath}\" --configuration Release --verbosity quiet",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = tempDir
            };

            using var process = System.Diagnostics.Process.Start(startInfo);
            if (process == null)
            {
                Console.WriteLine("Failed to start dotnet build process");
                return false;
            }

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Clean up temporary files
            try
            {
                File.Delete(tempSourcePath);
                File.Delete(projectPath);
                var binDir = Path.Combine(tempDir, "bin");
                var objDir = Path.Combine(tempDir, "obj");
                if (Directory.Exists(binDir)) Directory.Delete(binDir, true);
                if (Directory.Exists(objDir)) Directory.Delete(objDir, true);
            }
            catch { }

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"Dotnet build error (ExitCode: {process.ExitCode}):");
                Console.WriteLine($"Output: {output}");
                Console.WriteLine($"Error: {error}");
                return false;
            }

            // Check if the file exists at the expected path
            if (File.Exists(outputPath))
            {
                return true;
            }

            // Check alternative paths where dotnet might have placed the file
            var alternativePaths = new[]
            {
                Path.Combine(tempDir, "bin", "Release", "net8.0", $"{projectName}.dll"),
                Path.Combine(tempDir, "bin", "Debug", "net8.0", $"{projectName}.dll"),
                Path.Combine(Path.GetDirectoryName(outputPath)!, "net8.0", $"{projectName}.dll"),
                Path.Combine(Path.GetDirectoryName(outputPath)!, "Release", "net8.0", $"{projectName}.dll"),
                Path.Combine(Path.GetDirectoryName(outputPath)!, "Debug", "net8.0", $"{projectName}.dll")
            };

            foreach (var altPath in alternativePaths)
            {
                if (File.Exists(altPath))
                {
                    // Copy the file to the expected location
                    try
                    {
                        File.Copy(altPath, outputPath, true);
                        Console.WriteLine($"Found DLL at alternative path: {altPath}");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error copying file from {altPath} to {outputPath}: {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"DLL not found at expected path: {outputPath}");
            Console.WriteLine("Searched alternative paths:");
            foreach (var altPath in alternativePaths)
            {
                Console.WriteLine($"  - {altPath} (exists: {File.Exists(altPath)})");
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error compiling source: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Finds the C# compiler executable
    /// </summary>
    private static string? FindCSharpCompiler()
    {
        // Try to find csc.exe in common locations
        var possiblePaths = new[]
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft Visual Studio", "2022", "Community", "MSBuild", "Current", "Bin", "Roslyn", "csc.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft Visual Studio", "2022", "Professional", "MSBuild", "Current", "Bin", "Roslyn", "csc.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft Visual Studio", "2022", "Enterprise", "MSBuild", "Current", "Bin", "Roslyn", "csc.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio", "2019", "Community", "MSBuild", "Current", "Bin", "Roslyn", "csc.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio", "2019", "Professional", "MSBuild", "Current", "Bin", "Roslyn", "csc.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio", "2019", "Enterprise", "MSBuild", "Current", "Bin", "Roslyn", "csc.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "sdk", "8.0.0", "Roslyn", "bincore", "csc.dll"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "sdk", "8.0.0", "Roslyn", "bincore", "csc.exe")
        };

        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }

        // Try to find using dotnet command
        try
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "--list-sdks",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using var process = System.Diagnostics.Process.Start(startInfo);
            if (process != null)
            {
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    var lines = output.Split('\n');
                    foreach (var line in lines)
                    {
                        if (line.Contains("8.0"))
                        {
                            var version = line.Split(' ')[0];
                            var sdkPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "sdk", version, "Roslyn", "bincore", "csc.dll");
                            if (File.Exists(sdkPath))
                            {
                                return sdkPath;
                            }
                        }
                    }
                }
            }
        }
        catch { }

        return null;
    }

    /// <summary>
    /// Checks if external file exists for the given class name
    /// </summary>
    /// <param name="className">Name of the class to check</param>
    /// <returns>True if external file exists</returns>
    public static bool ExternalFileExists(string className)
    {
        var commandPath = Path.Combine(ExternalCommandsPath, $"{className}.command");
        var dllPath = Path.Combine(ExternalCommandsPath, $"{className}.dll");
        var csPath = Path.Combine(ExternalCommandsPath, $"{className}.cs.txt");
        return File.Exists(commandPath) || File.Exists(dllPath) || File.Exists(csPath);
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