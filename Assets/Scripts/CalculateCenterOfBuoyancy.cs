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

                float exactSubmergedVolume = 1308.149f;// 1017.58f;// (1043f)/ density;

                float currentSubmergedVolume = 0.0f;
                float scale = 4.70f;
 
                SliceMeshVol meshVolume = new SliceMeshVol();
                while (currentSubmergedVolume < exactSubmergedVolume && scale < 5)
                {
                    GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(scale);
                    currentSubmergedVolume = meshVolume.VolumeOfMesh(result[1].GetComponent<MeshFilter>().sharedMesh)/density;

                    string msg = "Volume for draught :   " + scale + " is : " + currentSubmergedVolume + " cube units.";
                    Debug.Log(msg);

                    scale += 0.002f;
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }
}
