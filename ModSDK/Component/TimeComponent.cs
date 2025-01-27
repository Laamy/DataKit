public enum Time
{
    Day, Midnight, Night, Noon
}

public class TimeComponent : Component
{
    private Time _currentTime;
    private int _timeToAdd;
    private bool _isAddMode;

    public TimeComponent(Time initialTime = global::Time.Day)
    {
        _currentTime = initialTime;
        _isAddMode = false;
    }

    public TimeComponent Set(Time newTime)
    {
        _currentTime = newTime;
        _isAddMode = false;
        return this;
    }

    public TimeComponent Add(int timeInTicks)
    {
        _isAddMode = true;
        _timeToAdd = timeInTicks;
        return this;
    }

    public override string ToRaw()
    {
        if (_isAddMode)
            return $"time add {_timeToAdd}";
        else return $"time set {_currentTime.ToString().ToLower()}";
    }
}
