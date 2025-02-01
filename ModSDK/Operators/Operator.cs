namespace Datapack.Operators;

using System;
using System.Collections.Generic;

public enum OperatorType
{
    /// <summary>
    /// @a (All) operator for minecraft commands
    /// </summary>
    All,

    /// <summary>
    /// @e (Entity) operator for minecraft commands
    /// </summary>
    AllEnt,

    /// <summary>
    /// @n (Nearest) operator for minecraft commands
    /// </summary>
    NearEnt,

    /// <summary>
    /// @p (Player) operator for minecraft commands
    /// </summary>
    NearPlayer,

    /// <summary>
    /// @r (Random) operator for minecraft commands
    /// </summary>
    Random,

    /// <summary>
    /// @s (Self) operator for minecraft commands
    /// </summary>
    Self
}

public class Operator
{
    private OperatorType type = OperatorType.Self;
    private List<OperatorAction> actions = new List<OperatorAction>();

    public Operator(OperatorType type)
    {
        this.type = type;
    }
    public Operator()
    {
    }

    public Operator SetType(OperatorType type)
    {
        this.type = type;
        return this;
    }

    public Operator AddAction(OperatorAction action)
    {
        this.actions.Add(action);
        return this;
    }

    // NOTE: Wont allow to specify specific players unless you use @a with a name operator cuz theres no point
    private string GetOperator()
    {
        return this.type switch
        {
            OperatorType.All => "@a",
            OperatorType.AllEnt => "@e",
            OperatorType.NearEnt => "@n",
            OperatorType.NearPlayer => "@p",
            OperatorType.Random => "@r",
            OperatorType.Self => "@s",
            _ => throw new ArgumentException()
        };
    }

    public string ToRaw()
    {
        var actionStrings = new List<string>();
        foreach (var action in actions)
            actionStrings.Add(action.ToRaw());
        return $"{GetOperator()}[{string.Join(",", actionStrings)}]";
    }

    // so i can shorten usage syntax
    public static Operator All => new Operator(OperatorType.All);
    public static Operator AllEnt => new Operator(OperatorType.AllEnt);
    public static Operator NearEnt => new Operator(OperatorType.NearEnt);
    public static Operator NearPlayer => new Operator(OperatorType.NearPlayer);
    public static Operator Random => new Operator(OperatorType.Random);
    public static Operator Self => new Operator(OperatorType.Self);

    // more shorteners
    public static AdvancementOperator Advancement(string advancement) => new AdvancementOperator().SetAdvancement(advancement);
    public static DistanceOperator Distance(float min, float max) => new DistanceOperator(min, max);
    public static XOperator X(int x) => new XOperator(x);
    public static XRotationOperator XRotation(int xRotation) => new XRotationOperator(xRotation);
    public static YOperator Y(int y) => new YOperator(y);
    public static YRotationOperator YRotation(int yRotation) => new YRotationOperator(yRotation);
    public static ZOperator Z(int z) => new ZOperator(z);
    public static DXOperator DX(int dx) => new DXOperator(dx);
    public static DYOperator DY(int dy) => new DYOperator(dy);
    public static DZOperator DZ(int dz) => new DZOperator(dz);

    // even MORE shorteners..
    public Operator AddAdvancement(string advancement) => AddAction(Advancement(advancement));
    public Operator AddDistance(float min, float max) => AddAction(Distance(min, max));
    public Operator AddX(int x) => AddAction(X(x));
    public Operator AddXRotation(int xRotation) => AddAction(XRotation(xRotation));
    public Operator AddY(int y) => AddAction(Y(y));
    public Operator AddYRotation(int yRotation) => AddAction(YRotation(yRotation));
    public Operator AddZ(int z) => AddAction(Z(z));
    public Operator AddDX(int dx) => AddAction(DX(dx));
    public Operator AddDY(int dy) => AddAction(DY(dy));
    public Operator AddDZ(int dz) => AddAction(DZ(dz));
}
