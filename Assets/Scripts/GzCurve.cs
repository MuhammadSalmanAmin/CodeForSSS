using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GzCurve : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //Quaternion rotationX = Quaternion.AngleAxis(10, new Vector3(1f, 0f, 0f));
        //gameObject.transform.rotation = rotationX;

        gameObject.AddComponent<RuntimeShatterExample>();

        #region Comment
        //GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(1.417989f, true, true, null);
        //currentSubmergedVolume = meshVolume.VolumeOfMesh(result[0].GetComponent<MeshFilter>().sharedMesh) / density;

        //while (scale < 2f)
        //{
        //    GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullAlongZ(1.417989f, true, true, null);
        //    currentSubmergedVolume = meshVolume.VolumeOfMesh(result[0].GetComponent<MeshFilter>().sharedMesh) / density;

        //    string msg = "Volume for draught :   " + scale + " is : " + currentSubmergedVolume + " cube units.";
        //    Debug.Log(msg);

        //    previousScale = scale;
        //    scale += 0.002f;
        //}
        #endregion

        GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(3.01f, true, true, null);
      
        var mesh = result[1].GetComponent<MeshFilter>().sharedMesh;

        //var waterplaneLength = Math.Abs(mesh.vertices.Min(x => x.x)) + Math.Abs(mesh.vertices.Max(x => x.x));
        //Debug.Log("WL Length  :  " + waterplaneLength);

        CalculateCentroidFromVolume centroidFromVol = new CalculateCentroidFromVolume();
        centroidFromVol.CalculateLCB(mesh);
        centroidFromVol.VolumeOfMesh(mesh);
    }

    private double angleBetweenExample(float angleATz,float angleATy)
    {
        Vector2 vector1 = new Vector2(angleATz, 0);
        Vector2 vector2 = new Vector2(angleATz, angleATy);
        double angleBetween;

        // angleBetween is approximately equal to 0.9548
        angleBetween = Vector2.Angle(vector1, vector2);

        return angleBetween;
    }
}
