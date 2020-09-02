using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed = 3.5f;
    public float scroll;
    private float X;
    private float Y;
    float minFov = 10f;
    float maxFov = 90f;
    float sensitivity = 10f;


    // Vector3 getPosition;
    // Vector3 minPosition = new Vector3(0.0f, 0.0f, 29.0f);
    // Vector3 maxPosition = new Vector3(0.0f, 0.0f, 31.0f);
    SetCursor cursor;
    bool handling;
    // Start is called before the first frame update
    void Start()
    {
    }
    void Update()
    {
          if (Input.GetMouseButton(0))
        {
           
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * speed, Input.GetAxis("Mouse X") * speed, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float fov = Camera.main.fieldOfView;
            fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;
        }
    }

    // private bool IsGreaterOrEqual(Vector3 getPosition, Vector3 minPosition)
    // {
    //     if (getPosition.x >= minPosition.x && getPosition.y >= minPosition.y && getPosition.z >= minPosition.z)
    //     {
    //         transform.localPosition -= Vector3.right * Input.GetAxis("Mouse ScrollWheel") * scroll;
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }

    // private bool IsLesserOrEqual(Vector3 getPosition, Vector3 minPosition)
    // {
    //     if (getPosition.x <= minPosition.x && getPosition.y <= minPosition.y && getPosition.z <= minPosition.z)
    //     {
    //         transform.localPosition += Vector3.right * Input.GetAxis("Mouse ScrollWheel") * scroll;
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }
}