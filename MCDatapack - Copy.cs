using System;
using System.Linq;
using System.Reflection;

using Datapack;
using Datapack.Components;
using Datapack.Operators;

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

// https://misode.github.io/predicate/
// https://mcstacker.net/

// based on https://github.com/rsmeowry/CopperSharp
[ModuleAuthor("opentk")]
[ModuleInfo("BuildWorldPack",
    Description = "A pack im using in my build world",
    CompilePath = "C:\\Users\\yeemi\\curseforge\\minecraft\\Instances\\essentials 1.21.4\\saves\\Build World\\datapacks")]
public class MCDatapack : Module
{
    public static void MoveMsg(GameFunctionEvent ctx, string area)
        => ctx.Message(Operator.Self, Component.Text($"You have been moved to the {area} area", TextColor.GREEN));

    //[FakeEvent(FakeEventType.RightClick, "custom_name")]
    //MenuUtil.Events.GiveItem(ctx, "custom_name", "minecraft:carrot_on_a_stick");

    [Event(EventType.WorldLoad)]
    public void load(GameFunctionEvent ctx)
    {
        ctx.Message(Operator.All, Component.Text("Refreshing datapack", TextColor.GREEN));

        ctx.Raw("scoreboard objectives remove right_click_test");
        ctx.Raw("scoreboard objectives add right_click_test minecraft.used:minecraft.carrot_on_a_stick");
        
        // TODO: inline option (default rn)
        // also TODO: brand new function per lambda. for example events/timer_1minutetimer.mcfunction then call it once it passes
        // Stopwatch.Create("1minutetimer", 20 * 60, CompileType.Inline);
        Stopwatch.Create("saturation", 20*5);
        Stopwatch.Create("clearLag", 20*300);
    }

    [Event(EventType.WorldTick)]
    public void tick(GameFunctionEvent ctx)
    {
        Stopwatch.Get("saturation").IfPass(ctx, e => e.Effect(Operator.All, Component.Effect(Effect.Saturation, 10 * 20, 255, true)));

        // TODO: move the tick stuff on the Stopwatch.Create to this IfPass so i can do smth on lets say 20*250 ticks then 20*300
        Stopwatch.Get("clearLag").IfPass(ctx, e => {
            e.Raw("kill @e[type=item]");
            e.Message(Operator.All, Component.Text("[ClearLag] Cleared dropped items", TextColor.GREEN));
        });

        // execute if entity @a[nbt={SelectedItem:{id:"minecraft:carrot_on_a_stick",count:1,components:{"minecraft:custom_data":{test:1}}}}]
        GiveUtil.SetAndExecute(ctx, "Back", new string[] { "function buildworldpack:load_inventory", "clear @s minecraft:carrot_on_a_stick", "function buildworldpack:debug" });
        GiveUtil.SetAndExecute(ctx, "Clear Inventory", new string[] { "clear @s", "function buildworldpack:debug" });
        GiveUtil.SetAndExecute(ctx, "Plains", new string[] { "function buildworldpack:plains" });
        GiveUtil.SetAndExecute(ctx, "Snowy", new string[] { "function buildworldpack:snow" });
        GiveUtil.SetAndExecute(ctx, "Dark Oak", new string[] { "function buildworldpack:dark_oak" });
        GiveUtil.SetAndExecute(ctx, "Admin Menu", new string[] { "function buildworldpack:give_menu_items" });
    }

    [Function]
    public void version(GameFunctionEvent ctx)
    {
        ctx.Message(Operator.Self, Component.Text($"Built at {DateTime.Now:dd/MM/yyyy-h:mm}", TextColor.DARK_PURPLE));
    }

    [Function]
    public void test(GameFunctionEvent ctx)
    {
        ctx.Message(Operator.All.AddDistance(3, 10),
            Component.Text($"hello world u are within 3 to 10 blocks from the player who executed this", TextColor.DARK_PURPLE));
    }

