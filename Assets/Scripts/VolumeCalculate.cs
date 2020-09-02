using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeCalculate : MonoBehaviour
{
    // public Text countVolume;
    // private float totalVolume;
    // //public GameObject[] objectsToDecal; 
    // MeshFilter mf;

    // void Start()
    // {
    //     mf = object.GetComponent<MeshFilter>();   
    //     Area(mf);
    //     totalVolume = 20;

    // }
    // void Area(Mesh m)
    // {
 
    //     Vector3[] mVertices = m.vertices;// m.bounds;
    //     Vector3 result = Vector3.zero;
    //     for (int p = mVertices.Length - 1, q = 0; q < mVertices.Length; p = q++)
    //     {
    //         result += Vector3.Cross(mVertices[q], mVertices[p]);
    //     }
    //     //Debug.Log(result);
    //     result *= 0.5f;
    //     totalVolume = result.magnitude;
    //     countVolume.text = "Volume : " + totalVolume.ToString();
    // }

    public Text countVolume;
    private float totalVolume;
    
    void Start()
    {
        Vector3 getSize = GetComponent<Renderer>().bounds.size;
        Debug.Log(getSize.x);
        Debug.Log(getSize.y);
        Debug.Log(getSize.z);
        //totalVolume = getSize.x + getSize.y + getSize.z;
        totalVolume = getSize.x * getSize.z;
//        countVolume.text = "Volume : " + totalVolume.ToString();
        Debug.Log("In Vol CAl : total vol :" + totalVolume);
    }
}
