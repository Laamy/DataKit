using System;

public enum EventType
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
public class EventAttribute : Attribute
{
    public EventType _event
    {
        get;
    }

    public EventAttribute(EventType functionName)
    {
        _event = functionName;
    }
}
public class FunctionAttribute : Attribute
{
}