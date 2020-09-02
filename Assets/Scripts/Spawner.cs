using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Transform spawnPos;
    public GameObject spawnee;

    // Update is called once per frame
    void Update () {
        // if (Input.GetMouseButton(1))
        // {
        //     Instantiate(spawnee, spawnPos.position, spawnPos.rotation);
        // }

        // if (Input.GetMouseButton(1))
        //  {
        //  Vector3 mousePos = Input.mousePosition;
        //  mousePos.z = 2.0f;       // we want 2m away from the camera position
        //  Vector3 objectPos = Camera.current.ScreenToWorldPoint(mousePos);
        //  Instantiate(spawnee, objectPos, Quaternion.identity);
        //  }

        if (Input.GetMouseButton (1)) { PutCoin (Input.mousePosition); }

    }

    public void PutCoin (Vector2 mousePosition) {
        RaycastHit hit = RayFromCamera (mousePosition, 1000.0f);
        GameObject.Instantiate (spawnee, hit.point, Quaternion.identity);
    }

    public RaycastHit RayFromCamera (Vector3 mousePosition, float rayLength) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay (mousePosition);
        Physics.Raycast (ray, out hit, rayLength);
        return hit;
    }
}