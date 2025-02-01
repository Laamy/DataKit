namespace Datapack.Operators;

public class _FloatOperator : OperatorAction
{
    private float value = 0;

    public _FloatOperator(float value) => this.value = value;

    public override string ToRaw() => $"{value}";
}
