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


    public GameObject[] SlicedShipHullHorizontal(float draught, GameObject customObject = null)
    {
 
        objectToShatter = customObject == null ? GameObject.FindGameObjectsWithTag("solid_hull")[0] : customObject;
        GameObject[] result = ShatterObject(objectToShatter, draught, crossSectionMaterial = null);
        return result;
    }


    public GameObject[] SlicedVerticalShipHull(float draught)
    {
        objectToShatter = GameObject.FindGameObjectsWithTag("solid_hull")[0];
        GameObject[] result = VerticalShatterObject(objectToShatter, draught, crossSectionMaterial = null);
        return result;
    }
    public GameObject[] ShatterObject(GameObject obj,float draught, Material crossSectionMaterial = null )
    {
        return obj.SliceInstantiate(GetHorizontalPlane(obj.transform.position, obj.transform.localScale, draught),
                                                            new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f),
                                                            crossSectionMaterial);

    }


    public GameObject[] VerticalShatterObject(GameObject obj, float draught, Material crossSectionMaterial = null)
    {
        return obj.SliceInstantiate(GetVerticalPlane(obj.transform.position, obj.transform.localScale, draught),
                                                            new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f),
                                                            crossSectionMaterial);

    }


    public EzySlice.Plane GetVerticalPlane(Vector3 positionOffset, Vector3 scaleOffset, float draught)
    {
        Vector3 direction = new Vector3(-10f, 0f, 0f);
        Vector3 positionnew = new Vector3(-10f, 0f, 0f);
        var plane = new EzySlice.Plane(positionnew, direction); 
        return plane;
    }

    public EzySlice.Plane GetHorizontalPlane(Vector3 positionOffset, Vector3 scaleOffset, float draught)
    {
        Vector3 direction = new Vector3(0f, draught, 0f);
        Vector3 positionnew = new Vector3(15f, draught, 0f);
        var plane = new EzySlice.Plane(positionnew, direction);
        return plane;
    }

}
