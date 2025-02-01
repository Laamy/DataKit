namespace Datapack.Operators;

public class XOperator : _FloatOperator
{
    public XOperator(float value) : base(value) { }

    public override string ToRaw() => $"x={base.ToRaw()}";
}
