namespace Datapack.Operators;

public class ZOperator : _FloatOperator
{
    public ZOperator(float value) : base(value) { }

    public override string ToRaw() => $"z={base.ToRaw()}";
}
