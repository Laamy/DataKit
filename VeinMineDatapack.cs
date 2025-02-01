using System;
using System.Linq;
using System.Reflection;

using Datapack;
using Datapack.Components;
using Datapack.Operators;

[ModuleAuthor("opentk")]
[ModuleInfo("BuildWorldPack",
    Description = "A pack im using in my build world",
    CompilePath = "C:\\Users\\yeemi\\curseforge\\minecraft\\Instances\\essentials 1.21.4\\saves\\New World\\datapacks")]
public class VeinMinerDatapack : Module
{
    [Event(EventType.WorldLoad)]
    public void load(GameFunctionEvent ctx)
    {
        ctx.Message(Operator.All, Component.Text("Refreshing vein miner datapack", TextColor.DARK_AQUA));

        ctx.Raw("scoreboard objectives remove brokeLogs");
        ctx.Raw("scoreboard objectives add brokeLogs minecraft.mined:minecraft.oak_log");
    }

    [Event(EventType.WorldTick)]
    public void tick(GameFunctionEvent ctx)
    {
        // temp 2 see if this works as intended
        // oops just realized this rapidly increases
        // dogshitttt
        ctx.Raw("execute as @a if score @s brokeLogs matches 1.. run execute at @e[type=item,name=\"Oak Log\",tag=!marked] run function buildworldpack:test");
        ctx.Raw("execute as @a if score @s brokeLogs matches 1.. run scoreboard players reset @s brokeLogs");
        ctx.Raw("execute as @e[type=item,name=\"Oak Log\"] run tag @s add marked");//oh shit
    }

    [Function]
    public void test(GameFunctionEvent ctx)
    {
        for (int x = -2; x < 4; x++)
            for (int z = -2; z < 4; z++)
                ctx.Raw($"execute if block ~{x} ~ ~{z} oak_leaves run fill ~{x} ~ ~{z} ~{x} ~1 ~{z} air destroy");
    }
}