using Assets.Scripts;
using Assets.Scripts.Helpers;
using System;
using System.Linq;
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

                var MaximumAndMinimumDraughts = CalculateDraught(totalLoad, scale, AnonymousHullData.Density, AnonymousHullData.MaxScale, AnonymousHullData.IncrementInScale);

                ApplySimpsonRule(MaximumAndMinimumDraughts.Item2, MaximumAndMinimumDraughts.Item1, 5f);


                Debug.Log("Displacement  : " + ShipHydrostaticData.Displacement);
                Debug.Log("Volume  : " + ShipHydrostaticData.Volume);
                Debug.Log("Draught  : " + ShipHydrostaticData.Draught);
                Debug.Log("Immersed Depth  : " + ShipHydrostaticData.ImmersedDepth);
                Debug.Log("Waterplane Length  : " + ShipHydrostaticData.WaterplaneLength);
                Debug.Log("Moment Of Inertia X  : " + ShipHydrostaticData.MomentOfInertiaX);
                Debug.Log("Waterplane Area  : " + ShipHydrostaticData.WaterplaneArea);
                Debug.Log("LCB Length  : " + ShipHydrostaticData.LCBLength);


                Debug.Log("KB  : " + ShipHydrostaticData.KB);
                Debug.Log("BMt  : " + ShipHydrostaticData.BMt);
                Debug.Log("GML  : " + ShipHydrostaticData.GMl);
                Debug.Log("KG  : " + ShipHydrostaticData.KG);

                Debug.Log("KM  : " + ShipHydrostaticData.Km);
                Debug.Log("TPI  : " + ShipHydrostaticData.TPI);
                Debug.Log("Moment Of Inertia Y  : " + ShipHydrostaticData.MomentOfInertiaY);
                Debug.Log("MCTC  : " + ShipHydrostaticData.MCTC);

                Debug.Log("GM  : " + ShipHydrostaticData.GM);
                Debug.Log("LCF Length  : " + ShipHydrostaticData.LCFLength);
                Debug.Log("Trim  : " + ShipHydrostaticData.Trim);
                Debug.Log("List  : " + ShipHydrostaticData.List);

                Debug.Log("Moment  : " + ShipHydrostaticData.Moment);
            

            }
        }
        catch
        {

        }
    }


    /// <summary>
    ///  Returns Maximum and Minimum Scale
    /// </summary>
    /// <param name="totalLoad"></param>
    /// <param name="inScale"></param>
    /// <param name="inDensity"></param>
    /// <param name="maxScale"></param>
    /// <param name="incrementInScale"></param>
    /// <returns></returns>
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
            var runtimeShatterComponent =  gameObject.GetComponent<RuntimeShatterExample>();

            while (currentSubmergedVolume < exactSubmergedVolume && scale < maxScale)
            {
                GameObject[] result = runtimeShatterComponent.SlicedShipHullAlongZ(scale, false, false, null);
                currentSubmergedVolume = meshVolume.VolumeOfMesh(result[1].GetComponent<MeshFilter>().sharedMesh) / density;

                submergedHull = result[1];

                previousScale = scale;
                scale += incrementInScale;
            }

            returnValue = new Tuple<float, float>(previousScale, scale);

            //Debug.Log("Total Displacement : " + (exactSubmergedVolume * density));
            //Debug.Log("Volume : " + currentSubmergedVolume);
            //Debug.Log("Draft Amidships : " + scale);
            //Debug.Log("Immersed Depth : " + scale);

            var wettedHull = runtimeShatterComponent.SlicedShipHullAlongZ(scale, true, true, null);

            #region Caution
            /// Need to review area script
            CalculateCentroidFromArea centroidFromArea = new CalculateCentroidFromArea();
            var wettedArea = centroidFromArea.AreaOfMesh(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);
           // Debug.Log("Area is : " + wettedArea);
            #endregion

            CalculateCentroidFromVolume centroidFromVol = new CalculateCentroidFromVolume();

            var kb = centroidFromVol.CalculateKB(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);
            var lcb = centroidFromVol.CalculateLCB(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);


            ShipHydrostaticData.Draught = scale;
            ShipHydrostaticData.ImmersedDepth = scale;
            ShipHydrostaticData.Volume = currentSubmergedVolume;
            ShipHydrostaticData.Displacement = totalLoad;
            ShipHydrostaticData.KB = kb;
            ShipHydrostaticData.LCBLength = lcb;

        }
        catch (Exception ex)
        {
            Debug.Log("CalculateDraught :: Exception : " + ex.ToString());
        }

        return returnValue;
    }

    public void ApplySimpsonRule(float maxSlicedPoint, float minSlicedPoint, float locationAtX)
    {
        try
        {
            #region Working

            var maxSlicer = maxSlicedPoint;
            var minSlicer = minSlicedPoint;

            System.Collections.Generic.List<Tuple<float, float>> coordinates = new System.Collections.Generic.List<Tuple<float, float>>();

            GameObject originalObject = OriginalGameObject;

            RuntimeShatterExample originalObjectComponent = originalObject.GetComponent<RuntimeShatterExample>();

            GameObject submergedHullFromMaximumSlicingPoint = originalObjectComponent.SlicedShipHullAlongZ(maxSlicer, false, false, null)[1];
            GameObject submergedHullFromMinimumSlicingPoint = originalObjectComponent.SlicedShipHullAlongZ(minSlicer, false, false, submergedHullFromMaximumSlicingPoint)[0]; 

            GameObject[] halfHull = originalObjectComponent.SlicedShipHullHorizontal(AnonymousHullData.ShipHalfOrdinatePointForSimpsonRule, false, false, submergedHullFromMinimumSlicingPoint);

            GameObject upperHalfHull = halfHull[0];

            Mesh mesh = upperHalfHull.GetComponent<MeshFilter>().sharedMesh;

            var waterplaneLength = Math.Abs(mesh.vertices.Min(x => x.x)) + Math.Abs(mesh.vertices.Max(x => x.x));

            //Debug.Log("WL Length  :  " + waterplaneLength);

            float equalChunk = waterplaneLength / AnonymousHullData.SplitSize;
            float currentLength = mesh.vertices.Min(x => x.x) + equalChunk;

            coordinates.Add(new Tuple<float, float>(mesh.vertices.Min(x => x.x), 0));

            SliceMeshVol meshVolume = new SliceMeshVol();
            GameObject subHull = originalObjectComponent.SlicedShipHullAlongZ(minSlicer, false, false)[1];
            var currentSubmergedVolume = meshVolume.VolumeOfMesh(subHull.GetComponent<MeshFilter>().sharedMesh) / AnonymousHullData.Density;

            int totalFailures = 0;

            upperHalfHull.AddComponent<RuntimeShatterExample>();

            for (int i = 1; i <= AnonymousHullData.SplitSize; i++)
            {
                GameObject[] slicedHulls = null;
                if (currentLength > 0)
                {
                    slicedHulls = upperHalfHull.GetComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, false, false, upperHalfHull);

                }
                else
                {
                    slicedHulls = upperHalfHull.GetComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, false, false, upperHalfHull);
                }
                if (slicedHulls != null)
                {
                    float sliceY = 0.0f;
                    Mesh meshed = null;
                    if (currentLength > 0)
                    {
                        meshed = slicedHulls[1].GetComponent<MeshFilter>().sharedMesh;
                    }
                    else
                    {
                        meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
                    }

                    if (meshed.vertices.Any(x => x.x == currentLength))
                    {
                        sliceY = meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y);
                    }
                    else
                    {
                        totalFailures++;
                        sliceY = coordinates[i - 1].Item2 + .00001f;
                    }
                    coordinates.Add(new Tuple<float, float>(currentLength, sliceY));
                }
                else
                {
                    coordinates.Add(new Tuple<float, float>(currentLength, AnonymousHullData.HullFinalOrdinateHeight));
                }
                currentLength += equalChunk;
            }

            #region Verified One

            var Ix = CalculateIX(coordinates, equalChunk);
            var gml = COF(coordinates, equalChunk, currentSubmergedVolume);

            #endregion

            #endregion

            CalculateWaterplaneArea wpArea = new CalculateWaterplaneArea();
            var waterplaneArea = wpArea.Caculate(minSlicedPoint, maxSlicedPoint, originalObject);


            float CLPosition = -5f;

            CalculateTrim(InputData.Conditions.ElementAt(0).LoadAdded, InputData.InitialDisplacement + InputData.Conditions.ElementAt(0).LoadAdded, locationAtX, waterplaneArea, waterplaneLength, gml);
            CalculateList(InputData.Conditions.ElementAt(0).LoadAdded, InputData.InitialDisplacement + InputData.Conditions.ElementAt(0).LoadAdded, Ix, CLPosition, gml, maxSlicedPoint, originalObject);

            ShipHydrostaticData.WaterplaneLength = waterplaneLength;
            ShipHydrostaticData.MomentOfInertiaX = Ix;
            ShipHydrostaticData.GMl = gml;
            ShipHydrostaticData.WaterplaneArea = waterplaneArea;

        }
        catch (Exception ex)
        {
            Debug.Log("Exception : " + ex.ToString());
        }
    }

    public float CalculateIX(System.Collections.Generic.List<Tuple<float, float>> coordinates, float equalChunk)
    {
        float productArea = 0.0f;

        for (int i = 0; i < coordinates.Count(); i++)
        {
            if (i == 0)
            {
                float area = 1 * (coordinates[i].Item2 * coordinates[i].Item2 * coordinates[i].Item2);
                productArea += area;
            }
            else if (i == coordinates.Count() - 1)
            {
                float area = 1 * (coordinates[i].Item2 * coordinates[i].Item2 * coordinates[i].Item2);
                productArea += area;
            }
            else
            {
                if (i % 2 == 1)
                {
                    float area = 4 * (coordinates[i].Item2 * coordinates[i].Item2 * coordinates[i].Item2);
                    productArea += area;
                }
                else
                {
                    float area = 2 * (coordinates[i].Item2 * coordinates[i].Item2 * coordinates[i].Item2);
                    productArea += area;
                }
            }
        }

        float result = ((2 * equalChunk) / 9) * productArea;
        Debug.Log("Moment of Inertia Ix is : " + result);

        return result;
    }

    public float COF(System.Collections.Generic.List<Tuple<float, float>> coordinates, float equalChunk, float displacement)
    {
        float area = 0.0f;

        var splitSize = equalChunk;

        for (int i = 0; i < coordinates.Count(); i++)
        {
            if (i == 0)
            {
                area += 1 * (coordinates[i].Item2);
            }
            else if (i == coordinates.Count() - 1)
            {
                area += 1 * (coordinates[i].Item2);
            }
            else
            {
                if (i % 2 == 1)
                {
                    area += 4 * (coordinates[i].Item2);
                }
                else
                {
                    area += 2 * (coordinates[i].Item2);
                }
            }
        }

        area = (splitSize / 3) * area;

        Debug.Log("Area is  : " + area);

        var squareCoordinates = new System.Collections.Generic.List<Tuple<float, float>>();

        for (int i = 0; i < coordinates.Count(); i++)
        {
            squareCoordinates.Add(new Tuple<float, float>(coordinates[i].Item1 * coordinates[i].Item1, coordinates[i].Item2));
        }

        float Iy = 0.0f;
        for (int i = 0; i < squareCoordinates.Count(); i++)
        {
            if (i == 0)
            {
                Iy += 1 * (squareCoordinates[i].Item2) * squareCoordinates[i].Item1;
            }
            else if (i == squareCoordinates.Count() - 1)
            {
                Iy += 1 * (squareCoordinates[i].Item2) * squareCoordinates[i].Item1;
            }
            else
            {
                if (i % 2 == 1)
                {
                    Iy += 4 * (squareCoordinates[i].Item2) * squareCoordinates[i].Item1;
                }
                else
                {
                    Iy += 2 * (squareCoordinates[i].Item2) * squareCoordinates[i].Item1;
                }
            }
        }

        float centerAtx = 0.0f, centerAty = 0.0f;

        #region Center of area

        float sumProduct = 0.0f;
        for (int i = 0; i < coordinates.Count(); i++)
        {
            if (i == 0)
            {
                sumProduct += 1 * (coordinates[i].Item2) * (coordinates[i].Item1);
            }
            else if (i == coordinates.Count() - 1)
            {
                sumProduct += 1 * (coordinates[i].Item2) * (coordinates[i].Item1);
            }
            else
            {
                if (i % 2 == 1)
                {
                    sumProduct += 4 * (coordinates[i].Item2) * (coordinates[i].Item1);
                }
                else
                {
                    sumProduct += 2 * (coordinates[i].Item2) * (coordinates[i].Item1);
                }
            }
        }


        #endregion

        #region Center of area AT y

        float sumProductForY = 0.0f;
        for (int i = 0; i < coordinates.Count(); i++)
        {
            if (i == 0)
            {
                sumProductForY += 1 * (coordinates[i].Item2) * (coordinates[i].Item2 / 2);
            }
            else if (i == coordinates.Count() - 1)
            {
                sumProductForY += 1 * (coordinates[i].Item2) * (coordinates[i].Item2 / 2);
            }
            else
            {
                if (i % 2 == 1)
                {
                    sumProductForY += 4 * (coordinates[i].Item2) * (coordinates[i].Item2 / 2);
                }
                else
                {
                    sumProductForY += 2 * (coordinates[i].Item2) * (coordinates[i].Item2 / 2);
                }
            }
        }
        centerAty = ((splitSize / 3) * sumProductForY) / area;

        #endregion

        centerAtx = ((splitSize / 3) * sumProduct) / area;

        Iy = (splitSize / 3) * Iy;

        Debug.Log("Center AT X is  : " + centerAtx);
        Debug.Log("Center AT Y is  : " + centerAty);

        // Debug.Log("Iy is  : " + Iy);

        var final = Iy - (2 * area * (centerAtx * centerAtx));


        var gml = (2 * final) / displacement;

        Debug.Log("Iyy is : " + 2 * final);
        Debug.Log("GML is : " + gml);

        return gml;
    }

    #region Trim
    public void CalculateTrim(float addedMass, float totalDisplacement, float locationATX, float waterplaneArea, float waterplaneLength, float gml)
    {
        float mctc = (totalDisplacement * gml) / (waterplaneLength * 100);
        Debug.Log("Mctc is : " + mctc);

        
        float angleInRadians = (addedMass * locationATX) / (totalDisplacement * gml);
        Debug.Log("Angle in radians : " + angleInRadians);

        double angleInDegree = angleInRadians * (180 / Math.PI);
        Debug.Log("Angle in degree : " + angleInDegree);

        double trim = angleInDegree * waterplaneLength;
        Debug.Log("Trim : " + trim);

        double tpi = (waterplaneArea * 1.025) / 100;
        Debug.Log("TPI : " + tpi);

        ShipHydrostaticData.MCTC = mctc;
        ShipHydrostaticData.Trim = trim;
        ShipHydrostaticData.TPI = tpi;
    }

    #endregion

    #region List

    public void CalculateList(float addedMass, float totalDisplacement, float MomentOfInertiaIx, float CLPosition, float gml, float maxSlice, GameObject gameObject)
    {
        float FixedKG = 3.5f;
        float KgZValue = 0.0f;

        float BMt = MomentOfInertiaIx / totalDisplacement;
        //Debug.Log("BMt is : " + BMt);

        CalculateCentroidFromVolume centroidFromVolume = new CalculateCentroidFromVolume();

        GameObject[] result = gameObject.GetComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(maxSlice, false, false, null);
        var KB = centroidFromVolume.CalculateKB(result[1].GetComponent<MeshFilter>().sharedMesh);
        //Debug.Log("KB is : " + KB);

        var KM = KB + BMt;
        //Debug.Log("KM is : " + KM);

        var initialDisplacement = totalDisplacement - addedMass;

        var overallKG = ((initialDisplacement * FixedKG) + (addedMass * KgZValue)) / totalDisplacement;
        //Debug.Log("Overall KG is : " + overallKG);

        var GM = KM - overallKG;
       // Debug.Log("GM is : " + GM);

        var moment = addedMass * CLPosition;
       // Debug.Log("moment is : " + moment);

        var ListAngle = moment / (totalDisplacement * GM);
       // Debug.Log("List Angle is : " + ListAngle);


        ShipHydrostaticData.BMt = BMt;
        ShipHydrostaticData.Km = KM;
        ShipHydrostaticData.KG = overallKG;
        ShipHydrostaticData.GM = GM;
        ShipHydrostaticData.Moment = moment;
        ShipHydrostaticData.List = ListAngle;
    }


    #endregion
}
