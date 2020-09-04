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
           //  CalculateBuoyancy();
        }
        public GameObject CalculateBuoyancy()
        {
            GameObject submergedHull = null;
            try
            {
                float initialLoad = 411.0f;
                float addedLoad = 535.0f;
                float density = 1.025f;

                float exactSubmergedVolume = 654.413f;

                float currentSubmergedVolume = 0.0f;
                float scale = 2.9f;

 
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

                gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(scale, true, true, null);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

            return submergedHull;
        }
    }
}
