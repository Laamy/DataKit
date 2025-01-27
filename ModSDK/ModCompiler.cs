using System.IO;
using System.Reflection;
using System;
using System.Linq;

public class ModCompiler
{
    private static string datapackPath = "CSharp_Datapack";
    private static string modpackName = "modcompiler.modpackname";
    private static string modpackDesc = "modcompiler.modpackdesc";

    public static void Main(string[] args)
    {
        ProcessModule(typeof(MCDatapack));
        Console.WriteLine("[+] finished building datapack");
    }

    private static void CreateDatapackDirectory()
    {
        if (Directory.Exists(datapackPath)) Directory.Delete(datapackPath, true);
        Directory.CreateDirectory(datapackPath);
        Directory.CreateDirectory(Path.Combine(datapackPath, "data", "minecraft", "tags", "functions"));
    }

    private static void ProcessModule(Type moduleType)
    {
        var functions = moduleType.GetMethods()
            .Where(m => m.GetCustomAttributes<MCEventAttribute>().Any() || m.GetCustomAttributes<MCFunctionAttribute>().Any())
            .ToList();

        var modInfo = moduleType.GetCustomAttribute<ModuleInfoAttribute>();
        if (modInfo.CompilePath != null)
            datapackPath = Path.Combine(modInfo.CompilePath, modInfo.FunctionName);
        else datapackPath = modInfo.FunctionName;

        modpackName = modInfo.FunctionName.ToLower();
        modpackDesc = modInfo.Description;

        Console.WriteLine("[~] Starting setup of datapack");
        CreateDatapackDirectory();

        Console.WriteLine("[~] Initializing module " + modpackName);

        Directory.CreateDirectory(Path.Combine(datapackPath, "data", modpackName, "function"));
        File.WriteAllText(Path.Combine(datapackPath, "pack.mcmeta"), "{\"pack\":{\"pack_format\":48,\"name\":\"" + modpackName + "\",\"description\":\"" + modpackDesc + "\"}}");

        foreach (var function in functions)
        {
            if (function.GetCustomAttributes<MCEventAttribute>().Any())
            {
                var eventAttribute = function.GetCustomAttribute<MCEventAttribute>();
                CreateEventFunctionFile(eventAttribute._event, function.Name, function);
                Console.WriteLine("[+] Initialized MCEvent " + function.Name + " of event type " + eventAttribute._event.ToString());
            }
            else if (function.GetCustomAttributes<MCFunctionAttribute>().Any())
            {
                var functionAttribute = function.GetCustomAttribute<MCFunctionAttribute>();
                CreateFunctionFile(function.Name, function);
                Console.WriteLine("[+] Initialized MCFunction " + function.Name);
            }
        }

        Console.WriteLine("[+] Initialized module " + modpackName);
    }

    private static void CreateEventFunctionFile(MCEventType eventType, string functionName, MethodInfo method)
    {
        string eventFolder = eventType == MCEventType.WorldLoad ? "load" : "tick";
        string filePath = Path.Combine(datapackPath, "data", "minecraft", "tags", "functions", $"{eventFolder}.json");

        string jsonContent = $"{{ \"values\": [\"{modpackName}:{functionName}\"] }}";
        File.WriteAllText(filePath, jsonContent);

        CreateFunctionFile(functionName, method);
    }

    private static void CreateFunctionFile(string functionName, MethodInfo method)
    {
        string filePath = Path.Combine(datapackPath, "data", modpackName, "function", $"{functionName}.mcfunction");

        WorldContext ctx = new WorldContext();
        ExecuteMethod(method, ctx);

        File.WriteAllText(filePath, ctx.GetOutput());
    }

    private static void ExecuteMethod(MethodInfo method, WorldContext ctx)
    {
        var parameters = method.GetParameters();
        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(WorldContext))
        {
            method.Invoke(Activator.CreateInstance(method.DeclaringType), new object[] { ctx });
        }
    }
}