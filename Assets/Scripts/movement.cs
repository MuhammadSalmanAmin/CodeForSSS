using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
        private GameObject cube_one;
        void Update () {
                if (Input.GetKeyDown (KeyCode.LeftArrow)) {
                        Vector3 position = this.transform.position;
                        position.x--;
                        this.transform.position = position;
                }
                if (Input.GetKeyDown (KeyCode.RightArrow)) {
                        Vector3 position = this.transform.position;
                        position.x++;
                        this.transform.position = position;
                }
                if (Input.GetKeyDown (KeyCode.UpArrow)) {
                        Vector3 position = this.transform.position;
                        position.z++;
                        this.transform.position = position;
                }
                if (Input.GetKeyDown (KeyCode.DownArrow)) {
                        Vector3 position = this.transform.position;
                        position.z--;
                        this.transform.position = position;
                }

        }
}