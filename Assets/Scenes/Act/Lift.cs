using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : CharacterBase
{

	GameObject chobj;

	Vector2 size = new Vector2(48, 8);

	// Use this for initialization
	void Start()
	{
		var ctrl = Character.ControlObject.getControl();
		ctrl.addObject(this);

		var obj = Resources.Load<GameObject>("Lift");
		chobj = GameObject.Instantiate<GameObject>(obj);
		chobj.name = "LiftObject";
		chobj.transform.SetParent(gameObject.transform, false);
		position = new Vector2(43 * 8, 1 * 8);

	}


	int direction = 1;
	public override void update()
	{
		var pos = position;
		if(direction == 1 && pos.y >= 192)
		{
			direction = -1;
		}
		else if(direction == -1 && pos.y <= 8)
		{
			direction = 1;
		}

		pos.y += 1 * direction;

		position = pos;
	}

	public override void updateGraph()
	{
		transform.position = position;
	}

	public bool isOn(Vector2 pos)
	{
		var lx = position.x - size.x/2;
		var rx = position.x + size.x / 2;
		if(pos.x<lx || rx < pos.x)
		{
			return false;
		}

		if(pos.y - position.y != 12)
		{
			return false;
		}

		return true;
	}
}
