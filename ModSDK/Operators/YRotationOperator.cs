namespace Datapack.Operators;

public class YRotationOperator : _FloatOperator
{
    public YRotationOperator(float value) : base(value) { }

    public override string ToRaw() => $"y_rotation={base.ToRaw()}";
}
