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
    }

    public GameFunctionEvent()
    {
        Caller = new LocalPlayer(this);
    }

    internal void Message(Component value)
        => _output.AppendLine($"tellraw @a {value.ToJSON()}");

    internal void Time(Component value)
        => _output.AppendLine(value.ToRaw());

    internal void Weather(Component value)
        => _output.AppendLine(value.ToRaw());

    internal void Teleport(string args)
        => _output.AppendLine($"tp {args}"); // not gonna overengineer this yet..

    internal void Raw(string command)
        => _output.AppendLine(command);

    internal string GetOutput() => _output.ToString();
}