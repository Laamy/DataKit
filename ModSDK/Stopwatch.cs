namespace Datapack;

using System.Collections.Generic;
using System;

public class Stopwatch
{
    public static Dictionary<string, Stopwatch> stopwatches = new Dictionary<string, Stopwatch>();
    public static bool show = false;

    public string Target;
    public uint ticks;

    private Stopwatch(string name, uint ticks)
    {
        //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //Target = "timer_" + new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        Target = name;
        this.ticks = ticks;
    }

    public static void Create(string name, uint ticks) => stopwatches.Add(name, new Stopwatch($"timer_{name}", ticks));
    public static void Show(bool show = true) => Stopwatch.show = show;
    public static Stopwatch Get(string name) => stopwatches[name];

    public void IfPass(GameFunctionEvent ctx, Action<GameFunctionEvent> gameEvent)
    {
        // TODO: uninline this to its own mcfunction file
        GameFunctionEvent actionEvent = new GameFunctionEvent();
        gameEvent(actionEvent);
        foreach (string command in actionEvent.GetOutput().Trim().Split('\n'))
            ctx.Raw($"execute if score {Target} datapackTimers matches {ticks} run {command}");
        ctx.Raw($"execute if score {Target} datapackTimers matches {ticks} run scoreboard players set {Target} datapackTimers 0");
    }
}