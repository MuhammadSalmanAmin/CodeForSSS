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
        int splitSize = 70;
        void Start()
        {
            applySimpson();
        }

        public void applySimpson()
        {
            try
            {
                System.Collections.Generic.List<Tuple<float, float>> coordinates = new System.Collections.Generic.List<Tuple<float, float>>();

                GameObject submergedHull1 = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(3.032006f, false, false, null)[1];// GetSubmergedHull();

                GameObject submergedHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(3.030006f, true, false, submergedHull1)[0];// GetSubmergedHull();

                GameObject[] halfHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(0.005f, true, true, submergedHull);

                GameObject upperHalfHull = halfHull[0];

                Mesh mesh = upperHalfHull.GetComponent<MeshFilter>().sharedMesh;

                var totalLength = Math.Abs(mesh.vertices.Min(x => x.x)) + Math.Abs(mesh.vertices.Max(x => x.x));

                Debug.Log("WL Length  :  " + totalLength);

                float equalChunk = totalLength / splitSize;
                float currentLength = mesh.vertices.Min(x => x.x) + equalChunk;

                coordinates.Add(new Tuple<float, float>(mesh.vertices.Min(x => x.x), 0));

                for (int i = 0; i < splitSize; i++)
                {
                    GameObject[] slicedHulls = null;
                    if (currentLength > 0)
                    {
                        slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, upperHalfHull, false, true);

                    }
                    else
                    {
                        slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, upperHalfHull, true, false);
                    }
                    if (slicedHulls != null)
                    {
                        float sliceY = 0.0f;
                        Mesh meshed = null;
                        if (currentLength > 0)
                        {
                            meshed = slicedHulls[1].GetComponent<MeshFilter>().sharedMesh;
                            sliceY = meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y);
                        }
                        else
                        {
                            meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
                            sliceY = meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y);
                        }

                        //Debug.Log("For x  : " + currentLength);
                        //Debug.Log("Y value from mesh is  : " + sliceY);
                        //Debug.Log("Y value from vertices : " + meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y));

                        coordinates.Add(new Tuple<float, float>(currentLength, sliceY));
                        //coordinates.Add(new Tuple<float, float>(currentLength, meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y)));
                    }
                    else
                    {
                        coordinates.Add(new Tuple<float, float>(currentLength, 3.889421f));
                    }
                    currentLength += equalChunk;
                }

               // var area = CalculateArea(coordinates, equalChunk);

               // Debug.Log("Waterplane area is :  " + area * 2);

                //CalculateLCF(coordinates, area * 2, 0.98649f);

                CalculateIX(coordinates);

                SecondMomentOfInertia(coordinates, equalChunk);

                //coordinates = new System.Collections.Generic.List<Tuple<float, float>>();
                //coordinates.Add(new Tuple<float, float>(0f, 0.5f));
                //coordinates.Add(new Tuple<float, float>(5f, 2f));
                //coordinates.Add(new Tuple<float, float>(8f, 1.5f));
                //coordinates.Add(new Tuple<float, float>(10.5f, 4f));
                //coordinates.Add(new Tuple<float, float>(12.5f, 2f));
                //coordinates.Add(new Tuple<float, float>(13.5f, 4f));
                //coordinates.Add(new Tuple<float, float>(13.5f,2f));
                //coordinates.Add(new Tuple<float, float>(12.5f, 4f));
                //coordinates.Add(new Tuple<float, float>(11f, 2f));
                //coordinates.Add(new Tuple<float, float>(7.5f, 4f));
                //coordinates.Add(new Tuple<float, float>(3f, 1.5f));
                //coordinates.Add(new Tuple<float, float>(1f, 2f));
                //coordinates.Add(new Tuple<float, float>(0f, 0.5f));

                //SecondMomentOfInertia(coordinates, equalChunk);
            }
            catch (Exception ex)
            {
                Debug.Log("Exception : " + ex.ToString());
            }
        }

        public void applySimpsonForIntactShip()
        {
            try
            {
                System.Collections.Generic.List<Tuple<float, float>> coordinates = new System.Collections.Generic.List<Tuple<float, float>>();

                GameObject submergedHull1 = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(3.062008f, false, false, null)[1];// GetSubmergedHull();

                GameObject submergedHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(3.060008f, false, false, submergedHull1)[0];// GetSubmergedHull();

                GameObject[] halfHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(0.005f, true, false, submergedHull);

                GameObject upperHalfHull = halfHull[0];

                //var splittt = 14.64324f;
                //GameObject[] slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(splittt, upperHalfHull, true, true);
                //Debug.Log(" Y Axis : " + slicedHulls[0].GetComponent<MeshFilter>().sharedMesh.vertices.Where(x => x.x == splittt).Max(z => z.y));

                Mesh mesh = upperHalfHull.GetComponent<MeshFilter>().sharedMesh;

                GetAllPointsAlongHull(mesh);


                //var totalLength = Math.Abs(mesh.vertices.Min(x => x.x)) + Math.Abs(mesh.vertices.Max(x => x.x));

                ////// Debug.Log("Total length is  :  " + totalLength);

                //float equalChunk = totalLength / splitSize;
                //float currentLength = mesh.vertices.Min(x => x.x) + equalChunk;

                //coordinates.Add(new Tuple<float, float>(mesh.vertices.Min(x => x.x), 0));

                //for (int i = 0; i < splitSize; i++)
                //{
                //    GameObject[] slicedHulls = null;
                //    if (currentLength > 0)
                //    {
                //        slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, upperHalfHull, false, true);

                //    }
                //    else
                //    {
                //        slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, upperHalfHull, true, false);
                //    }
                //    if (slicedHulls != null)
                //    {
                //        Mesh meshed = null;
                //        if (currentLength > 0)
                //        {
                //            meshed = slicedHulls[1].GetComponent<MeshFilter>().sharedMesh;
                //        }
                //        else
                //        {
                //            meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
                //        }
                //        Debug.Log("X Axis : " + currentLength + " , Y Axis : " + meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y));

                //        coordinates.Add(new Tuple<float, float>(currentLength, meshed.vertices.Where(x => x.x == currentLength).Max(z => z.z)));
                //    }
                //    else
                //    {
                //        coordinates.Add(new Tuple<float, float>(currentLength, 3.889421f));
                //        // Debug.Log("X Axis : " + currentLength + " , Y Axis : " + 3.889421f);
                //    }
                //    currentLength += equalChunk;

                //    // Debug.Log("Current length : " + currentLength);
                //}

                //Debug.Log("Area is :  " + CalculateArea(coordinates));

            }
            catch (Exception ex)
            {
                Debug.Log("Exception : " + ex.ToString());
            }

        }
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

        public void GetExtraProperties(Mesh mesh)
        {

            Debug.Log("Min of x : " + mesh.vertices.Min(x => x.x));
            Debug.Log("Max of x : " + mesh.vertices.Max(x => x.x));

            Debug.Log("Min of y : " + mesh.vertices.Min(x => x.y));
            Debug.Log("Max of y : " + mesh.vertices.Max(x => x.y));


            Debug.Log("Min of z : " + mesh.vertices.Min(x => x.z));
            Debug.Log("Max of z : " + mesh.vertices.Max(x => x.z));
        }

        public GameObject GetSubmergedHull()
        {
            CalculateCenterOfBuoyancy buoyancy = new CalculateCenterOfBuoyancy();
            return buoyancy.CalculateBuoyancy();
        }

        public System.Collections.Generic.List<Tuple<float, float>> GetAllPointsAlongHull(Mesh mesh)
        {
            System.Collections.Generic.List<Tuple<float, float>> points = new System.Collections.Generic.List<Tuple<float, float>>();

            float currentLength = mesh.vertices.Min(x => x.x);

            string[] results = new string[] { };

            while (currentLength != mesh.vertices.Max(x => x.x))
            {
                Tuple<float, float> newPoint = new Tuple<float, float>(currentLength, mesh.vertices.Where(x => x.x == currentLength).Max(p => p.z));
                Debug.Log(newPoint);

                //points.Add(newPoint);
                //results[count] = "(" + newPoint.Item1 + " , " + newPoint.Item2 + ")";

                currentLength = mesh.vertices.OrderBy(p => p.x).ToList().FirstOrDefault(x => x.x > currentLength).x;
            }

            string folder = @"D:\SSS\";
            string fileName = "Results.txt";
            string fullPath = folder + fileName;

            File.WriteAllLines(fullPath, results);

            return points;
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

        public void CalculateIX(System.Collections.Generic.List<Tuple<float, float>> coordinates)
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

            float result = 0.21922f * productArea;
            Debug.Log("IXX is : " + result);
        }

        public void SecondMomentOfInertia(System.Collections.Generic.List<Tuple<float, float>> coordinates,float equalChunk)
        {
            var area = FunctionOfArea(coordinates);
      
            FunctionOfFirstMoment(coordinates, equalChunk, area);
            FunctionOfSecondMoment(coordinates, equalChunk, area);
        }


        public float FunctionOfArea(System.Collections.Generic.List<Tuple<float, float>> coordinates)
        {
            var finalArea = 0.0f;

            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    finalArea +=  1 * (coordinates[i].Item2);
                }
                else if (i == coordinates.Count() - 1)
                {
                    finalArea += 1 * (coordinates[i].Item2);
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        finalArea += 4 * (coordinates[i].Item2);
                    }
                    else
                    {
                        finalArea += 2 * (coordinates[i].Item2);
                    }
                }
            }

            return finalArea;
        }

        public float FunctionOfFirstMoment(System.Collections.Generic.List<Tuple<float, float>> coordinates,float equalChunk,float functionOfArea)
        {
            var finalArea = 0.0f;

            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    var fa = 1 * (coordinates[i].Item2);
                    finalArea += fa * coordinates[i].Item1;
                }
                else if (i == coordinates.Count() - 1)
                {
                    var fa = 1 * (coordinates[i].Item2);
                    finalArea += fa * coordinates[i].Item1;
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        var fa = 4 * (coordinates[i].Item2);
                        finalArea += fa * coordinates[i].Item1;
                    }
                    else
                    {
                        var fa = 2 * (coordinates[i].Item2);
                        finalArea += fa * coordinates[i].Item1;
                    }
                }
            }

            var centroid = (finalArea * equalChunk) / functionOfArea;

            Debug.Log("Center ab8 AP :  " + centroid);

            var actualArea = 2 * (equalChunk / 3) * functionOfArea;

            Debug.Log("actualArea :  " + actualArea);

            return centroid;
        }

        public float FunctionOfSecondMoment(System.Collections.Generic.List<Tuple<float, float>> coordinates, float equalChunk, float functionOfArea)
        {
            var finalArea = 0.0f;

            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    var fa = 1 * (coordinates[i].Item2);
                    var st = fa * coordinates[i].Item1;
                    finalArea += st * coordinates[i].Item1;
                }
                else if (i == coordinates.Count() - 1)
                {
                    var fa = 1 * (coordinates[i].Item2);
                    var st = fa * coordinates[i].Item1;
                    finalArea += st * coordinates[i].Item1;
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        var fa = 4 * (coordinates[i].Item2);
                        var st = fa * coordinates[i].Item1;
                        finalArea += st * coordinates[i].Item1;
                    }
                    else
                    {
                        var fa = 2 * (coordinates[i].Item2);
                        var st = fa * coordinates[i].Item1;
                        finalArea += st * coordinates[i].Item1;
                    }
                }
            }

            var secondMomentOfArea = 2 * (equalChunk/3) * (equalChunk * equalChunk) * finalArea;

            Debug.Log("MOI ab8 AP :  " + secondMomentOfArea);

            return secondMomentOfArea;
        }
    }

}