namespace Datapack.Operators;

public class DistanceOperator : OperatorAction
{
    private float _min = 0, _max = -1;

    public DistanceOperator(float min, float max) => (_min, _max) = (min, max);

    public override string ToRaw() => $"distance={(_min == 0 ? "" : _min)}..{(_max == -1 ? "" : _min)}";
}
