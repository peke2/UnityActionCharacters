using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActMove : ActBase {

	Vector3 m_dir = new Vector3(1,0,0);
	float m_accel = 0;
	float m_speed = 0.1f;
	const float MAX_ACCEL = 4.0f;

	public ActMove()
	{

	}

	public override void update(ActParam param)
	{
		float h = Input.GetAxisRaw("Horizontal");

		if( Mathf.Abs(h) < 0.2f)
		{
			m_accel /= 1.2f;
			return;
		}

		m_accel += m_speed * h;
		m_accel = Mathf.Clamp(m_accel, -MAX_ACCEL, MAX_ACCEL);

		param.m_velocity += m_dir * m_accel;
	}

}
