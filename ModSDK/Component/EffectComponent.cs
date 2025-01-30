public enum Effect
{
    Speed,
    Slowness,
    Haste,
    Mining_Fatigue,
    Strength,
    Instant_Health,
    Instant_Damage,
    Jump_Boost,
    Nausea,
    Regeneration,
    Resistance,
    Fire_Resistance,
    Infested,
    Oozing,
    Weaving,
    Wind_Charged,
    Water_Breathing,
    Invisibility,
    Blindness,
    Night_Vision,
    Hunger,
    Weakness,
    Poison,
    /// <summary>
    /// BEDROCK ONLY (for parrots)
    /// </summary>
    Fatal_Poison,
    Wither,
    Health_Boost,
    Absorption,
    Saturation,
    /// <summary>
    /// JAVA ONLY
    /// </summary>
    Glowing,
    Levitation,
    Luck,
    Unluck,
    Slow_Falling,
    Conduit_Power,
    Dolphins_Grace,
    Bad_Omen,
    /// <summary>
    /// THIS IS FOR JAVA USE Village_Hero FOR BEDROCK!
    /// </summary>
    Hero_of_the_Village,
    /// <summary>
    /// THIS IS FOR BEDROCK USE Hero_of_the_Village FOR JAVA!
    /// </summary>
    Village_Hero,
    Darkness,
    Raid_Omen,
    Trial_Omen
}

// effect give @s minecraft:speed infinite 1 true
public class EffectComponent : Component
{
    private Effect _currentEffect;

    private uint _duration;
    private byte _amplifier;
    private bool _hideParticles;

    public EffectComponent(Effect initialEffect = global::Effect.Speed)
    {
        _currentEffect = initialEffect;
    }

    public EffectComponent FromRaw(string values = "minecraft:speed 1")
    {
        return this;
    }

    public EffectComponent Set(Effect newEffect)
    {
        _currentEffect = newEffect;
        return this;
    }

    public EffectComponent SetDuration(uint durationInTicks)
    {
        _duration = durationInTicks;
        return this;
    }

    public EffectComponent SetAmplifier(byte amplifier)
    {
        _amplifier = amplifier;
        return this;
    }

    public EffectComponent SetHideParticles(bool hideParticles)
    {
        _hideParticles = hideParticles;
        return this;
    }

    public Effect GetEffect()
    {
        return _currentEffect;
    }

    public override string ToRaw()
    {
        string command = $"minecraft:{_currentEffect.ToString().ToLower()} {_duration} {_amplifier} {_hideParticles.ToString().ToLower()}";

        return command;
    }
}
