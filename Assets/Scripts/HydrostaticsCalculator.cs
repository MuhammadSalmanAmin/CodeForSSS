using Assets.Scripts;
using Assets.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Water;

public class HydrostaticsCalculator : MonoBehaviour
{
    /// <summary>
    ///  Load is always taken in tons
    ///  Volume is taken as Load / Density
    /// </summary>
    public static InputData InputData { get; set; }
    public static GameObject OriginalGameObject { get; set; }
    public static HydrostaticsData ShipHydrostaticData { get; set; }
    #region Constructors
    public HydrostaticsCalculator()
    {
        ShipHydrostaticData = new HydrostaticsData();
    }
    public HydrostaticsCalculator(GameObject gameObject)
    {
        OriginalGameObject = gameObject;
        ShipHydrostaticData = new HydrostaticsData();
    }
    public HydrostaticsCalculator(InputData inputData, GameObject gameObject)
    {
        InputData = inputData;
        OriginalGameObject = gameObject;
        ShipHydrostaticData = new HydrostaticsData();
    }

    #endregion

    void Start()
    {
        ShipHydrostaticData = new HydrostaticsData();
        OriginalGameObject = gameObject;
        InputData = new InputData()
        {
            InitialDisplacement = AnonymousHullData.InitialLoad,
            Conditions = new List<LoadingCondition>()
            {
                new LoadingCondition() { LoadAdded = 249.1f }
            }
        };

        CaculateHydrostatics();
    }
    public void CaculateHydrostatics()
    {
        try
        {
            foreach (var condition in InputData.Conditions)
            {
                var scale = AnonymousHullData.GetNearbyScaleViaLoad(condition.LoadAdded);
                var totalLoad = AnonymousHullData.TotalLoad(condition.LoadAdded);

                var Draughts = CalculateDraught(totalLoad, scale, AnonymousHullData.Density, AnonymousHullData.MaxScale, AnonymousHullData.IncrementInScale);
            }
        }
        catch
        {

        }
    }


    public Tuple<float, float> CalculateDraught(float totalLoad, float inScale,float inDensity,float maxScale,float incrementInScale)
    {
        Tuple<float, float> returnValue = new Tuple<float, float>(0, 0);
        try
        {
            #region Declaration

            GameObject submergedHull = null;
            float density = inDensity;
            float exactSubmergedVolume = totalLoad/density;
            float scale = inScale;
            float currentSubmergedVolume = 0.0f;
            SliceMeshVol meshVolume = new SliceMeshVol();
            float previousScale = 0.0f;

            #endregion

            gameObject.AddComponent<RuntimeShatterExample>();

            while (currentSubmergedVolume < exactSubmergedVolume && scale < maxScale)
            {
                GameObject[] result = gameObject.GetComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(scale, false, false, null);
                currentSubmergedVolume = meshVolume.VolumeOfMesh(result[1].GetComponent<MeshFilter>().sharedMesh) / density;

                submergedHull = result[1];

                previousScale = scale;
                scale += incrementInScale;
            }

            returnValue = new Tuple<float, float>(previousScale, scale);

            Debug.Log("Total Displacement : " + (exactSubmergedVolume * density));
            Debug.Log("Volume : " + currentSubmergedVolume);
            Debug.Log("Draft Amidships : " + scale);
            Debug.Log("Immersed Depth : " + scale);

            var wettedHull = gameObject.GetComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(scale, true, true, null);

            CalculateCentroidFromArea centroidFromArea = new CalculateCentroidFromArea();
            var wettedArea = centroidFromArea.AreaOfMesh(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);

            Debug.Log("Area is : " + wettedArea);

            CalculateCentroidFromVolume centroidFromVol = new CalculateCentroidFromVolume();

            var kb = centroidFromVol.CalculateKB(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);
            centroidFromVol.CalculateLCB(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);


            ShipHydrostaticData.Draught = scale;
            ShipHydrostaticData.ImmersedDepth = scale;
            ShipHydrostaticData.Volume = currentSubmergedVolume;
            ShipHydrostaticData.Displacement = totalLoad;
            ShipHydrostaticData.KB = kb;

        }
        catch (Exception ex)
        {
            Debug.Log("CalculateDraught :: Exception : " + ex.ToString());
        }

        return returnValue;
    }
}
