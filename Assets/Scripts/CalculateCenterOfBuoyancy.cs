using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CalculateCenterOfBuoyancy : MonoBehaviour
    {
        void Start()
        {
            ///
            CalculateBuoyancy();
        }
        public GameObject CalculateBuoyancy()
        {
            GameObject submergedHull = null;
            try
            {
                float initialLoad = 411.0f;
                float addedLoad = 535.0f;
                float density = 1.025f;

                float exactSubmergedVolume = 1017.58f;

                float currentSubmergedVolume = 0.0f;
                float scale = 3.9f;


                SliceMeshVol meshVolume = new SliceMeshVol();
                while (currentSubmergedVolume < exactSubmergedVolume && scale < 5)
                {
                    GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(scale, false, false, null);
                    currentSubmergedVolume = meshVolume.VolumeOfMesh(result[1].GetComponent<MeshFilter>().sharedMesh) / density;

                    submergedHull = result[1];

                    string msg = "Volume for draught :   " + scale + " is : " + currentSubmergedVolume + " cube units.";
                    Debug.Log(msg);

                    scale += 0.002f;
                }

                Debug.Log("Total Displacement : " + (654.413f * 1.025f));
                Debug.Log("Volume : " + currentSubmergedVolume);
                Debug.Log("Draft Amidships : " + scale);
                Debug.Log("Immersed Depth : " + scale);

                var wettedHull = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(scale, false, false, null);
            
                CalculateCentroidFromArea centroidFromArea = new CalculateCentroidFromArea();
                var wettedArea = centroidFromArea.AreaOfMesh(gameObject.GetComponent<MeshFilter>().sharedMesh) - centroidFromArea.AreaOfMesh(wettedHull[0].GetComponent<MeshFilter>().sharedMesh);

                Debug.Log("Area is : " + wettedArea);

                CalculateCentroidFromVolume centroidFromVol = new CalculateCentroidFromVolume();

                centroidFromVol.CalculateKB(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);
                centroidFromVol.CalculateLCB(wettedHull[1].GetComponent<MeshFilter>().sharedMesh);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

            return submergedHull;
        }
    }
}
