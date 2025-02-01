namespace Datapack.Operators;

public class DYOperator : _FloatOperator
{
    public DYOperator(float value) : base(value) { }

    public override string ToRaw() => $"dy={base.ToRaw()}";
}
