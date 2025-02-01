namespace Datapack.Operators;

public class AdvancementOperator : OperatorAction
{
    private string _advancement; // gonna leave this as a string for now..
    //advancements: nbt (example {minecraft:story/shiny_gear=true})
    public AdvancementOperator() { }

    public AdvancementOperator SetAdvancement(string advancement)
    {
        _advancement = advancement;
        return this;
    }

    public override string ToRaw() => $"advancement={_advancement}";
}
