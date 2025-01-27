using System.Text;

public class WorldContext
{
    private StringBuilder _output = new StringBuilder();

    internal void Announce(Component value)
        => _output.AppendLine($"tellraw @a {value.ToJSON()}\r\n");

    internal void Raw(string command)
        => _output.AppendLine($"{command}\r\n");

    internal string GetOutput() => _output.ToString();
}