using System.Reflection;

// based on https://github.com/rsmeowry/CopperSharp
[ModuleAuthor("opentk")]
[ModuleInfo("ExampleModule",
    Description = "A cool example module",
    CompilePath = "C:\\Users\\yeemi\\curseforge\\minecraft\\Instances\\essentials latest\\saves\\Build World\\datapacks")
]
public class MCDatapack : Module
{
    [MCEvent(MCEventType.WorldLoad)]
    public void load(WorldContext ctx)
    {
        ctx.Announce(Component.Text("load function was called!").Color(TextColor.GREEN));
    }

    [MCEvent(MCEventType.WorldTick)]
    public void tick(WorldContext ctx)
    {
        ctx.Announce(Component.Text("tick function was called!").Color(TextColor.GREEN));
    }

    [MCFunction]
    public void test(WorldContext ctx)
    {
        ctx.Announce(Component.Text("Hello world!").Color(TextColor.RED));
    }

    [MCFunction]
    public void day(WorldContext ctx)
    {
        ctx.Announce(Component.Text("Time has been set to day!").Color(TextColor.GREEN));
        ctx.Raw("time set day");
    }
}