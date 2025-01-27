public class Component
{
    internal static TextComponent Text(string v) => new TextComponent(v);
    internal static TimeComponent Time(Time time = global::Time.Day) => new TimeComponent(time);
    internal static WeatherComponent Weather(Weather weather = global::Weather.Clear) => new WeatherComponent(weather);

    public virtual string ToJSON()
    {
        return string.Empty;
    }

    public virtual string ToRaw()
    {
        return string.Empty;
    }
}