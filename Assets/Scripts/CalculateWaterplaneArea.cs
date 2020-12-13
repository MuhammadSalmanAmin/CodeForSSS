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

            Debug.Log("Min : "+ gameObject.GetComponent<MeshFilter>().sharedMesh.vertices.Min(x => x.x));

            Debug.Log("Max : " + gameObject.GetComponent<MeshFilter>().sharedMesh.vertices.Max(x => x.x));
            //try
            //{
            //    float submergedVolume = 0.0f;
            //    float density = 1.025f;

            //    SliceMeshVol meshVolume = new SliceMeshVol();

            //    //GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(4.849989f);
            //    //GameObject[] result2 = result[1].AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(4.848989f, result[1]);

            //    GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(3.002008f, false, false);
            //    GameObject[] result2 = result[1].AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(3.000008f, false, false, result[1]);

            //    submergedVolume = meshVolume.VolumeOfMesh(result2[0].GetComponent<MeshFilter>().sharedMesh);

            //    string msg = "Volume for draught :   " + " is : " + submergedVolume + " cube units.";

            //    msg = "Waterplane area is : " + submergedVolume / 0.002 + " sq units.";
            //    Debug.Log(msg);
            //}
            //catch (Exception ex)
            //{
            //    Debug.Log("Exception : " + ex.ToString());
            //}
        }

        public float Caculate(float minSliced, float maxSliced,GameObject gameObject)
        {
            float submergedVolume = 0.0f;
            float waterplaneArea = 0.0f;
            try
            {
                SliceMeshVol meshVolume = new SliceMeshVol();

                GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(maxSliced, false, false);
                GameObject[] result2 = result[1].AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(minSliced, false, false, result[1]);

                submergedVolume = meshVolume.VolumeOfMesh(result2[0].GetComponent<MeshFilter>().sharedMesh);

                waterplaneArea = submergedVolume / (maxSliced - minSliced);

                string msg = "Volume for draught :   " + " is : " + submergedVolume + " cube units.";

                msg = "Waterplane area is : " + waterplaneArea + " sq units.";
                Debug.Log(msg);

            }
            catch (Exception ex)
            {
                Debug.Log("Exception : " + ex.ToString());
            }
            return waterplaneArea;
        }
    }

}