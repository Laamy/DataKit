using System;
using System.Reflection;

public class McUtils
{
    public static void MoveMsg(GameFunctionEvent ctx, string area)
        => ctx.Caller.Message(Component.Text($"You have been moved to the {area} area", TextColor.GREEN));
}

// based on https://github.com/rsmeowry/CopperSharp
[ModuleAuthor("opentk")]
[ModuleInfo("BuildWorldPack",
    Description = "A pack im using in my build world",
    CompilePath = "C:\\Users\\yeemi\\curseforge\\minecraft\\Instances\\essentials latest\\saves\\Build World\\datapacks")
]
public class MCDatapack : Module
{
    [Event(EventType.WorldLoad)]
    public void load(GameFunctionEvent ctx)
    {
        ctx.Message(Component.Text("Refreshing datapack", TextColor.GREEN));

        Stopwatch.Show();
        // TODO: inline option (default rn)
        // also TODO: brand new function per lambda. for example 1minutetimer/events/timer_1minutetimer.mcfunction then call it once it passes
        Stopwatch.Create("1minutetimer", 20 * 60);
    }

    [Event(EventType.WorldTick)]
    public void tick(GameFunctionEvent ctx)
    {
        Stopwatch.Get("1minutetimer")
            .IfPass(ctx, e => {
            e.Message(Component.Text("[DATAPACK] 1 minute has passed", TextColor.YELLOW));
            e.Message(Component.Text("[DATAPACK] another message", TextColor.YELLOW));
        });
    }

    [Function]
    public void version(GameFunctionEvent ctx)
    {
        TimeSpan builtWhen = DateTime.Now.TimeOfDay;
        ctx.Caller.Message(Component.Text($"Built at {DateTime.Now:dd/MM/yyyy-h:mm}", TextColor.DARK_PURPLE));
    }

    [Function]
    public void dark_oak(GameFunctionEvent ctx)
    {
        McUtils.MoveMsg(ctx, "dark oak");

        ctx.Caller.Effect(true, Effect.Blindness, 5, 1, true);
        ctx.Weather(Weather.Clear);
        ctx.Caller.Teleport("-955.08 64.00 8979.23");
    }

    [Function]
    public void plains(GameFunctionEvent ctx)
    {
        McUtils.MoveMsg(ctx, "plains");

        ctx.Caller.Effect(true, Effect.Blindness, 5, 1, true);
        ctx.Weather(Weather.Clear);
        ctx.Caller.Teleport("5775.48 69.00 -5716.71");
    }

    [Function]
    public void snow(GameFunctionEvent ctx)
    {
        McUtils.MoveMsg(ctx, "snow");

        ctx.Caller.Effect(true, Effect.Blindness, 2, 1, true);
        ctx.Weather(Weather.Thunder);
        ctx.Caller.Teleport("-5572.51 70.00 11383.36");
    }
}