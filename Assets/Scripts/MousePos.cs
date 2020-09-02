using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePos : MonoBehaviour {
  public float speed;

  // Update is called once per frame
  void Update () {
    if (Input.GetMouseButtonDown (0)) {
      transform.Rotate (0, speed * Time.deltaTime, 0);
    }

    if (Input.GetMouseButtonDown (1)) {
      transform.Rotate (0, -(speed * Time.deltaTime), 0);
    }
  }
}