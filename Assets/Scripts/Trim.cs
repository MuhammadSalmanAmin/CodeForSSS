using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trim : MonoBehaviour {
	public Vector3 degrees;
	void FixedUpdate () {

		Quaternion rotationX = Quaternion.AngleAxis (degrees.x, new Vector3 (1f, 0f, 0f));
		this.transform.rotation = rotationX;
	}

	void ChangeTrim (Vector3 degrees) {
		this.degrees = degrees;
		FixedUpdate ();
	}
}