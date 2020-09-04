using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts
{
    public class Simpson1By3Rule : MonoBehaviour
    {
        void Start()
        {
            try
            {
                SliceMeshVol meshVolume = new SliceMeshVol();

                // Mesh cut to flatten one surface
                GameObject[] halfHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(0.007f);
 
                GameObject upperHalfHull = halfHull[0];

                float totalLength = 50.52903f;
                float currentLength = -20.705647f;
                float maxLength = 24.77048f;
                float equalChunk = 5.052903f;

                List<GameObject> slicedStations = new List<GameObject>();

                // GameObject[] slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(-15.65274f, upperHalfHull);
                // var meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
                // var maxX = meshed.vertices.Max(x => x.x);
                // Debug.Log("Current length : " + currentLength + " x ordinate is : " + maxX + " :::: y ordinate is :  " + meshed.vertices.Where(p => p.x == maxX).Max(que => que.y));


                //foreach(var item in meshed.vertices.Where(x=>x.x== maxX))
                // {
                //     Debug.Log("y length : " + item.y);
                // }

 
                for (int i = 0; i < 10; i++)
                {
                    // [1] represent lower hull
                    GameObject[] slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength, upperHalfHull);
                    //slicedStations.Add(slicedHulls[0]);

                    var meshed = slicedHulls[0].GetComponent<MeshFilter>().sharedMesh;
                    var maxX = meshed.vertices.Max(x => x.x);
 
                    Debug.Log("Current length : " + currentLength + " x ordinate is : " + slicedHulls[0].GetComponent<Renderer>().bounds.size.x + " :::: y ordinate is :  " + slicedHulls[0].GetComponent<Renderer>().bounds.size.y);

                    currentLength += equalChunk;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Exception : " + ex.ToString());
            }
        }

        public void GetExtraProperties()
        {
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

            Debug.Log("Min of x : "  + mesh.vertices.Min(x => x.x));
            Debug.Log("Max of x : " + mesh.vertices.Max(x => x.x));

            Debug.Log("Min of y : " + mesh.vertices.Min(x => x.y));
            Debug.Log("Max of y : " + mesh.vertices.Max(x => x.y));


            Debug.Log("Min of z : " + mesh.vertices.Min(x => x.z));
            Debug.Log("Max of z : " + mesh.vertices.Max(x => x.z));
        }
    }

}