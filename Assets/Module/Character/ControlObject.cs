using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public class ControlObject : MonoBehaviour
	{
		BaseControl baseControl;
		static ControlObject self;

		public static BaseControl getControl()
		{
			return self.baseControl;
		}

		private void Awake()
		{
			baseControl = new BaseControl();
			self = this;
		}

		// Use this for initialization
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{
			baseControl.update();
		}
	}
}
