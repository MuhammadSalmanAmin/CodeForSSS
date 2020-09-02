using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List : MonoBehaviour {
	public Vector3 degrees;

	void FixedUpdate () {

		Quaternion rotationZ = Quaternion.AngleAxis (degrees.z, new Vector3 (0f, 0f, 1f));
		this.transform.rotation = rotationZ;
	}

	void ChangeTrim (Vector3 degrees) {
		this.degrees = degrees;
		FixedUpdate ();
	}
}