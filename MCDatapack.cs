using System.Reflection;

public class McUtils
{
    public static void MoveMsg(GameFunctionEvent ctx, string area)
        => ctx.Caller.Message(Component.Text($"You have been moved to the {area} area").Color(TextColor.GREEN));
}

// based on https://github.com/rsmeowry/CopperSharp
[ModuleAuthor("opentk")]
[ModuleInfo("BuildWorldPack",
    Description = "A pack im using in my build world",
    CompilePath = "C:\\Users\\yeemi\\curseforge\\minecraft\\Instances\\essentials latest\\saves\\Build World\\datapacks")
]
public class MCDatapack : Module
{
    // TODO: fix this, no clue why this wont work
    [Event(EventType.WorldLoad)]
    public void load(GameFunctionEvent ctx)
    {
        ctx.Message(Component.Text("load function was called!").Color(TextColor.GREEN));
    }

    [Function]
    public void dark_oak(GameFunctionEvent ctx)
    {
        McUtils.MoveMsg(ctx, "dark oak");

        ctx.Raw("effect give @s minecraft:darkness 5 1 true");
        ctx.Raw("effect give @s minecraft:darkness 1 1 true");
        ctx.Weather(Component.Weather(Weather.Clear));
        ctx.Caller.Teleport("-955.08 64.00 8979.23");
    }

    [Function]
    public void plains(GameFunctionEvent ctx)
    {
        McUtils.MoveMsg(ctx, "plains");

        ctx.Raw("effect give @s minecraft:darkness 5 1 true");
        ctx.Raw("effect give @s minecraft:darkness 1 1 true");
        ctx.Weather(Component.Weather(Weather.Clear));
        ctx.Caller.Teleport("5775.48 69.00 -5716.71");
    }

    [Function]
    public void snow(GameFunctionEvent ctx)
    {
        McUtils.MoveMsg(ctx, "snow");

        ctx.Raw("effect give @s minecraft:blindness 2 1 true");
        ctx.Weather(Component.Weather(Weather.Thunder));
        ctx.Caller.Teleport("-5572.51 70.00 11383.36");
    }
}