using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActMove : ActBase {

	Vector3 m_accel = new Vector3(1,0,0);
	float m_speed = 4;

	public ActMove()
	{

	}

	public override void update(ActParam param)
	{
		float h = Input.GetAxisRaw("Horizontal");

		if( Mathf.Abs(h) < 0.2f)
		{
			return;
		}

		param.m_velocity += m_accel * h * m_speed;
	}

}
