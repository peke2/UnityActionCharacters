using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public interface ICore
	{
		Hit hit { get; set; }
		Vector2 position { get; set; }

		bool isRemoved {get; set;}

		void init();
		void update();
		void updateGraph();
		void onRemoved();
		void onHit(ICore target);
	}
}