namespace Datapack.Operators;

public class DXOperator : _FloatOperator
{
    public DXOperator(float value) : base(value) { }

    public override string ToRaw() => $"dx={base.ToRaw()}";
}
