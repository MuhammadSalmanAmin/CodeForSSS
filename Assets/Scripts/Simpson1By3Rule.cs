using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using UnityStandardAssets.Water;

namespace Assets.Scripts
{
    public class Simpson1By3Rule : MonoBehaviour
    {
        static float initialMass = 1010.0f;
        static float addedMass = 390f;
        static int splitSize = 100;
        static float locationAtX = -83f;
        static float CLPosition = -5f;

        void Start()
        {
            float totalDisplacement = initialMass + addedMass;
            applySimpson(addedMass,totalDisplacement, 3.0321004f, 3.032004f, locationAtX);
        }

        public void applySimpson(float addedMass,float totalDisplacement,float maxSlicedPoint, float minSlicedPoint,float locationAtX)
        {
            try
            {
                #region Working

                var maxSlicer = maxSlicedPoint;
                var minSlicer = minSlicedPoint;

                System.Collections.Generic.List<Tuple<float, float>> coordinates = new System.Collections.Generic.List<Tuple<float, float>>();

                GameObject originalObject = gameObject;

                GameObject submergedHull1 = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(maxSlicer, false, false, null)[1];// GetSubmergedHull();

                GameObject submergedHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(minSlicer, false, false, submergedHull1)[0];// GetSubmergedHull();

                GameObject[] halfHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(0.005f, true, true, submergedHull);

                GameObject upperHalfHull = halfHull[0];

                Mesh mesh = upperHalfHull.GetComponent<MeshFilter>().sharedMesh;

                var waterplaneLength = Math.Abs(mesh.vertices.Min(x => x.x)) + Math.Abs(mesh.vertices.Max(x => x.x));

                Debug.Log("WL Length  :  " + waterplaneLength);

                float equalChunk = waterplaneLength / splitSize;
                float currentLength = mesh.vertices.Min(x => x.x) + equalChunk;

                coordinates.Add(new Tuple<float, float>(mesh.vertices.Min(x => x.x), 0));

                SliceMeshVol meshVolume = new SliceMeshVol();
                GameObject subHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(minSlicer, false, false)[1];
                var currentSubmergedVolume = meshVolume.VolumeOfMesh(subHull.GetComponent<MeshFilter>().sharedMesh) / 1.025f;


                Debug.Log("VOlume  : " + currentSubmergedVolume);
                int totalFailures = 0;

                for (int i = 1; i <= splitSize; i++)
                {
                    GameObject[] slicedHulls = null;
                    if (currentLength > 0)
                    {
                        slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, true, false, upperHalfHull);

                    }
                    else
                    {
                        slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, false, true, upperHalfHull);
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
                            sliceY = meshed.vertices.Where(x => x.x == currentLength).Max(z => z.z);
                        }
                        else
                        {
                            totalFailures++;
                            sliceY = coordinates[i - 1].Item2 + .00001f;
                        }

                       // Debug.Log("Max y is  : " + sliceY);
                        coordinates.Add(new Tuple<float, float>(currentLength, sliceY));
                    }
                    else
                    {
                      //  Debug.Log("last value : " + mesh.vertices.Where(x => x.x == 3.1419f).First().y);

                        coordinates.Add(new Tuple<float, float>(currentLength, 3.036004f));
                    }
                    currentLength += equalChunk;
                }

                //Debug.Log("Total failtues : " + totalFailures);
                // SecondMomentOfInertia(coordinates, equalChunk);
                //var area = CalculateArea(coordinates, equalChunk);

                #region Verified One

                var Ix = CalculateIX(coordinates, equalChunk);
                var gml = COF(coordinates, equalChunk, currentSubmergedVolume);

                #endregion

                #endregion

                CalculateWaterplaneArea wpArea = new CalculateWaterplaneArea();
                var waterplaneArea = wpArea.Caculate(minSlicedPoint, maxSlicedPoint, originalObject);

                CalculateTrim(addedMass, totalDisplacement, locationAtX, waterplaneArea, waterplaneLength, gml);

