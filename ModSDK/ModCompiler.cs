using System.IO;
using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.Design;

// temp here
public class Stopwatch
{
    public static Dictionary<string, Stopwatch> stopwatches = new Dictionary<string, Stopwatch>();
    public static bool show = false;
    public static void Show(bool show = true) => Stopwatch.show = show;

    public string Target;
    public uint ticks;

    private Random random = new Random();

    /// <summary>
    /// expected to be ran on world load
    /// </summary>
    private Stopwatch(string name, uint ticks)
    {
        //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //Target = "timer_" + new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        Target = name;
        this.ticks = ticks;
    }

    public static void Create(string name, uint ticks) => stopwatches.Add(name, new Stopwatch($"timer_{name}", ticks));
    public static Stopwatch Get(string name) => stopwatches[name];

    /// <summary>
    /// expected to be ran on world tick
    /// </summary>
    public void IfPass(GameFunctionEvent ctx, Action<GameFunctionEvent> gameEvent)
    {
        // theres probably a better way 2 do this via commands
        GameFunctionEvent actionEvent = new GameFunctionEvent();
        gameEvent(actionEvent);
        foreach (string command in actionEvent.GetOutput().Trim().Split('\n'))
            ctx.Raw($"execute if score {Target} datapackTimers matches {ticks} run {command}");
        ctx.Raw($"execute if score {Target} datapackTimers matches {ticks} run scoreboard players set {Target} datapackTimers 0");
    }
}

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
        Directory.CreateDirectory(Path.Combine(datapackPath, "data", "minecraft", "tags", "function"));
    }

    private static void ProcessModule(Type moduleType)
    {
        var functions = moduleType.GetMethods()
            .Where(m => m.GetCustomAttributes<EventAttribute>().Any() || m.GetCustomAttributes<FunctionAttribute>().Any())
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
            if (function.GetCustomAttributes<EventAttribute>().Any())
            {
                var eventAttribute = function.GetCustomAttribute<EventAttribute>();
                CreateEventFunctionFile(eventAttribute._event, function.Name, function);
                Console.WriteLine("[+] Initialized MCEvent " + function.Name + " of event type " + eventAttribute._event.ToString());
            }
            else if (function.GetCustomAttributes<FunctionAttribute>().Any())
            {
                var functionAttribute = function.GetCustomAttribute<FunctionAttribute>();
                CreateFunctionFile(function.Name, function);
                Console.WriteLine("[+] Initialized MCFunction " + function.Name);
            }
        }

        Console.WriteLine("[+] Initialized module " + modpackName);
    }
   
    private static void CreateEventFunctionFile(EventType eventType, string functionName, MethodInfo method)
    {
        string eventFolder = eventType == EventType.WorldLoad ? "load" : "tick";
        string filePath = Path.Combine(datapackPath, "data", "minecraft", "tags", "function", $"{eventFolder}.json");

        string jsonContent = $"{{ \"values\": [\"{modpackName}:{functionName}\"] }}";
        File.WriteAllText(filePath, jsonContent);

        CreateFunctionFile(functionName, method, eventType);
    }

    private static void CreateFunctionFile(string functionName, MethodInfo method, EventType? no = null)
    {
        string filePath = Path.Combine(datapackPath, "data", modpackName, "function", $"{functionName}.mcfunction");

        GameFunctionEvent ctx = new GameFunctionEvent();
        ExecuteMethod(method, ctx);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(ctx.GetOutput());

        // lazy asf
        switch (no)
        {
            case EventType.WorldLoad:
                sb.AppendLine("scoreboard objectives remove datapackTimers");
                sb.AppendLine("scoreboard objectives add datapackTimers dummy");
                if (Stopwatch.show)
                    sb.AppendLine("scoreboard objectives setdisplay sidebar datapackTimers");
                break;
            case EventType.WorldTick:
                //Console.WriteLine($"[~] Adding {Stopwatch.stopwatches.Count} stopwatch timers");
                foreach (var stopwatch in Stopwatch.stopwatches)
                    sb.AppendLine($"scoreboard players add {stopwatch.Value.Target} datapackTimers 1");
                break;
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    private static void ExecuteMethod(MethodInfo method, GameFunctionEvent ctx)
    {
        var parameters = method.GetParameters();
        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(GameFunctionEvent))
        {
            method.Invoke(Activator.CreateInstance(method.DeclaringType), new object[] { ctx });
        }
    }
}