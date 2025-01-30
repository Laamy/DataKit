using System.Text;

public class GameFunctionEvent
{
    private StringBuilder _output = new StringBuilder();
    public LocalPlayer Caller { get; private set; }

    public class LocalPlayer
    {
        private GameFunctionEvent _ctx;

        public LocalPlayer(GameFunctionEvent ctx) => _ctx = ctx;

        internal void Message(Component value)
            => _ctx._output.AppendLine($"tellraw @s {value.ToJSON()}");

        internal void Teleport(string args)
            => _ctx.Teleport(args); // redirect for completeness

        internal void Effect(Component value, bool give = true)
            => _ctx._output.AppendLine($"effect {(give == true ? "give" : "clear")} " +
                $"@s {(give == true ? value.ToRaw() : ((EffectComponent)value).GetEffect().ToString().ToLower())}");
        internal void Effect(bool give = true, Effect effect = global::Effect.Speed, uint dur = 30, byte amp = 0, bool hide = false)
        {
            EffectComponent effectComponent = Component.Effect(effect, dur, amp, hide);
            _ctx._output.AppendLine($"effect {(give == true ? "give" : "clear")} " +
                $"@s {(give == true ? effectComponent.ToRaw() : effectComponent.GetEffect().ToString().ToLower())}");
        }
    }

    public GameFunctionEvent()
    {
        Caller = new LocalPlayer(this);
    }

    internal void Message(Component value)
        => _output.AppendLine($"tellraw @a {value.ToJSON()}");

    internal void Time(Component value)
        => _output.AppendLine(value.ToRaw());
    internal void Time(Time value)
        => _output.AppendLine(Component.Time(value).ToRaw());

    internal void Weather(Component value)
        => _output.AppendLine(value.ToRaw());
    internal void Weather(Weather value)
        => _output.AppendLine(Component.Weather(value).ToRaw());

    // TODO: a basic class/enum for targets..
    internal void Effect(string target, Component value, bool give = true)
        => _output.AppendLine($"effect {(give == true ? "give" : "clear")} " +
            $"{target} {(give == true ? value.ToRaw() : ((EffectComponent)value).GetEffect().ToString().ToLower())}");
    internal void Effect(string target, bool give = true, Effect effect = global::Effect.Speed, uint dur = 30, byte amp = 0, bool hide = false)
    {
        EffectComponent effectComponent = Component.Effect(effect, dur, amp, hide);
        _output.AppendLine($"effect {(give == true ? "give" : "clear")} " +
            $"{target} {(give == true ? effectComponent.ToRaw() : effectComponent.GetEffect().ToString().ToLower())}");
    }

    internal void Teleport(string args)
        => _output.AppendLine($"tp {args}"); // not gonna overengineer this yet..

    internal void Raw(string command)
        => _output.AppendLine(command);

    internal string GetOutput() => _output.ToString();
}