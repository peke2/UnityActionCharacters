using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public interface ICore
	{
		Hit m_hit { get; set; }
		Vector2 m_position { get; set; }

		bool isRemoved {get; set;}

		void init();
		void update();
		void updateGraph();
		void onRemoved();
		void onHit(ICore target);
	}
}