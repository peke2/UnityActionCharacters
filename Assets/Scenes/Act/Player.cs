using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character.CharacterObject {

	GameObject player;

	// Use this for initialization
	void Start () {
		var ctrl = Character.ControlObject.getControl();
		ctrl.addObject(this);

		var obj = Resources.Load<GameObject>("Player");
		player = GameObject.Instantiate<GameObject>(obj);
		player.name = "PlayerObject";
		player.transform.SetParent(gameObject.transform, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	int counter = 0;
	float angleRad = 0;
	public override void update()
	{
		angleRad += 10 * Mathf.Deg2Rad;
		float r = 2;

		var x = Mathf.Cos(angleRad) * r;
		var y = Mathf.Sin(angleRad) * r;

		position = new Vector2(x, y);

		if(++counter > 120)
		{
			isRemoved = true;
		}
	}

	public override void updateGraph()
	{
		player.transform.position = position;
	}
}
