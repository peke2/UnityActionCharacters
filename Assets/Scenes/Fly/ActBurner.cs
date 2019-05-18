using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActBurner : ActBase {

	Vector3 m_accel = new Vector3(0,1,0);
	float m_speed = 8;

	public ActBurner()
	{

	}

	public override void update(ActParam param)
	{
		if (!Input.GetButton("Fire1"))
		{
			return;
		}

		float amount = 10;
		float fuel = param.getFuel(amount);

		float rate = fuel / amount;

		param.m_velocity += m_accel * m_speed * rate;
	}

}
