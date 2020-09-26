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

    public GameObject[] SlicedVerticalShipHull(float draught, GameObject customObject = null, bool displayUpperHull = true, bool displayLowerHull = true)
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
        Vector3 direction = new Vector3(0f, draught, 0f);
        Vector3 positionnew = new Vector3(15f, draught, 0f);
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


}
