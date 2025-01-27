public class Component
{
    internal static TextComponent Text(string v) => new TextComponent(v);

    public virtual string ToJSON()
    {
        return string.Empty;
    }
}