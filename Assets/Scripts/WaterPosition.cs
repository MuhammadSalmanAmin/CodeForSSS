using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPosition : MonoBehaviour
{
    // Update is called once per frame
    Vector3 position;
    Vector3 direction;
    void FixedUpdate()
    {
        position = GameObject.FindGameObjectWithTag("WaterProDaytime").transform.position;
        Debug.Log("Water Position is : " +  position);

        direction = GameObject.FindGameObjectWithTag("WaterProDaytime").transform.forward;
        Debug.Log("Water Direction is : " +  direction);
    }


    public List<Vector3> GetWaterPositionAndDirection(){

         List<Vector3> result = new List<Vector3>();
        position = GameObject.FindGameObjectWithTag("WaterProDaytime").transform.position;
        Debug.Log("Water Position is : " +  position);


        direction = GameObject.FindGameObjectWithTag("WaterProDaytime").transform.forward;
        Debug.Log("Water Direction is : " +  direction);

        result.Add(position);
        result.Add(direction);

        return result;
    }
}
