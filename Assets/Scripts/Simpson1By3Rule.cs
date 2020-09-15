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
        int splitSize = 50;
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
                        slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, upperHalfHull, false, false);

                    }
                    else
                    {
                        slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, upperHalfHull, false, false);
                    }
                    if (slicedHulls != null)
                    {
                        Mesh meshed = null;
                        if (currentLength > 0)
                        {
                            meshed = slicedHulls[1].GetComponent<MeshFilter>().sharedMesh;
                        }
                        else
                        {
                            meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
                        }

                        coordinates.Add(new Tuple<float, float>(currentLength, meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y)));
                    }
                    else
                    {
                        coordinates.Add(new Tuple<float, float>(currentLength, 3.889421f));
                    }
                    currentLength += equalChunk;
                }

                var area = CalculateArea(coordinates);

                Debug.Log("Waterplane area is :  " + area * 2);

                CalculateLCF(coordinates, area * 2, 0.98649f);

                CalculateIX(coordinates);
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
        public float CalculateArea(System.Collections.Generic.List<Tuple<float, float>> coordinates)
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
            finalArea = (0.98649f * finalArea) / 3;

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

        public void CalculateIYY(System.Collections.Generic.List<Tuple<float, float>> coordinates)
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
            Debug.Log("IYY is : " + result);
        }

        public void MomentsOfInertia(System.Collections.Generic.List<Tuple<float, float>> coordinates)
        {
            var finalArea = 0.0f;
            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    // Debug.Log("First i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item2 * 1 * (coordinates[i].Item1 * coordinates[i].Item1 * coordinates[i].Item1);
                }
                else if (i == coordinates.Count() - 1)
                {
                    //Debug.Log("Last i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item2 * 1 * (coordinates[i].Item1 * coordinates[i].Item1 * coordinates[i].Item1);
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        //Debug.Log("Multiply by 4 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item2 * 4 * (coordinates[i].Item1 * coordinates[i].Item1 * coordinates[i].Item1);
                    }
                    else
                    {
                        // Debug.Log("Multiply by 2 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item2 * 2 * (coordinates[i].Item1 * coordinates[i].Item1 * coordinates[i].Item1);
                    }
                }
            }
            finalArea = 0.65766f * finalArea;

            Debug.Log("Iy : " + 2 * finalArea);
        }

        public float CalculateAreaForMoment(System.Collections.Generic.List<Tuple<float, float>> coordinates)
        {
            var finalArea = 0.0f;
            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    // Debug.Log("First i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item1 * 1 * (coordinates[i].Item2 * coordinates[i].Item2 * coordinates[i].Item2);
                }
                else if (i == coordinates.Count() - 1)
                {
                    //Debug.Log("Last i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item1 * 1 * (coordinates[i].Item2 * coordinates[i].Item2 * coordinates[i].Item2);
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        //Debug.Log("Multiply by 4 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item1 * 4 * (coordinates[i].Item2 * coordinates[i].Item2 * coordinates[i].Item2);
                    }
                    else
                    {
                        // Debug.Log("Multiply by 2 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item1 * 2 * (coordinates[i].Item2 * coordinates[i].Item2 * coordinates[i].Item2);
                    }
                }
            }
            finalArea = 0.65766f * finalArea;

            Debug.Log("Ix : " + 2 * finalArea);
            return finalArea;
        }

        public void SecondMomentOfInertia(System.Collections.Generic.List<Tuple<float, float>> coordinates)
        {
            //coordinates = new System.Collections.Generic.List<Tuple<float, float>>();
            //coordinates.Add(new Tuple<float, float>(0f, 0.11f));
            //coordinates.Add(new Tuple<float, float>(0.2f, 0.3f));
            //coordinates.Add(new Tuple<float, float>(0.4f, 0.6f));
            //coordinates.Add(new Tuple<float, float>(0.6f, 0.9f));
            //coordinates.Add(new Tuple<float, float>(0.8f, 1.05f));
            //coordinates.Add(new Tuple<float, float>(1.0f, 1f));
            //coordinates.Add(new Tuple<float, float>(1.2f, 0.7f));
            //coordinates.Add(new Tuple<float, float>(1.4f, 0.4f));
            //coordinates.Add(new Tuple<float, float>(1.6f, 0.2f));
            //coordinates.Add(new Tuple<float, float>(1.8f, 0.1f));
            //coordinates.Add(new Tuple<float, float>(2.0f, 0.05f));

            var finalArea = 0.0f;
            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    finalArea += coordinates[i].Item2 * 1 * (coordinates[i].Item1 * coordinates[i].Item1);
                }
                else if (i == coordinates.Count() - 1)
                {
                    finalArea += coordinates[i].Item2 * 1 * (coordinates[i].Item1 * coordinates[i].Item1);
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        finalArea += coordinates[i].Item2 * 4 * (coordinates[i].Item1 * coordinates[i].Item1);
                    }
                    else
                    {
                        finalArea += coordinates[i].Item2 * 2 * (coordinates[i].Item1 * coordinates[i].Item1);
                    }
                }
            }

            finalArea = (0.98649f / 3) * finalArea;

            Debug.Log("Iy : " + finalArea);
        }


        public float CalculateCOFx(System.Collections.Generic.List<Tuple<float, float>> coordinates)
        {
            var finalArea = 0.0f;
            for (int i = 0; i < coordinates.Count(); i++)
            {
                if (i == 0)
                {
                    // Debug.Log("First i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item1 * coordinates[i].Item2 * 1;
                }
                else if (i == coordinates.Count() - 1)
                {
                    //Debug.Log("Last i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item1 * coordinates[i].Item2 * 1;
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        //Debug.Log("Multiply by 4 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item1 * coordinates[i].Item2 * 4;
                    }
                    else
                    {
                        // Debug.Log("Multiply by 2 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item1 * coordinates[i].Item2 * 2;
                    }
                }
            }
            finalArea = ((0.98649f * finalArea) / 3) / 347.57f;

            return finalArea;
        }

    }

}