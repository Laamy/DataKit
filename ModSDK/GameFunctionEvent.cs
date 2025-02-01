namespace Datapack;

using System.Text;

using Datapack.Components;
using Datapack.Operators;

/// <summary>
/// the IR (Intermediate Representation) for the game mcfunctions
/// </summary>
public class GameFunctionEvent
{
    private StringBuilder _output = new StringBuilder();
    
    public GameFunctionEvent()
    {
    }

    internal void Message(Operator target, Component value)
        => _output.AppendLine($"tellraw {target.ToRaw()} {value.ToJSON()}");

    internal void Time(Component value)
        => _output.AppendLine(value.ToRaw());
    internal void Time(Time value)
        => _output.AppendLine(Component.Time(value).ToRaw());

    internal void Weather(Component value)
        => _output.AppendLine(value.ToRaw());
    internal void Weather(Weather value)
        => _output.AppendLine(Component.Weather(value).ToRaw());

    // TODO: a basic class/enum for targets..
    internal void Effect(Operator target, Component value, bool give = true)
        => _output.AppendLine($"effect {(give == true ? "give" : "clear")} " +
            $"{target.ToRaw()} {(give == true ? value.ToRaw() : ((EffectComponent)value).GetEffect().ToString().ToLower())}");
    internal void Effect(Operator target, bool give = true, Effect effect = Components.Effect.Speed, uint dur = 30, byte amp = 0, bool hide = false)
    {
        EffectComponent effectComponent = Component.Effect(effect, dur, amp, hide);
        _output.AppendLine($"effect {(give == true ? "give" : "clear")} " +
            $"{target.ToRaw()} {(give == true ? effectComponent.ToRaw() : effectComponent.GetEffect().ToString().ToLower())}");
    }

    internal void Teleport(string args)
        => _output.AppendLine($"tp {args}"); // not gonna overengineer this yet..

    internal void Raw(string command)
        => _output.AppendLine(command);

    internal string GetOutput() => _output.ToString();
}