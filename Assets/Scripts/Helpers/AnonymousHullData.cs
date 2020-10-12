using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helpers
{
    public static class AnonymousHullData
    {
        public static float InitialLoad = 421.7f;
        public static float Density = 1.025f;
        public static float IncrementInScale = 0.002f;
        public static float MaxScale = 5.1f;
        public static float GetNearbyScaleViaLoad(float load)
        {
            float approxScale = 0.0f;

            var TotalLoad = InitialLoad + load;

            if (TotalLoad >= 421.7f && TotalLoad <= 500.4f)
            {
                approxScale = 2.20f;
            }
            else if (TotalLoad > 500.4f && TotalLoad <= 583.8f)
            {
                approxScale = 2.40f;
            }
            else if (TotalLoad > 583.8 && TotalLoad <= 670.8f)
            {
                approxScale = 2.70f;
            }
            else if (TotalLoad > 670.8f && TotalLoad <= 760.5f)
            {
                approxScale = 2.95f;
            }
            else if (TotalLoad > 760.5f && TotalLoad <= 852.6f)
            {
                approxScale = 3.20f;
            }

            else if (TotalLoad > 852.6f && TotalLoad <= 946.9f)
            {
                approxScale = 3.45f;
            }
            else if (TotalLoad > 946.9f && TotalLoad <= 1043f)
            {
                approxScale = 3.70f;
            }

            else if (TotalLoad > 1043f && TotalLoad <= 1141f)
            {
                approxScale = 3.90f;
            }
            else if (TotalLoad > 1141f && TotalLoad <= 1240f)
            {
                approxScale = 4.20f;
            }
            else if (TotalLoad > 1240f && TotalLoad <= 1341f)
            {
                approxScale = 4.45f;
            }
            else if (TotalLoad > 1341f && TotalLoad <= 1443f)
            {
                approxScale = 4.70f;
            }

            return approxScale;
        }

        public static float TotalLoad(float addedLoad)
        {
            return addedLoad + InitialLoad;
        }
    }
}
