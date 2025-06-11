using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FinLeafIsle.DayTimeWeather
{
    public enum TimeOfDay
    {
        Morning,
        Afternoon,
        Evening,
        Night
    }
    public class DayTime
    {
        public int Time { get;  set; } = 600;
        public int Day { get;  set; } = 1;

        public float TimeSpeed = 60f;
        public float _timeAccumulator = 0;
        private readonly List<TimePeriod> _lightingPeriods = new()
            {
                new TimePeriod(600, 700, new Color(255, 157, 36), 0.1f),  // Morning (6 AM–9 AM)
                new TimePeriod(700, 1700, Color.White, 0f),              // Day
                new TimePeriod(1700, 1930, new Color(255, 157, 36), 0.3f),// Evening
                new TimePeriod(1930, 2800, new Color(0, 10, 21), 0.9f),// Night
                
            };

        public void Update(GameTime gameTime)
        {
            _timeAccumulator += (float)gameTime.ElapsedGameTime.TotalSeconds * TimeSpeed;

            while (_timeAccumulator >= 60f)
            {
                _timeAccumulator -= 60f;
                AdvanceOneMinute();
            }
        }

        public void AdvanceOneMinute()
        {
            int hours = Time / 100;
            int minutes = Time % 100;

            minutes++;

            if (minutes >= 60)
            {
                minutes = 0;
                hours++;
            }
            if (hours >= 26)
            {
                hours = 0;
                Day++;
                Time = 600;
            }
            Time = hours * 100 + minutes;
        }

        public Color GetCurrentTint(int currentTime)
        {
            foreach (var period in _lightingPeriods)
            {
                if (currentTime >= period.StartTime && currentTime < period.EndTime)
                    return period.Tint;
            }

            return Color.White; // fallback
        }

        public string GetFormattedTime()
        {
            int hours = Time / 100;
            int minutes = Time % 100;

            string period = hours >= 12 ? "PM" : "AM";
            int displayHour = hours % 12;
            if (displayHour == 0) displayHour = 12;

            return $"{displayHour:00}:{minutes:00} {period}";
        }

        public string GetFormattedDay() => $"Day {Day}";
    }

}
