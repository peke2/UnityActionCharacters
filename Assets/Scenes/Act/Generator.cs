using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
		var obj = new GameObject("Player");
		obj.AddComponent<Player>();
		obj.transform.SetParent(gameObject.transform,false);

		//Enemy enemy;
		//for(int i = 0; i < 10; i++)
		//{
		//	obj = new GameObject("Enemy");
		//	enemy = obj.AddComponent<Enemy>();
		//	obj.transform.SetParent(gameObject.transform,false);
		//	enemy.position = new Vector2(Random.Range(-5,5),Random.Range(-5,5));
		//}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
