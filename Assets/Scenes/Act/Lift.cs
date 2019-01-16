using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : CharacterBase
{

	GameObject chobj;

	Vector2 size = new Vector2(48, 8);

	public float speed = 1;

	// Use this for initialization
	void Start()
	{
		var ctrl = Character.ControlObject.getControl();
		ctrl.addObject(this, 0);	//	プレイヤーよりも先に処理するためレイヤー0を指定

			//[todo] 処理の順番の正常化を検討する
			//リフトからプレイヤーを操作するためレイヤーで処理優先を上げているが、
			//プレイヤーの落下処理の後、次のフレームでリフトからの押上げ処理が入るため一瞬めり込む
			//プレイヤー側でも「地面」として処理する必要があるのではないか？

		var obj = Resources.Load<GameObject>("Lift");
		chobj = GameObject.Instantiate<GameObject>(obj);
		chobj.name = "LiftObject";
		chobj.transform.SetParent(gameObject.transform, false);
		//position = new Vector2(43 * 8, 1 * 8);

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
		if(direction == 1 && pos.y >= 160)
		{
			direction = -1;
		}
		else if(direction == -1 && pos.y <= 8)
		{
			direction = 1;
		}

		var move = new Vector2(0, speed * direction);

		Vector2 backVector = Vector2.zero;
		if(player!=null && isOn(pos, ref backVector))
		{
			//	押し戻し＋追従
			player.move(backVector + move);
		}

		pos += move;

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
