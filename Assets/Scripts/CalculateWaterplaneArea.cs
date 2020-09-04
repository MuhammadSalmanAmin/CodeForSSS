using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts
{
    public class CalculateWaterplaneArea : MonoBehaviour
    {
        void Start()
        {
            try
            {
                float submergedVolume = 0.0f;
                float density = 1.025f;

                SliceMeshVol meshVolume = new SliceMeshVol();

                //GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(4.849989f);
                //GameObject[] result2 = result[1].AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(4.848989f, result[1]);

                GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(3.062008f);
                //GameObject[] result2 = result[1].AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(3.060008f, result[1]);

                //submergedVolume = meshVolume.VolumeOfMesh(result2[0].GetComponent<MeshFilter>().sharedMesh) ;

                ////string msg = "Volume for draught :   " + " is : " + submergedVolume + " cube units.";

                //string msg = "Waterplane area is : " + submergedVolume/0.002 + " sq units.";
               // Debug.Log(msg);
            }
            catch (Exception ex)
            {
                Debug.Log("Exception : " + ex.ToString());
            }
        }
    }

}