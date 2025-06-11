using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLeafIsle.Components.ItemComponent
{
    public enum PatternType
    {
        floater = 0,
        dart = 1,
        dartMaster = 2,
        smooth = 3,
        intervMix = 4,
        probMix = 5,
        
    }
    public class FishBehaviorDefinition
    {
        public PatternType PatternType;
        public int Frequency;
        public float MoveTimeMin;
        public float MoveTimeMax;
        public float Strength;
        public float TiredTime;
        public float TiredStrengthMultiplier;
        public float TiredDuration;
        public List<float> ExtraParams = new();
 
    }

}
