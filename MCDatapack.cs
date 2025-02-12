﻿using System;
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
    CompilePath = "C:\\Users\\yeemi\\curseforge\\minecraft\\Instances\\essentials 1.21.4\\saves\\New World\\datapacks")]
public class MCDatapack : Module
{
    public static void MoveMsg(GameFunctionEvent ctx, string area)
        => ctx.Message(Operator.Self, Component.Text($"You have been moved to the {area} area", TextColor.GREEN));

    [Event(EventType.WorldLoad)]
    public void load(GameFunctionEvent ctx)
    {
        ctx.Message(Operator.All, Component.Text("Refreshing datapack", TextColor.GREEN));

        // reset the objectives the datapack uses
        ctx.Raw("scoreboard objectives remove right_click_test");
        ctx.Raw("scoreboard objectives add right_click_test minecraft.used:minecraft.carrot_on_a_stick");

        ctx.Raw("scoreboard objectives remove brokeLogs");
        ctx.Raw("scoreboard objectives add brokeLogs minecraft.mined:minecraft.oak_log");
    }

    [Event(EventType.WorldTick)]
    public void tick(GameFunctionEvent ctx)
    {
        // temp 2 see if this works as intended
        // oops just realized this rapidly increases
        ctx.Raw("execute as @a if score @s brokeLogs matches 1.. run execute at @e[type=item,name=\"Oak Log\",tag=!marked] run function buildworldpack:test");
        ctx.Raw("execute as @a if score @s brokeLogs matches 1.. run scoreboard players reset @s brokeLogs");
        ctx.Raw("execute as @e[type=item,name=\"Oak Log\"] run tag @s add marked");//oh shit

        // execute if entity @a[nbt={SelectedItem:{id:"minecraft:carrot_on_a_stick",count:1,components:{"minecraft:custom_data":{test:1}}}}]
        GiveUtil.SetAndExecute(ctx, "Back", new string[] { "function buildworldpack:load_inventory", "clear @s minecraft:carrot_on_a_stick", "function buildworldpack:debug" });
        GiveUtil.SetAndExecute(ctx, "Clear Inventory", new string[] { "clear @s", "function buildworldpack:debug" });
        GiveUtil.SetAndExecute(ctx, "Repair Map", new string[] { "function buildworldpack:repair_map" });
        GiveUtil.SetAndExecute(ctx, "Admin Menu", new string[] { "function buildworldpack:give_menu_items" });
    }

    [Function]
    public void test(GameFunctionEvent ctx)
    {
        for (int x = -2; x < 4; x++)
            for (int z = -2; z < 4; z++)
                ctx.Raw($"execute if block ~{x} ~ ~{z} oak_leaves run fill ~{x} ~ ~{z} ~{x} ~1 ~{z} air destroy");
    }

    [Function]
    public void repair_map(GameFunctionEvent ctx)
    {
        ctx.Raw("function buildworldpack:repair_oak_tree"); // vein miner thingy

        ctx.Message(Operator.All, Component.Text("Map repaired", TextColor.GREEN));
    }

    [Function]
    public void repair_oak_tree(GameFunctionEvent ctx)
    {
        ctx.Raw("fill 9 56 -5 13 61 -9 air");
        ctx.Raw("fill 11 56 -7 11 60 -7 oak_log");
        ctx.Raw("fill 13 58 -5 9 59 -9 oak_leaves replace air");
        ctx.Raw("fill 10 60 -8 12 61 -6 oak_leaves replace air");
    }

    [Function]
    public void version(GameFunctionEvent ctx)
    {
        ctx.Message(Operator.Self, Component.Text($"Built at {DateTime.Now:dd/MM/yyyy-h:mm}", TextColor.DARK_PURPLE));
    }

    [Function]
    public void give_menu_items(GameFunctionEvent ctx)
    {
        ctx.Raw("function buildworldpack:save_inventory");
        GiveUtil.GiveItem(ctx, "Back", "minecraft:structure_void");
        GiveUtil.GiveItem(ctx, "Clear Inventory", "minecraft:barrier");
        GiveUtil.GiveItem(ctx, "Repair Map", "minecraft:painting");
    }

    [Function]
    public void debug(GameFunctionEvent ctx)
    {
        GiveUtil.GiveItem(ctx, "Admin Menu", "minecraft:nether_star");
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