using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System.Threading;
using UnityEngine.UI;

/**
 * Represents a really badly written shatter script! use for reference purposes only.
 */
public class RuntimeShatterExample : MonoBehaviour
{

    public List<Vector3> result;
    Vector3 direction, positionnew;
    public GameObject objectToShatter;
    public Material crossSectionMaterial;
    public WaterPosition waterPositionObject;

    public List<GameObject> prevShatters = new List<GameObject>();


    public Text countVolume;
    private float totalVolume;

    void Start()
    {
    }


    public GameObject[] SlicedShipHullHorizontal(float draught, bool displayUpperHull = true, bool displayLowerHull = true, GameObject customObject = null)
    {

        objectToShatter = customObject == null ? GameObject.FindGameObjectsWithTag("solid_hull")[0] : customObject;

 
        GameObject[] result = ShatterObject(objectToShatter, draught, displayUpperHull, displayLowerHull, crossSectionMaterial = null);
        return result;
    }

    public GameObject[] SlicedShipHullAlongZ(float draught, bool displayUpperHull, bool displayLowerHull, GameObject customObject = null)
    {

        objectToShatter = customObject == null ? GameObject.FindGameObjectsWithTag("solid_hull")[0] : customObject;
        GameObject[] result = ShatterObjectAlongZAxis(objectToShatter, draught, displayUpperHull, displayLowerHull, crossSectionMaterial = null);
        return result;
    }

    public GameObject[] SlicedVerticalShipHull(float draught, bool displayUpperHull = true, bool displayLowerHull = true, GameObject customObject = null)
    {
        objectToShatter = customObject == null ? GameObject.FindGameObjectsWithTag("solid_hull")[0] : customObject;
        GameObject[] result = VerticalShatterObject(objectToShatter, draught, displayUpperHull, displayLowerHull, crossSectionMaterial = null);
        return result;
    }
    public GameObject[] ShatterObject(GameObject obj, float draught, bool displayUpperHull, bool displayLowerHull, Material crossSectionMaterial = null)
    {
        return obj.SliceInstantiate(GetPlaneAlongYAxis(obj.transform.position, obj.transform.localScale, draught),
                                                            new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f)
                                                            , displayUpperHull, displayLowerHull, crossSectionMaterial);

    }

    public GameObject[] ShatterObjectAlongZAxis(GameObject obj, float draught, bool displayUpperHull, bool displayLowerHull, Material crossSectionMaterial = null)
    {
        return obj.SliceInstantiate(GetPlaneAlongZAxis(obj.transform.position, obj.transform.localScale, draught),
                                                            new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f),
                                                              displayUpperHull, displayLowerHull, crossSectionMaterial);

    }
    public GameObject[] VerticalShatterObject(GameObject obj, float draught, bool displayUpperHull, bool displayLowerHull, Material crossSectionMaterial = null)
    {
        
        return obj.SliceInstantiate(GetPlaneAlongXAxis(obj.transform.position, obj.transform.localScale, draught),
                                                            new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f),
                                                              displayUpperHull, displayLowerHull, crossSectionMaterial);

    }


    public EzySlice.Plane GetPlaneAlongXAxis(Vector3 positionOffset, Vector3 scaleOffset, float draught)
    {
        Vector3 direction = new Vector3(draught, 0f, 0f);
        Vector3 positionnew = new Vector3(draught, 0f, 0f);
        var plane = new EzySlice.Plane(positionnew, direction);
        return plane;
    }


    public EzySlice.Plane GetPlaneAlongYAxis(Vector3 positionOffset, Vector3 scaleOffset, float draught)
    {
        // For 670.8

        // 10 -> -0.1765 -> 1.8/2.77   ---> (0.176325 -> LCB 
        // 20 -> -0.364 -> 1.8/2.725  ---> 0.36397
        // 30 -> -0.5775 -> 1.9/2.65   ---> 0.57735

        // 40 -> -0.8392 -> 1.8/2.77   ---> 0.8391
        // 50 -> -1.192 -> 1.8/2.725  ---> 1.19175
        // 60 -> -1.7325 -> 1.9/2.65   ---> 1.73205

        // 70 -> -2.748 -> 1.8/2.77   ---> 2.74746
        // 80 -> -5.675 -> 1.8/2.725  ---> 5.6713

        // 90 -> -.4 -> 1.9/2.65   ---> 0.57735

        //Vector3 direction = new Vector3(0f, 1f, 1f);
        //Vector3 positionnew = new Vector3(0f, 3f, 1f);

        //Vector3 direction = new Vector3(0f, 1f, -2.748f);
        //Vector3 positionnew = new Vector3(0f, 0.1795f, 0f);

        Vector3 direction = new Vector3(0f, draught, 0f);
        Vector3 positionnew = new Vector3(0f, draught, 0f);

        var plane = new EzySlice.Plane(positionnew, direction);
        return plane;
    }

    public EzySlice.Plane GetPlaneAlongZAxis(Vector3 positionOffset, Vector3 scaleOffset, float draught)
    {
        Vector3 direction = new Vector3(0f, 0f, draught);
        Vector3 positionnew = new Vector3(0f, 0f, draught);
     
        var plane = new EzySlice.Plane(positionnew, direction);
        return plane;
    }

    public EzySlice.Plane GetPlaneAlongAboutYZPlane(Vector3 positionOffset, Vector3 scaleOffset, float draught)
    {
 
        Vector3 direction = new Vector3(0f, -1f, 1f);
        Vector3 positionnew = new Vector3(0f, draught, 1f);
        var plane = new EzySlice.Plane(positionnew, direction);
        return plane;
    }

}
