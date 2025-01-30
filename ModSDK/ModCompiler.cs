namespace Datapack
{
    using System.IO;
    using System.Reflection;
    using System;
    using System.Linq;
    using System.Text;

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
            datapackPath = modInfo.CompilePath != null ? Path.Combine(modInfo.CompilePath, modInfo.FunctionName) : datapackPath;
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

            if (no == EventType.WorldLoad)
            {
                sb.AppendLine("scoreboard objectives remove datapackTimers");
                sb.AppendLine("scoreboard objectives add datapackTimers dummy");
                if (Stopwatch.show) sb.AppendLine("scoreboard objectives setdisplay sidebar datapackTimers");
            }
            else if (no == EventType.WorldTick)
            {
                foreach (var stopwatch in Stopwatch.stopwatches)
                    sb.AppendLine($"scoreboard players add {stopwatch.Value.Target} datapackTimers 1");
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
}