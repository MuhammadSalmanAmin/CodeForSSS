using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts
{
    public class CalculateWaterplaneArea : MonoBehaviour
    {
        void Start()
        {
            try
            {
                float scale = 0f;
                GameObject[] result = gameObject.AddComponent<RuntimeShatterExample>().SlicedVerticalShipHull(scale);
 
            }
            catch (Exception ex)
            {
                Debug.Log("Exception : " + ex.ToString());
            }
        }
    }

}