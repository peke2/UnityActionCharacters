using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActGravity : ActBase {

	Vector3 m_accel = new Vector3(0,-1,0);
	float m_speed = 0;

	public ActGravity()
	{

	}

	public override void update(ActParam param)
	{
		if (param.m_isGround)
		{
			return;
		}

		param.m_velocity += m_accel * m_speed;

		m_speed += 0.1f;	
		if( m_speed > 4.0f)
		{
			m_speed = 4.0f;
		}
	}

}
