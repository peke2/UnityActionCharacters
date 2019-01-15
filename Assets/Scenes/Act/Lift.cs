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
		ctrl.addObject(this, 0);	//	プレイヤーよりも先に処理するためレイヤー0を指定

		var obj = Resources.Load<GameObject>("Lift");
		chobj = GameObject.Instantiate<GameObject>(obj);
		chobj.name = "LiftObject";
		chobj.transform.SetParent(gameObject.transform, false);
		position = new Vector2(43 * 8, 1 * 8);

	}

	Player player;
	int direction = 1;
	public override void update()
	{
		var obj = GameObject.Find("Player");
		if(obj != null)
		{
			player = obj.GetComponent<Player>();
		}
		else
		{
			player = null;
		}

		var pos = position;
		if(direction == 1 && pos.y >= 192)
		{
			direction = -1;
		}
		else if(direction == -1 && pos.y <= 8)
		{
			direction = 1;
		}

		var move = new Vector2(0, 1 * direction);
		pos += move;

		Vector2 backVector = Vector2.zero;
		if(player!=null && isOn(pos, ref backVector))
		{
			//	押し戻し＋追従
			player.move(backVector + move);
		}

		position = pos;
	}

	public override void updateGraph()
	{
		transform.position = position;
	}

	private bool isOn(Vector2 pos, ref Vector2 backVector)
	{
		backVector = Vector2.zero;

		var playerSize = player.getSize();
		var playerPos = player.position;

		var lx = pos.x - size.x/2;
		var rx = pos.x + size.x / 2;
		if(playerPos.x<lx || playerPos.x>rx )
		{
			return false;
		}

		float diff = playerPos.y - pos.y;
		float halfLen = (playerSize.y + size.y)/2;
		if(diff < size.y/2 || diff > halfLen)
		{
			return false;
		}

		//	めり込んだ分を押し戻す
		backVector.y = halfLen - diff;

		return true;
	}
}
