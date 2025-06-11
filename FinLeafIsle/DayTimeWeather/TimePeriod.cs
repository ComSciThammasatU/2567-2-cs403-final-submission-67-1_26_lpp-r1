using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FinLeafIsle.DayTimeWeather
{
    public struct TimePeriod
    {
        public int StartTime;
        public int EndTime;
        public Color Tint;

        public TimePeriod(int start, int end, Color tint, float trans)
        {
            StartTime = start;
            EndTime = end;
            Tint = tint * trans;
        }
    }
}
 
