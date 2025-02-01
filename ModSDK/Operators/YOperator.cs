namespace Datapack.Operators;

public class YOperator : _FloatOperator
{
    public YOperator(float value) : base(value) { }

    public override string ToRaw() => $"y={base.ToRaw()}";
}
