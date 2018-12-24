using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var obj = new GameObject("Player");
		obj.AddComponent<Player>();
		obj.transform.SetParent(gameObject.transform, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
