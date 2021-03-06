﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public class CharacterObject : MonoBehaviour, ICore
	{
		protected GameObject m_view;
		public Hit m_hit {get; set;}
		public Vector2 m_position {get; set;}

		public bool isRemoved {get; set;}

		// Use this for initialization
		void Start()
		{
			init();
		}

		// Update is called once per frame
		void Update()
		{

		}

		virtual public void init()
		{
		}

		virtual public void update()
		{
		}

		virtual public void updateGraph()
		{
		}

		virtual public void onRemoved()
		{
			GameObject.Destroy(gameObject);
		}

		virtual public void onHit(ICore target)
		{
		}
	}
}
