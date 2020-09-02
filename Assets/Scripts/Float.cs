
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class Float : MonoBehaviour
{

    public MeshVolume meshVolumeObject;
    public float waterLevel = -3.5f;
    public float floatThreshold = 2.0f;
    public float waterDensity = 0.125f;
    public float downForce = 0.0f;
    float forceFactor;
    float buoyancyForce;
    float forceOfGravity;
    float mBodySubmergedVolume;

    float mBodyDensity = 0.785f;
    Vector3 floatForce;

    Vector3 buoyantForce;

    public Text txtDisplayVolume;
    void Start()
    {
        //meshVolumeObject = FindObjectOfType(typeof(MeshVolume)) as MeshVolume;
        SliceHull(10f);

        //DisplayVolume();
    }

    void FixedUpdate()
    {
        forceFactor = 1.0f - ((transform.position.y - waterLevel) / floatThreshold);
        if (forceFactor > 0.0f)
        {
            floatForce = -Physics.gravity * GetComponent<Rigidbody>().mass * (forceFactor - GetComponent<Rigidbody>().velocity.y * waterDensity);
            floatForce += new Vector3(0.0f, -downForce * GetComponent<Rigidbody>().mass, 0.0f);
            GetComponent<Rigidbody>().AddForceAtPosition(floatForce, transform.position);
        }

    }

    void ChangeDraught(float waterLevel)
    {
        this.waterLevel = waterLevel;
        FixedUpdate();

    }

    public GameObject[] SliceHull(float draught, bool isHorizontal = true)
    {
        GameObject[] result;
        if (isHorizontal)
        {
            result = gameObject.AddComponent<RuntimeShatterExample>().SlicedShipHullHorizontal(draught);
        }
        else
        {
            result = gameObject.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(draught);
        }

        Debug.Log("Submerged Volume : " + result[1].AddComponent<SliceMeshVol>().CalculateSlicedMeshVolume().ToString());
        return result;
    }


    public GameObject[] SliceHullVertically(float draught)
    {
        GameObject[] result;

        result = gameObject.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(draught);

        //Debug.Log("Submerged Volume : " + result[1].AddComponent<SliceMeshVol>().CalculateSlicedMeshVolume().ToString());
        return result;
    }

    public void DisplayVolume()
    {
        var result = meshVolumeObject.CalculateMeshVolume();
        string msg = "In Float file :" + result + " Vol mesh is  cube units.";
        Debug.Log(msg);

        mBodySubmergedVolume = result * (mBodyDensity / waterDensity);
        float mMass = result * mBodyDensity;
        float rigidbodymass = GetComponent<Rigidbody>().mass;

        Debug.Log("In Float file Rigid body mass is :" + rigidbodymass);
        //forceOfGravity = (GetComponent<Rigidbody>().mass) * (-Physics.gravity.y);
        // buoyancyForce = (mBodySubmergedVolume) * (-Physics.gravity.y);

        //  float buoyancy = forceOfGravity - buoyancyForce;
        //  buoyantForce = Vector3.up * buoyancy;
        Debug.Log("In Float file Hull mass is :" + mMass);

    }



}