                //CalculateList(addedMass, totalDisplacement, Ix, CLPosition , gml,maxSlicedPoint, originalObject);
            }
            catch (Exception ex)
            {
                Debug.Log("Exception : " + ex.ToString());
            }
        }

        #region Trim
        public void CalculateTrim(float addedMass ,float totalDisplacement,float locationATX,float waterplaneArea ,float waterplaneLength ,float gml )
        {
            float mctc = (totalDisplacement * gml) / (waterplaneLength * 100);
            Debug.Log("Mctc is : " + mctc);

            float angleInRadians = (addedMass * locationATX) / (totalDisplacement * gml);
            Debug.Log("Angle in radians : " + angleInRadians);

            double angleInDegree = angleInRadians * (180 / Math.PI);
            Debug.Log("Angle in degree : " + angleInDegree);

            double trim = angleInDegree * waterplaneLength;
            Debug.Log("Trim : " + trim);

            double tpi = (waterplaneArea * 1.025) / 100 ;
            Debug.Log("TPI : " + tpi);
        }

        #endregion

        #region List

        public void CalculateList(float addedMass, float totalDisplacement, float MomentOfInertiaIx, float CLPosition, float gml,float maxSlice,GameObject gameObject)
        {
            float FixedKG = 3.5f;
            float KgZValue = 0.0f;

            float BMt = MomentOfInertiaIx / totalDisplacement;
            Debug.Log("BMt is : " + BMt);

            CalculateCentroidFromVolume centroidFromVolume = new CalculateCentroidFromVolume();

            GameObject[] result = gameObject.GetComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(maxSlice, false, false, null);
            var KB = centroidFromVolume.CalculateKB(result[1].GetComponent<MeshFilter>().sharedMesh);
            Debug.Log("KB is : " + KB);

            var KM = KB + BMt;
            Debug.Log("KM is : " + KM);

            var initialDisplacement = totalDisplacement - addedMass;

            var overallKG = ((initialDisplacement * FixedKG) + (addedMass * KgZValue)) / totalDisplacement;
            Debug.Log("Overall KG is : " + overallKG);

            var GM = KM - overallKG;
            Debug.Log("GM is : " + GM);

            var moment = addedMass * CLPosition;
            Debug.Log("moment is : " + moment);

            var ListAngle = moment / (totalDisplacement * GM) ;
            Debug.Log("List Angle is : " + ListAngle);
        }


        #endregion

        #region No Reference
        public float CalculateArea(System.Collections.Generic.List<Tuple<float, float>> coordinates, float equalChunk)
        {
            var finalArea = 0.0f;
            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    // Debug.Log("First i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item2 * 1;
                }
                else if (i == coordinates.Count() - 1)
                {
                    //Debug.Log("Last i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item2 * 1;
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        //Debug.Log("Multiply by 4 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item2 * 4;
                    }
                    else
                    {
                        // Debug.Log("Multiply by 2 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item2 * 2;
                    }
                }
            }
            finalArea = (equalChunk * finalArea) / 3;

            return finalArea;
        }

        public void CalculateLCF(System.Collections.Generic.List<Tuple<float, float>> coordinates, float waterPlaneArea, float intervalLength)
        {
            float productArea = 0.0f;

            foreach (var item in coordinates)
            {
                float area = item.Item1 * item.Item2;
                productArea += area;
            }

            Debug.Log("Water plane area : " + waterPlaneArea + " Product Area : " + productArea);
            float result = ((2 / waterPlaneArea) * (intervalLength / 3)) * productArea;

            Debug.Log("LCF is : " + result);
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
          
            //float result = (2 * equalChunk* productArea)/3;
            float result = ((2 * equalChunk) / 9) * productArea;
            Debug.Log("Moment of Inertia Ix is : " + result);

            return result;
        }

        #endregion

        #region Google Reference https://flylib.com/books/en/2.22.1/calculating_the_second_moment_of_an_area.html
        public float COF(System.Collections.Generic.List<Tuple<float, float>> coordinates, float equalChunk, float displacement)
        {
            float result = 0.0f;
            float area = 0.0f;

            var splitSize = equalChunk;//0.2f

            //coordinates = new System.Collections.Generic.List<Tuple<float, float>>();
            //coordinates.Add(new Tuple<float, float>(0f, 0.1f));
            //coordinates.Add(new Tuple<float, float>(0.2f, 0.3f));
            //coordinates.Add(new Tuple<float, float>(0.4f, 0.6f));
            //coordinates.Add(new Tuple<float, float>(0.6f, 0.9f));
            //coordinates.Add(new Tuple<float, float>(0.8f, 1.05f));
            //coordinates.Add(new Tuple<float, float>(1f, 1f));
            //coordinates.Add(new Tuple<float, float>(1.2f, 0.7f));
            //coordinates.Add(new Tuple<float, float>(1.4f, 0.4f));
            //coordinates.Add(new Tuple<float, float>(1.6f, 0.2f));
            //coordinates.Add(new Tuple<float, float>(1.8f, 0.1f));
            //coordinates.Add(new Tuple<float, float>(2.0f, 0.050f));

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


           // var gml = (2 * final) / displacement;

            var gml = (final) / displacement;
            Debug.Log("Iyy is : " + final);
            Debug.Log("GML is : " + gml);

            return gml;
        }

        #endregion

        #region YouTube Reference https://www.youtube.com/watch?v=yAnVUFdEvKM
        public void SecondMomentOfInertia(System.Collections.Generic.List<Tuple<float, float>> coordinates, float equalChunk)
        {
            System.Collections.Generic.List<Tuple<float, float>> functionOfAreaCoordinates = new System.Collections.Generic.List<Tuple<float, float>>();
            System.Collections.Generic.List<Tuple<float, float>> functionOfFOMCoordinates = new System.Collections.Generic.List<Tuple<float, float>>();

            var area = FunctionOfArea(coordinates, out functionOfAreaCoordinates);

            #region Testing
            //functionOfAreaCoordinates = new System.Collections.Generic.List<Tuple<float, float>>();
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(0,0));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(0.5f,10f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(1f,12f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(2f,42f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(3f,25f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(4f,54f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(5f,27f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(6f,50f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(7f,22f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(8f,30f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(9f,4.5f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(9.5f,2f));
            //functionOfAreaCoordinates.Add(new Tuple<float, float>(10f,0));
            //equalChunk = 18f;
            #endregion

            FunctionOfFirstMoment(functionOfAreaCoordinates, equalChunk, area, out functionOfFOMCoordinates);

            FunctionOfSecondMoment(functionOfFOMCoordinates, equalChunk, area);
        }
        public float FunctionOfArea(System.Collections.Generic.List<Tuple<float, float>> coordinates, out System.Collections.Generic.List<Tuple<float, float>> newCoordinates)
        {
            var finalArea = 0.0f;

            newCoordinates = new System.Collections.Generic.List<Tuple<float, float>>();

            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    finalArea += 1 * (coordinates[i].Item2);

                    newCoordinates.Add(new Tuple<float, float>(coordinates[i].Item1, 1 * (coordinates[i].Item2)));

                }
                else if (i == coordinates.Count() - 1)
                {
                    finalArea += 1 * (coordinates[i].Item2);
                    newCoordinates.Add(new Tuple<float, float>(coordinates[i].Item1, 1 * (coordinates[i].Item2)));
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        finalArea += 4 * (coordinates[i].Item2);
                        newCoordinates.Add(new Tuple<float, float>(coordinates[i].Item1, 4 * (coordinates[i].Item2)));
                    }
                    else
                    {
                        finalArea += 2 * (coordinates[i].Item2);
                        newCoordinates.Add(new Tuple<float, float>(coordinates[i].Item1, 2 * (coordinates[i].Item2)));
                    }
                }
            }
            return finalArea;
        }
        public float FunctionOfFirstMoment(System.Collections.Generic.List<Tuple<float, float>> coordinates, float equalChunk, float functionOfArea, out System.Collections.Generic.List<Tuple<float, float>> outCoordinates)
        {
            var firstMomentOfArea = 0.0f;

            outCoordinates = new System.Collections.Generic.List<Tuple<float, float>>();
 
            for (int i = 0; i < coordinates.Count(); i++)
            {
               
                firstMomentOfArea += coordinates[i].Item1 * coordinates[i].Item2;
                outCoordinates.Add(new Tuple<float, float>(coordinates[i].Item1, coordinates[i].Item1 * coordinates[i].Item2));
            }

            var centroid = (firstMomentOfArea * equalChunk) / functionOfArea;

            Debug.Log("Center ab8 AP :  " + centroid);

            var actualArea = 2 * (equalChunk / 3) * functionOfArea;

            Debug.Log("Area about AP is :  " + actualArea);

            return centroid;
        }
        public float FunctionOfSecondMoment(System.Collections.Generic.List<Tuple<float, float>> coordinates, float equalChunk, float functionOfArea)
        {
            var finalArea = 0.0f;

            for (int i = 0; i < coordinates.Count(); i++)
            {
                finalArea += coordinates[i].Item1 * coordinates[i].Item2;
            }

            var secondMomentOfArea = 2 * (equalChunk / 3) * (equalChunk * equalChunk) * finalArea;

            Debug.Log("MOI ab8 AP :  " + secondMomentOfArea);

            return secondMomentOfArea;
        }

        #endregion


        #region Center Of Buoyancy
        public Tuple<float, float> CalculateBuoyancy(float volume, float inScale)
        {
            Tuple<float, float> returnValue = new Tuple<float, float>(0, 0);
            GameObject submergedHull = null;
            try
            {
                float density = 1.025f;

                float exactSubmergedVolume = volume;
                float scale = inScale;

                float currentSubmergedVolume = 0.0f;

                SliceMeshVol meshVolume = new SliceMeshVol();

                float previousScale = 0.0f;

                while (currentSubmergedVolume < exactSubmergedVolume && scale < 5.0f)
                {
                    GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(scale, false, false, null);
                    currentSubmergedVolume = meshVolume.VolumeOfMesh(result[1].GetComponent<MeshFilter>().sharedMesh) / density;

                    submergedHull = result[1];

                    string msg = "Volume for draught :   " + scale + " is : " + currentSubmergedVolume + " cube units.";
                    Debug.Log(msg);

                    previousScale = scale;
                    scale += 0.002f;
                }

                returnValue = new Tuple<float, float>(previousScale, scale);

                Debug.Log("Total Displacement : " + (exactSubmergedVolume * 1.025f));
                Debug.Log("Volume : " + currentSubmergedVolume);
                Debug.Log("Draft Amidships : " + scale);
                Debug.Log("Immersed Depth : " + scale);

                var wettedHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(scale, true, true, null);

                CalculateCentroidFromArea centroidFromArea = new CalculateCentroidFromArea();
                var wettedArea = centroidFromArea.AreaOfMesh(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);

                Debug.Log("Area is : " + wettedArea);

                CalculateCentroidFromVolume centroidFromVol = new CalculateCentroidFromVolume();

                centroidFromVol.CalculateKB(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);
                centroidFromVol.CalculateLCB(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

            return returnValue;
        }
        #endregion

        #region test
        public void SLiceIt()
        {
            var splitSize = 10f;
            GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(.1f, true, true, null);
            GameObject upperHalfHull = result[0];

            Mesh mesh = upperHalfHull.GetComponent<MeshFilter>().sharedMesh;

            var totalLength = Math.Abs(mesh.vertices.Min(x => x.x)) + Math.Abs(mesh.vertices.Max(x => x.x));

            Debug.Log("WL Length  :  " + totalLength);

            float equalChunk = totalLength / splitSize;
            float currentLength = mesh.vertices.Min(x => x.x) + equalChunk;

            System.Collections.Generic.List<Tuple<float, float>> coordinates = new System.Collections.Generic.List<Tuple<float, float>>();

            coordinates.Add(new Tuple<float, float>(mesh.vertices.Min(x => x.x), 5));

            SliceMeshVol meshVolume = new SliceMeshVol();

            //int totalFailures = 0;

            for (int i = 1; i <= splitSize; i++)
            {
                GameObject[] slicedHulls = null;
                if (currentLength >= 0)
                {
                    slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, true, true, upperHalfHull);

                }
                else
                {
                    slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, true, true, upperHalfHull);
                }
                if (slicedHulls != null)
                {
                    float sliceY = 0.0f;
                    Mesh meshed = null;
                    if (currentLength == 0)
                    {
                        coordinates.Add(new Tuple<float, float>(currentLength, 5f));
                    }
                    else
                    {
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
                            sliceY = coordinates[i - 1].Item2 + .00001f;
                        }
                        coordinates.Add(new Tuple<float, float>(currentLength, sliceY));
                    }
                }
                else
                {
                    coordinates.Add(new Tuple<float, float>(currentLength, 5f));
                }
                currentLength += equalChunk;
            }


            foreach (var item in coordinates)
            {
                Debug.Log("FOr x : " + item.Item1 + ", Y is : " + item.Item2);
            }


            SecondMomentOfInertia(coordinates, equalChunk);
        }
        #endregion
    }

}