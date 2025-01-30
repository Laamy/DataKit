using System;
using System.Linq;
using System.Reflection;

using Datapack;
using Datapack.Components;

class GiveUtil
{
    public static Random random = new Random();

    public static void SetAndExecute(GameFunctionEvent ctx, string custom_name, string[] commands)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string tempTag = "temp_" + new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        string itemNbtData = "nbt={SelectedItem:{components:{\"minecraft:custom_name\":'{\"extra\":[{\"italic\":false,\"text\":\"" + custom_name + "\"}],\"text\":\"\"}'}}}";
        ctx.Raw($"execute as @a[{itemNbtData},scores={{right_click_test=1}}] run tag @s add " + tempTag);
        foreach (string str in commands)
            ctx.Raw($"execute as @a[tag=" + tempTag + "] run " + str);
        ctx.Raw($"scoreboard players set @a[tag=" + tempTag + "] right_click_test 0");
        ctx.Raw($"execute as @a[tag=" + tempTag + "] run tag @s remove " + tempTag);
    }

    public static void GiveItem(GameFunctionEvent ctx, string custom_name, string model = "minecraft:carrot_on_a_stick")
    {
        void giveStick(string name) => ctx.Raw($"give @a minecraft:carrot_on_a_stick[item_model=\"{model}\",custom_name='[\"\",{{\"text\":\"{name}\",\"italic\":false,custom_data:{{test:1}}}}]',enchantment_glint_override=true] 1");
        giveStick(custom_name);
    }
}

// based on https://github.com/rsmeowry/CopperSharp
[ModuleAuthor("opentk")]
[ModuleInfo("BuildWorldPack",
    Description = "A pack im using in my build world",
    CompilePath = "C:\\Users\\yeemi\\curseforge\\minecraft\\Instances\\essentials 1.21.4\\saves\\Build World\\datapacks")]
public class MCDatapack : Module
{
    public static void MoveMsg(GameFunctionEvent ctx, string area)
        => ctx.Caller.Message(Component.Text($"You have been moved to the {area} area", TextColor.GREEN));

    //[FakeEvent(FakeEventType.RightClick)]

    [Event(EventType.WorldLoad)]
    public void load(GameFunctionEvent ctx)
    {
        ctx.Message(Component.Text("Refreshing datapack", TextColor.GREEN));

        ctx.Raw("scoreboard objectives remove right_click_test");
        ctx.Raw("scoreboard objectives add right_click_test minecraft.used:minecraft.carrot_on_a_stick");
        
        Stopwatch.Show();
        // TODO: inline option (default rn)
        // also TODO: brand new function per lambda. for example 1minutetimer/events/timer_1minutetimer.mcfunction then call it once it passes
        // Stopwatch.Create("1minutetimer", 20 * 60, CompileType.Inline);
        Stopwatch.Create("saturation", 20*5);
        Stopwatch.Create("clearLag", 20*300);
    }

    [Event(EventType.WorldTick)]
    public void tick(GameFunctionEvent ctx)
    {
        Stopwatch.Get("saturation").IfPass(ctx, e => e.Effect("@a", Component.Effect(Effect.Saturation, 10 * 20, 255, true)));

        // TODO: move the tick stuff on the Stopwatch.Create to this IfPass so i can do smth on lets say 20*250 ticks then 20*300
        Stopwatch.Get("clearLag").IfPass(ctx, e => {
            e.Raw("kill @e[type=item]");
            e.Message(Component.Text("[ClearLag] Cleared dropped items", TextColor.GREEN));
        });

        // execute if entity @a[nbt={SelectedItem:{id:"minecraft:carrot_on_a_stick",count:1,components:{"minecraft:custom_data":{test:1}}}}]
        GiveUtil.SetAndExecute(ctx, "Clear Inventory", new string[] {
            "clear @s",
            "function buildworldpack:debug"
        });
        GiveUtil.SetAndExecute(ctx, "Plains", new string[] { "function buildworldpack:plains" });
        GiveUtil.SetAndExecute(ctx, "Snowy", new string[] { "function buildworldpack:snow" });
        GiveUtil.SetAndExecute(ctx, "Dark Oak", new string[] { "function buildworldpack:dark_oak" });
    }

    [Function]
    public void version(GameFunctionEvent ctx)
    {
        ctx.Caller.Message(Component.Text($"Built at {DateTime.Now:dd/MM/yyyy-h:mm}", TextColor.DARK_PURPLE));
    }

    [Function]
    public void debug(GameFunctionEvent ctx)
    {
        GiveUtil.GiveItem(ctx, "Clear Inventory", "minecraft:barrier");
        GiveUtil.GiveItem(ctx, "Snowy", "minecraft:snow_block");
        GiveUtil.GiveItem(ctx, "Dark Oak", "minecraft:dark_oak_log");
        GiveUtil.GiveItem(ctx, "Plains", "minecraft:grass_block");
    }

    [Function]
    public void dark_oak(GameFunctionEvent ctx)
    {
        MoveMsg(ctx, "dark oak");
        ctx.Caller.Effect(true, Effect.Blindness, 5, 1, true);
        ctx.Weather(Weather.Clear);
        ctx.Caller.Teleport("-955.08 64.00 8979.23");
    }

    [Function]
    public void plains(GameFunctionEvent ctx)
    {
        MoveMsg(ctx, "plains");
        ctx.Caller.Effect(true, Effect.Blindness, 5, 1, true);
        ctx.Weather(Weather.Clear);
        ctx.Caller.Teleport("5775.48 69.00 -5716.71");
    }

    [Function]
    public void snow(GameFunctionEvent ctx)
    {
        MoveMsg(ctx, "snow");
        ctx.Caller.Effect(true, Effect.Blindness, 2, 1, true);
        ctx.Weather(Weather.Thunder);
        ctx.Caller.Teleport("-5572.51 70.00 11383.36");
    }
}