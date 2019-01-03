using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public interface ICore
	{
		Hit hit { get; set; }
		Vector2 position { get; set; }
/*
		public delegate void Callback();

		public Callback removed { get; set; }

		public Core()
		{
			hit = new Hit();
		}

		public void setHit(Vector2 offset, Vector2 size)
		{
			hit.setOffset(offset);
			hit.setSize(size);
		}

		public Hit getHit()
		{
			return hit;
		}
*/

		bool isRemoved {get; set;}
		//public void remove()
		//{
		//	isRemoved = true;
		//}

		void init();
		void update();
		void updateGraph();
		void onRemoved();
		void onHit(ICore target);
	}
}