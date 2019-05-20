using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

	ActParam m_actParam = new ActParam();
	List<ActBase> m_actions = new List<ActBase>();

	// Use this for initialization
	void Start () {
		m_actParam.m_position = transform.position;
		m_actParam.m_velocity = Vector3.zero;

		m_actions.Add(new ActBurner());
		m_actions.Add(new ActMove());
		m_actions.Add( new ActGravity() );
	}
	
	// Update is called once per frame
	void Update () {
		m_actParam.m_velocity = Vector3.zero;
		//	燃料の回復
		m_actParam.m_fuel += 4;
		if (m_actParam.m_fuel > 1000)
		{
			m_actParam.m_fuel = 1000;
		}

		foreach (var act in m_actions)
		{
			act.update(m_actParam);
		}

		m_actParam.m_position += m_actParam.m_velocity;

		updateGroundStatus();

		transform.position = m_actParam.m_position;
	}

	void updateGroundStatus()
	{
		bool isGround = false;
		float offset = 8;
		float footY = m_actParam.m_position.y - offset;
		if(footY <= 0)
		{
			m_actParam.m_position.y = offset;
			isGround = true;
		}

		m_actParam.m_isGround = isGround;
	}

}
