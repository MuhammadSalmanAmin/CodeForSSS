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
            // CalculateBuoyancy(1017f,3.9f);
            SLiceIt();
        }
        public Tuple<float,float> CalculateBuoyancy(float volume,float inScale)
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
          
            GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(.1f, true, true, null);

        }
    }
}
