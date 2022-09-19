using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnumTypes
{
    public struct EnumTypeRange
    {
        public int min;
        public int max;
        public int range { get { return max - min + 1; } }
        public EnumTypeRange(int aMin, int aMax)
        {
            min = aMin; max = aMax;
        }

        public static int RandomValueFromRanges(params EnumTypeRange[] ranges)
        {
            if (ranges.Length == 0)
                return 0;
            int count = 0;
            foreach (EnumTypeRange r in ranges)
                count += r.range;
            int sel = UnityEngine.Random.Range(0, count);
            foreach (EnumTypeRange r in ranges)
            {
                if (sel < r.range)
                {
                    return r.min + sel;
                }
                sel -= r.range;
            }
            throw new Exception("Error generating random from ranges");
        }
    }
}
