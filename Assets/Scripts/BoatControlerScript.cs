using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(Float))]
public class BoatControlerScript : MonoBehaviour{
        public Vector3 COM;
        Transform m_COM;
   void Update()
    {
        Balance();     
    }

   void Balance() {
       if (!m_COM) {
           m_COM = new GameObject("COM").transform;
           m_COM.SetParent(transform);
       }
       m_COM.position = COM * transform.position.magnitude;
       GetComponent<Rigidbody>().centerOfMass = m_COM.position;
   }
}