    [Function]
    public void give_menu_items(GameFunctionEvent ctx)
    {
        ctx.Raw("function buildworldpack:save_inventory");
        GiveUtil.GiveItem(ctx, "Back", "minecraft:structure_void");
        GiveUtil.GiveItem(ctx, "Clear Inventory", "minecraft:barrier");
        GiveUtil.GiveItem(ctx, "Snowy", "minecraft:powder_snow_bucket");
        GiveUtil.GiveItem(ctx, "Dark Oak", "minecraft:dark_oak_sapling");
        GiveUtil.GiveItem(ctx, "Plains", "minecraft:short_grass");
    }

    [Function]
    public void debug(GameFunctionEvent ctx)
    {
        GiveUtil.GiveItem(ctx, "Admin Menu", "minecraft:nether_star");
    }

    [Function]
    public void dark_oak(GameFunctionEvent ctx)
    {
        MoveMsg(ctx, "dark oak");
        ctx.Effect(Operator.Self, true, Effect.Blindness, 5, 1, true);
        ctx.Weather(Weather.Clear);
        ctx.Teleport("-955.08 64.00 8979.23");
    }

    [Function]
    public void plains(GameFunctionEvent ctx)
    {
        MoveMsg(ctx, "plains");
        ctx.Effect(Operator.Self, true, Effect.Blindness, 5, 1, true);
        ctx.Weather(Weather.Clear);
        ctx.Teleport("5775.48 69.00 -5716.71");
    }

    [Function]
    public void snow(GameFunctionEvent ctx)
    {
        MoveMsg(ctx, "snow");
        ctx.Effect(Operator.Self, true, Effect.Blindness, 2, 1, true);
        ctx.Weather(Weather.Thunder);
        ctx.Teleport("-5572.51 70.00 11383.36");
    }

    // TODO: make the mc data command into its own expansive "component"
    [Function]
    public void save_inventory(GameFunctionEvent ctx)
    {
        // https://www.youtube.com/watch?v=MhuDMSOY23Y
        ctx.Raw("data remove storage buildworldpack:hotbar inventory");
        ctx.Raw("data modify storage buildworldpack:hotbar inventory.data set from entity @s Inventory");
        ctx.Raw("clear @s");
    }
    [Function]
    public void load_inventory(GameFunctionEvent ctx)
    {
        ctx.Raw("clear @s");
        ctx.Raw("data modify storage buildworldpack:hotbar temporary.inventory set from storage buildworldpack:hotbar inventory");
        ctx.Raw("function buildworldpack:item_replace");
    }
    [Function]
    public void item_replace(GameFunctionEvent ctx)
    {
        ctx.Raw("data remove storage buildworldpack:hotbar temporary.item");
        ctx.Raw("data modify storage buildworldpack:hotbar temporary.item.id set from storage buildworldpack:hotbar temporary.inventory.data[-1].id");
        ctx.Raw("data modify storage buildworldpack:hotbar temporary.item.count set from storage buildworldpack:hotbar temporary.inventory.data[-1].count");
        ctx.Raw("data modify storage buildworldpack:hotbar temporary.item.slot set from storage buildworldpack:hotbar temporary.inventory.data[-1].Slot");

        ctx.Raw("execute store result score #current slot run data get storage buildworldpack:hotbar temporary.item.slot");
        ctx.Raw("execute unless score #current slot matches 0..35 run data remove storage buildworldpack:hotbar temporary.inventory.data[-1]");
        ctx.Raw("execute unless score #current slot matches 0..35 run return run function buildworldpack:item_replace with storage buildworldpack:hotbar temporary.inventory.data[-1]");
        ctx.Raw("function buildworldpack:item_replace_macro with storage buildworldpack:hotbar temporary.item");
        ctx.Raw("function buildworldpack:item_replace with storage buildworldpack:hotbar temporary.inventory.data[-1]");
    }
    [Function]
    public void item_replace_macro(GameFunctionEvent ctx)
    {
        ctx.Raw("$item replace entity @s container.$(slot) with $(id) $(count)");
        ctx.Raw("data remove storage buildworldpack:hotbar temporary.inventory.data[-1]");
    }
}