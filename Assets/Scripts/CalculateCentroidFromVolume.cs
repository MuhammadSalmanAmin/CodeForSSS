 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using UnityEngine;
using UnityEngine.UIElements;

public class CalculateCentroidFromVolume : MonoBehaviour
{
    void Start()
    {
        CalculateMeshVolume();
        try
        {
            //DMesh3 meshDetails = StandardMeshReader.ReadMesh("D:\\Python\\hull.obj");

            //double mass = 0.0f;
            //g3.Vector3d center = new g3.Vector3d();
            //double[,] inertia = new double[3, 3];
            //MeshMeasurements.MassProperties(meshDetails, out mass, out center, out inertia);

            //Debug.Log("Volume is :  " + mass);
            //Debug.Log("Center Is  : " + center);

            //  CalculateBuoyancy();


        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }


    public void CalculateBuoyancy()
    {
        try
        {
            float initialLoad = 411.0f;
            float addedLoad = 89.4f;

            float totalLoad = initialLoad + addedLoad;

            float submergedVolume = 0.0f;

            float scale = 2.0f;
            float volume = 0.0f;

            while (submergedVolume < totalLoad && scale < 5)
            {
                GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(scale);
                submergedVolume = VolumeOfMesh(result[1].GetComponent<MeshFilter>().sharedMesh)/1.025f;

                string msg = "Volume for draught :   " + scale  + " is : " + submergedVolume + " cube units.";
                Debug.Log(msg);

                scale += 0.1f;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public float CalculateMeshVolume()
    {
        Debug.Log("===============The algorithm written displays following results============== ");
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        float volume = VolumeOfMesh(mesh);
        string msg = "Volume is " + volume + " cube units.";
        Debug.Log(msg);
        Debug.Log("===============The algorithm written displays following results============== ");
        return volume;
    }

    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        var x = (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);

        return x;
    }

    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0.0f;
        float volume_sum = 0.0f;
        Vector3 centroid = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;


        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            var cross = Vector3.Cross(p1, p2);
            var dot = UnityEngine.Vector3.Dot(cross, p3);

            volume = dot / 6;
            centroid += volume * (p1 + p2 + p3) / 4;
            volume_sum += volume;
        }
        centroid /= volume_sum;

        Debug.Log("Centroid From Volume  -> Centroid : " + centroid);

        return (volume_sum);
    }

    public float CalculateKB(Mesh mesh)
    {
        float volume = 0.0f;
        float volume_sum = 0.0f;
        Vector3 centroid = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;


        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            var cross = Vector3.Cross(p1, p2);
            var dot = UnityEngine.Vector3.Dot(cross, p3);

            volume = dot / 6;
            centroid += volume * (p1 + p2 + p3) / 4;
            volume_sum += volume;
        }
        centroid /= volume_sum;
        //Debug.Log("KB : " + centroid.z);
        return (centroid.z);
    }

    public float CalculateLCF(Mesh mesh)
    {
        float volume = 0.0f;
        float volume_sum = 0.0f;
        Vector3 centroid = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;


        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            var cross = Vector3.Cross(p1, p2);
            var dot = UnityEngine.Vector3.Dot(cross, p3);

            volume = dot / 6;
            centroid += volume * (p1 + p2 + p3) / 4;
            volume_sum += volume;
        }
        centroid /= volume_sum;
        Debug.Log("LCF : " + centroid.x);
        return (centroid.x);
    }
    public float CalculateLCB(Mesh mesh)
    {
        float volume = 0.0f;
        float volume_sum = 0.0f;
        Vector3 centroid = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;


        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            var cross = Vector3.Cross(p1, p2);
            var dot = UnityEngine.Vector3.Dot(cross, p3);

            volume = dot / 6;
            centroid += volume * (p1 + p2 + p3) / 4;
            volume_sum += volume;
        }
        centroid /= volume_sum;

        Debug.Log("LCB : " + centroid.x);
        return (centroid.x);
    }
    public float VolumeOfMesh1(Mesh mesh)
    {
        float volume = 0.0f;
        float volume_sum = 0.0f;
        Vector3 centroid = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        var totalSUm = 0.0f;
        var final = new Vector3();

        double maxX = vertices.Max(x => x.x) + vertices.Min(x => x.x);
        double maxY = vertices.Max(x => x.y) + vertices.Min(x => x.y);
        double maxZ = vertices.Max(x => x.z) + vertices.Min(x => x.z);


        Debug.Log("MAx X " + vertices.Max(x => x.x));
        Debug.Log("MAx Y " + vertices.Max(x => x.y));
        Debug.Log("MAx Z " + vertices.Max(x => x.z));


        Debug.Log("Mix X " + vertices.Min(x => x.x));
        Debug.Log("MAx Y " + vertices.Min(x => x.y));
        Debug.Log("MAx Z " + vertices.Min(x => x.z));

        Debug.Log("x,y,z" + maxX / vertices.Count() + ", " + maxY / vertices.Count() + " ," + maxZ / vertices.Count());


        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            Vector3 center = (p1 + p2 + p3) / 3;

            Debug.Log("For triangle " + i + "p1 : " + p1 + "  p2 : " + p2 + "  p3 :  " + p3);

            // totalSUm += (volume * center.y);
            //volume = center * volume;
            //volume_sum += volume;

            volume = SignedVolumeOfTriangle(p1, p2, p3);
            centroid += volume * center;

            volume_sum += volume;

            break;

        }
        centroid /= volume_sum;
        Debug.Log("In Calculate Centroid From Volume  -> Centroid : " + centroid);
        return (volume_sum);
    }
}
