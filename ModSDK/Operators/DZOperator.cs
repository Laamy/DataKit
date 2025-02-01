namespace Datapack.Operators;

public class DZOperator : _FloatOperator
{
    public DZOperator(float value) : base(value) { }

    public override string ToRaw() => $"dz={base.ToRaw()}";
}
