using System;

public enum MCEventType
{
    WorldLoad, WorldTick
}

public class ModuleInfoAttribute : Attribute
{
    public string FunctionName
    {
        get;
    }
    public string Description
    {
        get;
        set;
    }
    public string CompilePath
    {
        get;
        set;
    }

    public ModuleInfoAttribute(string functionName)
    {
        FunctionName = functionName;
    }
}

public class ModuleAuthorAttribute : Attribute
{
    public string FunctionName
    {
        get;
    }

    public ModuleAuthorAttribute(string functionName)
    {
        FunctionName = functionName;
    }
}

public class MCEventAttribute : Attribute
{
    public MCEventType _event
    {
        get;
    }

    public MCEventAttribute(MCEventType functionName)
    {
        _event = functionName;
    }
}

public class MCFunctionAttribute : Attribute
{
}