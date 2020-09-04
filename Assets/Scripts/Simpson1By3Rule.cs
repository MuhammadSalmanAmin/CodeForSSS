using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts
{
    public class Simpson1By3Rule : MonoBehaviour
    {
        int splitSize = 3;
        void Start()
        {

            //try
            //{
            //    SliceMeshVol meshVolume = new SliceMeshVol();

            //    // Mesh cut to flatten one surface
            //    GameObject[] halfHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(0.007f);

            //    GameObject upperHalfHull = halfHull[0];

            //    float totalLength = 50.52903f;
            //    float currentLength = -20.705647f;
            //    float maxLength = 24.77048f;
            //    float equalChunk = 5.052903f;

            //    List<GameObject> slicedStations = new List<GameObject>();

            //    // GameObject[] slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(-15.65274f, upperHalfHull);
            //    // var meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
            //    // var maxX = meshed.vertices.Max(x => x.x);
            //    // Debug.Log("Current length : " + currentLength + " x ordinate is : " + maxX + " :::: y ordinate is :  " + meshed.vertices.Where(p => p.x == maxX).Max(que => que.y));


            //    //foreach(var item in meshed.vertices.Where(x=>x.x== maxX))
            //    // {
            //    //     Debug.Log("y length : " + item.y);
            //    // }


            //    for (int i = 0; i < 10; i++)
            //    {
            //        // [1] represent lower hull
            //        GameObject[] slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, upperHalfHull);
            //        //slicedStations.Add(slicedHulls[0]);

            //        var meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
            //        var maxX = meshed.vertices.Max(x => x.x);

            //        Debug.Log("Current length : " + currentLength + " x ordinate is : " + slicedHulls[0].GetComponent<Renderer>().bounds.size.x + " :::: y ordinate is :  " + slicedHulls[0].GetComponent<Renderer>().bounds.size.y);

            //        currentLength += equalChunk;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.Log("Exception : " + ex.ToString());
            //}


            applySimpson();
            //applySimpsonForIntactShip();
        }

        public void applySimpson()
        {
            try
            {
                System.Collections.Generic.List<Tuple<float, float>> coordinates = new System.Collections.Generic.List<Tuple<float, float>>();

                GameObject submergedHull1 = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(3.032006f, false, false, null)[1];// GetSubmergedHull();
                
                GameObject submergedHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(3.030006f, false, false, submergedHull1)[0];// GetSubmergedHull();

                GameObject[] halfHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(0.007f, true, false, submergedHull);

                GameObject upperHalfHull = halfHull[0];

                //var splittt = 14.64324f;
                //GameObject[] slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(splittt, upperHalfHull, true, true);
                //Debug.Log(" Y Axis : " + slicedHulls[0].GetComponent<MeshFilter>().sharedMesh.vertices.Where(x => x.x == splittt).Max(z => z.y));

                Mesh mesh = upperHalfHull.GetComponent<MeshFilter>().sharedMesh;
 

                var totalLength = Math.Abs(mesh.vertices.Min(x => x.x)) + Math.Abs(mesh.vertices.Max(x => x.x));

                //// Debug.Log("Total length is  :  " + totalLength);

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
                        Mesh meshed = null;
                        if (currentLength > 0)
                        {
                            meshed = slicedHulls[1].GetComponent<MeshFilter>().sharedMesh;
                        }
                        else
                        {
                            meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
                        }
                        Debug.Log("X Axis : " + currentLength + " , Y Axis : " + meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y));

                        coordinates.Add(new Tuple<float, float>(currentLength, meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y)));
                    }
                    else
                    {
                        coordinates.Add(new Tuple<float, float>(currentLength, 3.889421f));
                        // Debug.Log("X Axis : " + currentLength + " , Y Axis : " + 3.889421f);
                    }
                    currentLength += equalChunk;

                    // Debug.Log("Current length : " + currentLength);
                }

                Debug.Log("Area is :  " + CalculateArea(coordinates));

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

                GameObject[] halfHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(0.005f, false, false, submergedHull);

                GameObject upperHalfHull = halfHull[0];

                //var splittt = 14.64324f;
                //GameObject[] slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(splittt, upperHalfHull, true, true);
                //Debug.Log(" Y Axis : " + slicedHulls[0].GetComponent<MeshFilter>().sharedMesh.vertices.Where(x => x.x == splittt).Max(z => z.y));

                Mesh mesh = upperHalfHull.GetComponent<MeshFilter>().sharedMesh;


                var totalLength = Math.Abs(mesh.vertices.Min(x => x.x)) + Math.Abs(mesh.vertices.Max(x => x.x));

                //// Debug.Log("Total length is  :  " + totalLength);

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
                        Mesh meshed = null;
                        if (currentLength > 0)
                        {
                            meshed = slicedHulls[1].GetComponent<MeshFilter>().sharedMesh;
                        }
                        else
                        {
                            meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
                        }
                        Debug.Log("X Axis : " + currentLength + " , Y Axis : " + meshed.vertices.Where(x => x.x == currentLength).Max(z => z.y));

                        coordinates.Add(new Tuple<float, float>(currentLength, meshed.vertices.Where(x => x.x == currentLength).Max(z => z.z)));
                    }
                    else
                    {
                        coordinates.Add(new Tuple<float, float>(currentLength, 3.889421f));
                        // Debug.Log("X Axis : " + currentLength + " , Y Axis : " + 3.889421f);
                    }
                    currentLength += equalChunk;

                    // Debug.Log("Current length : " + currentLength);
                }

                Debug.Log("Area is :  " + CalculateArea(coordinates));

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
                    Debug.Log("First i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item2 * 1;
                }
                else if (i == coordinates.Count() - 1)
                {
                    Debug.Log("Last i : " + i + " and value : " + coordinates[i].Item2);
                    finalArea += coordinates[i].Item2 * 1;
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        Debug.Log("Multiply by 4 for i : "+  i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item2 * 4;
                    }
                    else
                    {
                        Debug.Log("Multiply by 2 for i : " + i + " and value : " + coordinates[i].Item2);
                        finalArea += coordinates[i].Item2 * 2;
                    }
                }
            }

            finalArea = (  (splitSize+1) * finalArea) / 3;

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
    }

}