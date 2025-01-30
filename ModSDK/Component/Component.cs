public class Component
{
    internal static TextComponent Text(string v, string color = null)
    {
        TextComponent textComp = new TextComponent(v);

        if (color != null && color != string.Empty)
            textComp.Color(color);

        return textComp;
    }

    internal static TimeComponent Time(Time time = global::Time.Day) => new TimeComponent(time);

    internal static WeatherComponent Weather(Weather weather = global::Weather.Clear) => new WeatherComponent(weather);

    internal static EffectComponent Effect(Effect effect = global::Effect.Speed, uint dur = 30, byte amp = 0, bool hide = false)
        => new EffectComponent(effect).SetDuration(dur).SetAmplifier(amp).SetHideParticles(hide);

    public virtual string ToJSON()
    {
        return string.Empty;
    }

    public virtual string ToRaw()
    {
        return string.Empty;
    }
}