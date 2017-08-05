using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TagCreater : NetworkBehaviour {

	private int count=0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	[ServerCallback]
	void OnCollisionEnter(Collision other){
		if (count == 0) {
			other.gameObject.tag = "Ball1";
			count++;
		} else if (count == 1) {
			other.gameObject.tag = "Ball2";
		}

	}
}
