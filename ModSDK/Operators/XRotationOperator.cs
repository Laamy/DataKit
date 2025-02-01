namespace Datapack.Operators;

public class XRotationOperator : _FloatOperator
{
    public XRotationOperator(float value) : base(value) { }

    public override string ToRaw() => $"x_rotation={base.ToRaw()}";
}
