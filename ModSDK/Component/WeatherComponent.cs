namespace Datapack.Components
{
    public enum Weather
    {
        Clear, Rain, Thunder
    }

    public class WeatherComponent : Component
    {
        private Weather _currentWeather;
        private int _duration;
        private bool _isDurationSet;

        public WeatherComponent(Weather initialWeather = Components.Weather.Clear)
        {
            _currentWeather = initialWeather;
            _isDurationSet = false;
        }

        public WeatherComponent Set(Weather newWeather)
        {
            _currentWeather = newWeather;
            _isDurationSet = false;
            return this;
        }

        public WeatherComponent SetDuration(int durationInTicks)
        {
            _isDurationSet = true;
            _duration = durationInTicks;
            return this;
        }

        public override string ToRaw()
        {
            if (_isDurationSet)
                return $"weather {_currentWeather.ToString().ToLower()} {_duration}";
            else return $"weather {_currentWeather.ToString().ToLower()}";
        }
    }
}