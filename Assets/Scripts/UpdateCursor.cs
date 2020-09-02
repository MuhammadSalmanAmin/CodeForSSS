using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCursor : MonoBehaviour
{
	SetCursor cursor;

	bool handling;
    // Start is called before the first frame update
    void Start()
    {
		cursor = GameObject.FindGameObjectWithTag ("CameraRotator").GetComponent<SetCursor>();
    }

    // Update is called once per frame
    void Update()
    {
		if (handling) {
			cursor.setHand();
		}
    }

	void OnMouseDown(){
		handling = true;
	}
	void OnMouseUp(){
		handling = false;
		cursor.setMouse ();
	}
}
