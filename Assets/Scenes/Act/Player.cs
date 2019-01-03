using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase {

	GameObject chobj;

	// Use this for initialization
	void Start () {
		var ctrl = Character.ControlObject.getControl();
		ctrl.addObject(this);

		var obj = Resources.Load<GameObject>("Player");
		chobj = GameObject.Instantiate<GameObject>(obj);
		chobj.name = "PlayerObject";
		chobj.transform.SetParent(gameObject.transform, false);

		hit = new Character.Hit();
		hit.setSize(new Vector2(1f,1f));
		hit.setOffset(new Vector2(-0.5f,0.5f));

		type = CharacterBase.Type.Player;
	}

	// Update is called once per frame
	void Update () {
		
	}

	int counter = 0;
	float angleRad = 0;
	public override void update()
	{
		angleRad += 3 * Mathf.Deg2Rad;
		float r = 2;

		var x = Mathf.Cos(angleRad) * r;
		var y = Mathf.Sin(angleRad) * r;

		position = new Vector2(x, y);

		if(++counter > 300)
		{
			isRemoved = true;
		}
	}

	public override void updateGraph()
	{
		chobj.transform.position = position;
	}

	public override void onHit(Character.ICore target)
	{

	}
}
