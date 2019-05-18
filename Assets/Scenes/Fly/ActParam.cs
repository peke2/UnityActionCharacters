using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActParam {

	public Vector3 m_position;
	public Vector3 m_velocity;
	public bool m_isGround;

	public float m_fuel;

	public float getFuel(float amount)
	{
		if( m_fuel < amount)
		{
			amount = m_fuel;
		}

		m_fuel -= amount;
		return amount;
	}
}
