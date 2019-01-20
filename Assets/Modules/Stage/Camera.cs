using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
	public interface ICamera
	{
		void initCameraInfo(float x, float y, float w, float h, float areaW, float areaH);
		void setPosition(float x, float y);
		void update();
	}
}