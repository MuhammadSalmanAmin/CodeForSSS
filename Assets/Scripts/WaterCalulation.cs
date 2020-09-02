using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCalulation : MonoBehaviour
{
    // private float totalVolume;
    // float waterDepth;
    // float waterWidth;
    // float waterLength;
    // float waterDensity;
    // float waterMass;
    // float waterVolume;
    // GameObject mesh;
    // Bounds bodyBounds;
    // Bounds bounds;
    // float mBodyDensity;
    void Start()
    {
        GameObject mesh = GameObject.FindWithTag("solid_hull");
        Mesh meshFilter = mesh.GetComponent<MeshFilter>().mesh;
        Vector3 getSize = meshFilter.bounds.size;
        Debug.Log(" Length : " + getSize.x);
        Debug.Log(" Width : " + getSize.y);
        Debug.Log(" Height : " + getSize.z);


        
       // Bounds bounds = meshFilter.bounds;
    }
}
//     GameObject mesh2 = GameObject.FindWithTag ("WaterProDaytime");

//     waterDepth = Mathf.Sqrt(Mathf.Pow(mesh.transform.position.y, 2.0f) + Mathf.Pow(mesh2.transform.position.y, 2.0f));
//     waterLength = Mathf.Sqrt(Mathf.Pow(bounds.min.x, 2.0f) + Mathf.Pow(bounds.max.x, 2.0f));
//     waterWidth = Mathf.Sqrt(Mathf.Pow(bounds.min.z, 2.0f) + Mathf.Pow(bounds.max.z, 2.0f));

//     waterVolume = waterDepth * waterLength * waterWidth * 7.48f;
//     waterMass = 9.11f * waterDepth * waterLength * waterWidth;
//     waterDensity = waterMass/waterVolume;
//     Debug.Log("In Water Calculation ->  Water Length : " + waterLength);
//     Debug.Log("In Water Calculation ->  Water Width : " + waterWidth);
// }
// void Update()
// {
//       Debug.Log("The Mesh  : " + _mesh);
//     var vertices = _mesh.vertices;
//     var normals = _mesh.normals;
//     for (int i = 0; i < _mesh.vertices.Length; i++)
//     {
//         var vertex = _mesh.vertices[i];
//         var func = _waveNumber * (vertex.x - speed * Time.time);
//         vertex.y = amplitude * Mathf.Sin(func);
//         var tangent = new Vector3(1, _waveNumber * amplitude * Mathf.Cos(func), 0);
//         tangent.Normalize();
//         var normal = new Vector3(-tangent.y, tangent.x, 0);
//         vertices[i] = vertex;
//         normals[i] = normal;
//     }
//     _mesh.vertices = vertices;
//     _mesh.normals = normals;
// }

//  void Update()
// {
//     Vector3[] vertices = mesh.vertices;
//     Vector3[] normals = mesh.normals;
//     for (int i = 0; i < mesh.vertices.Length; i++)
//     {
//         Vector3[] vertex = mesh.vertices[i];
//         var func = _waveNumber * (vertex.x - speed * Time.time);
//         vertex.y = amplitude * Mathf.Sin(func);
//         var tangent = new Vector3(1, _waveNumber * amplitude * Mathf.Cos(func), 0);
//         tangent.Normalize();
//         var normal = new Vector3(-tangent.y, tangent.x, 0);
//         vertices[i] = vertex;
//         normals[i] = normal;
//     }
//     _mesh.vertices = vertices;
//     _mesh.normals = normals;
// }
//    void Start()
//    {
//        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
//        //Debug.Log("In Water Calculation : The mesh is " + mesh);
//        float volume = VolumeOfMesh(mesh);
//        string msg = "In Water Calculation : The volume of the mesh is " + volume + " cube units.";
//        Debug.Log(msg);
//    }


//    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
//    {
//        float v321 = p3.x * p2.y * p1.z;
//        float v231 = p2.x * p3.y * p1.z;
//        float v312 = p3.x * p1.y * p2.z;
//        float v132 = p1.x * p3.y * p2.z;
//        float v213 = p2.x * p1.y * p3.z;
//        float v123 = p1.x * p2.y * p3.z;
//        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
//    }
//    public float VolumeOfMesh(Mesh mesh)
//    {
//        float volume = 0;
//        Vector3[] vertices = mesh.vertices;
//        int[] triangles = mesh.triangles;
//        for (int i = 0; i < mesh.triangles.Length; i += 3)
//     {
//            Vector3 p1 = vertices[triangles[i + 0]];
//            Vector3 p2 = vertices[triangles[i + 1]];
//            Vector3 p3 = vertices[triangles[i + 2]];
//            volume += SignedVolumeOfTriangle(p1, p2, p3);
//        }
//        return Mathf.Abs(volume);
//    }

//}
