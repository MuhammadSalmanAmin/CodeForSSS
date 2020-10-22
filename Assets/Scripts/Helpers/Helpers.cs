using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helpers
{
    public class InputData
    {
        public InputData()
        {

        }

        public InputData(float initialDisplacement)
        {
            InitialDisplacement = initialDisplacement;
            Conditions = new List<LoadingCondition>();
        }

        public InputData(float initialDisplacement, List<LoadingCondition> loadingConditions)
        {
            InitialDisplacement = initialDisplacement;
            Conditions = loadingConditions;
        }
        public float InitialDisplacement { get; set; }
        public List<LoadingCondition> Conditions { get; set; }
    }
    public class LoadingCondition
    {
        public float LoadAdded { get; set; }
        public float LocationAtX { get; set; }
        public float LocationAtY { get; set; }
    }

    public class HydrostaticsData
    {
        public float Displacement { get; set; }
        public float Volume { get; set; }
        public float Draught { get; set; }
        public float ImmersedDepth { get; set; }
        public float WaterplaneLength { get; set; }
        public float MomentOfInertiaX { get; set; }
        public float WaterplaneArea { get; set; }
        public float LCBLength { get; set; }
        public float LCFLength { get; set; }
        public float KB { get; set; }
        public float KG { get; set; }
        public float MomentOfInertiaY { get; set; }
        public float GMl { get; set; }
        public float BMt { get; set; }
        public float Km { get; set; }

        public float MCTC { get; set; }

        public double TPI { get; set; }

        public double Trim { get; set; }

        public float List { get; set; }

        public float GM { get; set; }
        public float Moment { get; set; }
    }
}
