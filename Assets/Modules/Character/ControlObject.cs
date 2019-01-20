using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public class ControlObject : MonoBehaviour
	{
		BaseControl m_baseControl;
		static ControlObject m_self;

		public static BaseControl getControl()
		{
			return m_self.m_baseControl;
		}

		private void Awake()
		{
			m_baseControl = new BaseControl();
			m_self = this;
		}

		// Use this for initialization
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{
			m_baseControl.update();
		}
	}
}
