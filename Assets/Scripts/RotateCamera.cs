using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {
    // private float baseAngle = 0.0f;

    // void OnMouseDown(){
    //    Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
    //    pos = Input.mousePosition - pos;
    //    baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
    //    baseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) *Mathf.Rad2Deg;
    // }

    // void OnMouseDrag(){
    //     Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
    //     pos = Input.mousePosition - pos;
    //     float ang = Mathf.Atan2(pos.y, pos.x) *Mathf.Rad2Deg - baseAngle;
    //     transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
    // }

    float rotSpeed = 20;

    void OnMouseDrag () {
        float rotX = Input.GetAxis ("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis ("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX);
        transform.Rotate(Vector3.right, rotY);
    }
}