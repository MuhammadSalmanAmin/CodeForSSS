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
        public void CalculateBuoyancy()
        {
            try
            {
                float initialLoad = 411.0f;
                float addedLoad = 535.0f;
                float density = 1.025f;

                float totalLoad = (670.8f)/ density;

                float submergedVolume = 0.0f;
                float scale = 2.8f;
 
                SliceMeshVol meshVolume = new SliceMeshVol();
                while (submergedVolume < totalLoad && scale < 5)
                {
                    GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(scale);
                    submergedVolume = meshVolume.VolumeOfMesh(result[1].GetComponent<MeshFilter>().sharedMesh)/density;

                    string msg = "Volume for draught :   " + scale + " is : " + submergedVolume + " cube units.";
                    Debug.Log(msg);

                    scale += 0.005f;
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }
}
