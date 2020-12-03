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

        var angle = 10;
        var draught = 2.992f;
        gameObject.AddComponent<RuntimeShatterExample>();

        CalculateCentroidFromVolume centroidFromVol = new CalculateCentroidFromVolume();
 
        Quaternion rotationX = Quaternion.AngleAxis(angle, new Vector3(1f, 0f, 0f));
        gameObject.transform.rotation = rotationX;

    
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

        GameObject[] result = gameObject.GetComponent<RuntimeShatterExample>().SlicedShipHullForGzCurve(draught, true, true, null);
      
        var mesh = result[1].GetComponent<MeshFilter>().sharedMesh;


        //var kb1 = centroidFromVol.CalculateLCB(mesh);
       
        //var b1BDesh = centroidFromVol.CalculateLCBZ(mesh);

        
        centroidFromVol.VolumeOfMesh(mesh);

       // CalculateBBdash(1.833f, kb1, b1BDesh, ConvertDegreeToRadians(angle), 3.5f);
    }

    // kb ----   Center of buoyancy when intact
    // Kb1 ----  Center of buoyancy when rotated
    // b1BDesh - Vertical shift of buoyancy
    private float CalculateBBdash(float Kb,float Kb1,float b1BDesh,double angle,float kg)
    {
        var result = 0.0f;

        var bb1 = Kb1 - Kb;
 
        var bb1TanTheta = bb1 * Math.Tan(angle);

        var bDoubleDash = b1BDesh + bb1TanTheta;

        var bBDesh = bDoubleDash * Math.Cos(angle);

        var kn = (Kb * Math.Sin(angle)) + bBDesh;
        var gz = kn - (kg * Math.Sin(angle));

        Debug.Log("kn is : " + kn);
        //Debug.Log("kg is : " + gz);
        return result;
    }

    public static double ConvertDegreeToRadians(double angle)
    {
        double degrees = angle * (Math.PI / 180);
        return (degrees);
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
