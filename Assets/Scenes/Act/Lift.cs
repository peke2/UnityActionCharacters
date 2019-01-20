using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : CharacterObjectBase
{

	GameObject m_chobj;

	Vector2 size = new Vector2(48, 8);

	public float m_speed = 1/152.0f;
	public Vector2 m_moveDirection = new Vector2(0,1);
	public Vector2 m_startPos = new Vector2(0,8);
	public Vector2 m_endPos = new Vector2(0,160);

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
		m_chobj = GameObject.Instantiate<GameObject>(obj);
		m_chobj.name = "LiftObject";
		m_chobj.transform.SetParent(gameObject.transform, false);
		//position = new Vector2(43 * 8, 1 * 8);

	}

	Player player;
	int direction = 1;
	float parameter = 0;
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

		parameter += m_speed * direction;

		if(parameter > 1.0f)
		{
			direction = -1;
			parameter = 1.0f;
		}
		else if( parameter < 0.0f )
		{
			direction = 1;
			parameter = 0.0f;
		}

		var pos = (m_endPos - m_startPos) * parameter + m_startPos;
		var move = pos - position;

		Vector2 backVector = Vector2.zero;
		if(player!=null && isOn(position, player.getSize(), player.position, ref backVector))
		{
			//	押し戻し＋追従
			player.move(backVector + move);
		}

		//pos += move;

		position = pos;
	}

	public override void updateGraph()
	{
		transform.position = position;
	}


	public bool isOn(Vector2 playerSize, Vector2 playerPos, ref Vector2 backVector)
	{
		return isOn(position, playerSize, playerPos, ref backVector);
	}

	private bool isOn(Vector2 pos, Vector2 playerSize, Vector2 playerPos, ref Vector2 backVector)
	{
		backVector = Vector2.zero;

		//var playerSize = player.getSize();
		//var playerPos = player.position;

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
