namespace Datapack.Components;

using System.Text;

public class TextComponent : Component
{
    public TextComponent(string text)
    {
        _text = text;
    }

    private string _text;
    private string _color;

    internal TextComponent Color(string colorCode)
    {
        _color = colorCode;
        return this;
    }

    public override string ToJSON()
    {
        StringBuilder json = new StringBuilder("{");

        if (!string.IsNullOrEmpty(_text))
        {
            json.Append($"\"text\": \"{_text}\"");
        }

        if (!string.IsNullOrEmpty(_color))
        {
            if (json.Length > 1) json.Append(", ");
            json.Append($"\"color\": \"{_color}\"");
        }

        json.Append("}");

        return json.ToString();
    }
}