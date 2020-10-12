using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using UnityEngine;

namespace Assets.Scripts
{
    public class CalculateCenterOfBuoyancy : MonoBehaviour
    {
        void Start()
        {
            ///
            CalculateBuoyancy(670.8f, 3.0f);
        }
        public Tuple<float,float> CalculateBuoyancy(float volume,float inScale,GameObject inGameObject = null)
        {
            Tuple<float, float> returnValue = new Tuple<float, float>(0,0);
            GameObject submergedHull = null;
            try
            {
                float density = 1.025f;

                float exactSubmergedVolume = volume;
                float scale = inScale ;

                float currentSubmergedVolume = 0.0f;

                SliceMeshVol meshVolume = new SliceMeshVol();

                float previousScale = 0.0f;

                while (currentSubmergedVolume < exactSubmergedVolume  && scale < 5.0f  )
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
                    slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength,true,true, upperHalfHull);

                }
                else
                {
                    slicedHulls = upperHalfHull.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(currentLength,true,true, upperHalfHull);
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


            foreach(var item in coordinates)
            {
                Debug.Log("FOr x : " + item.Item1 + ", Y is : " + item.Item2);
            }
        }
    }
}
