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

		Lift liftObj;
		obj = new GameObject("Lift");
		liftObj = obj.AddComponent<Lift>();
		obj.transform.SetParent(gameObject.transform, false);
		liftObj.startPos = new Vector2(30 * 8, 10 * 8);
		liftObj.endPos = new Vector2(41 * 8, 18 * 8);
		//liftObj.endPos = new Vector2(30 * 8, 10 * 8);
		liftObj.position = liftObj.startPos;

		obj = new GameObject("Lift2");
		liftObj = obj.AddComponent<Lift>();
		obj.transform.SetParent(gameObject.transform, false);
		liftObj.startPos = new Vector2(48 * 8, 1 * 8);
		liftObj.endPos = new Vector2(48 * 8, 20 * 8);
		liftObj.position = liftObj.startPos;
		liftObj.speed = 1 / 152.0f * 1.5f;


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
