//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CalculateCentroidFromArea : MonoBehaviour
//{
//    void Start()
//    {
//        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
//        float area = AreaOfMesh(mesh);
//        string msg = "Area of mesh is " + area + " cube units.";
//        Debug.Log(msg);
//    }
//    public float AreaOfMesh(Mesh mesh)
//    {
//        float area_sum = 0.0f;
//        Vector3[] vertices = mesh.vertices;
//        int[] triangles = mesh.triangles;
//        Vector3 centroid = new Vector3(0.0f, 0.0f, 0.0f);
//        Vector3 sumofallR = new Vector3(0.0f, 0.0f, 0.0f);
//        for (int i = 0; i < mesh.triangles.Length; i+=3)
//        {
//            Vector3 p1 = vertices[triangles[i + 0]];
//            Vector3 p2 = vertices[triangles[i + 1]];
//            Vector3 p3 = vertices[triangles[i + 2]];
//            Vector3 r = (p1 + p2 + p3) / 3;
//            float a = (0.5f * (Vector3.Cross(p2 - p1, p3 - p1)).magnitude) * 2; 
//            centroid += a * r;
//            area_sum += a;

//            sumofallR += r;
//        }

//     //   var c = centroid / sumofallR;

//        centroid /= area_sum;
//        Debug.Log("In Calculate Centroid From Area  -> Centroid : " + centroid);
//        return area_sum;
//    }
//}


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalculateCentroidFromArea : MonoBehaviour
{
    void Start()
    {
        //Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        //float area = AreaOfMesh(mesh);
        //string msg = "Area of mesh is " + area + " cube units.";
        //Debug.Log(msg);
    }
    public float AreaOfMesh(Mesh mesh)
    {
        float area_sum = 0.0f;
        Vector3[] vertices = mesh.vertices;

        float max = mesh.vertices.Select(x => x.x).Max();
        float min = mesh.vertices.Select(x => x.x).Min();

      //  Debug.Log("Max : " + max + "  , Min : " + min);
       // Debug.Log("Final Length  : " + max + min);

        int[] triangles = mesh.triangles;
        Vector3 centroid = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            Vector3 center = (p1 + p2 + p3) / 4;

          
            float area = 0.5f * (Vector3.Cross(p2 - p1, p3 - p1)).magnitude;
            centroid += area * center;
            area_sum += area;
        }
        centroid /= area_sum;
       // Debug.Log("Centroid from area is : " + centroid);
        return area_sum;
    }
}