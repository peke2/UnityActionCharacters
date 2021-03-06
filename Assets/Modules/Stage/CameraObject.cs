﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
	public class CameraObject : MonoBehaviour, ICamera
	{
		Vector2 m_position = Vector2.zero;
		Vector2 m_size = Vector2.zero;
		Vector2 m_scopeSize = Vector2.zero;
		Vector2 m_stageSize = Vector2.zero;

		public void initCameraInfo(float x, float y, float w, float h, float areaW, float areaH)
		{
			m_size = new Vector2(w, h);
			m_scopeSize = m_size * 0.7f;
			m_stageSize = new Vector2(areaW, areaH);
			m_position = limit(new Vector2(x, y));
		}

		public void setPosition(float x, float y)
		{
			m_position = limit(new Vector2(x, y));
		}

		public void update()
		{
			var obj = GameObject.Find("PlayerObject");
			if(obj == null)
			{
				return;
			}

			var playerPos = obj.transform.position;

			var cameraObj = GameObject.Find("Main Camera");
			if(cameraObj == null)
			{
				return;
			}
			var cameraPos = cameraObj.transform.position;

			var diff = playerPos - cameraPos;
			var halfScope = m_scopeSize / 2;
			var adjust = Vector2.zero;

			if( diff.x < -halfScope.x )
			{
				adjust.x = diff.x + halfScope.x;
			}
			else if( diff.x > halfScope.x )
			{
				adjust.x = diff.x - halfScope.x;
			}

			if( diff.y < -halfScope.y )
			{
				adjust.y = diff.y + halfScope.y;
			}
			else if( diff.y > halfScope.y )
			{
				adjust.y = diff.y - halfScope.y;
			}

			setPosition(m_position.x + adjust.x, m_position.y + adjust.y);
			cameraPos.x = m_position.x;
			cameraPos.y = m_position.y;
			cameraObj.transform.position = cameraPos;
		}

		Vector2 limit(Vector2 pos)
		{
			//	左下が原点(0,0)の座標系
			var halfSize = m_size / 2;
			var lb = pos - halfSize;
			var rt = pos + halfSize;

			var areaLb = new Vector2(0, 0);
			var areaRt = m_stageSize;

			float diff;
			diff = lb.x - areaLb.x;
			if(diff < 0)
			{
				pos.x -= diff;
			}
			else
			{
				diff = rt.x - areaRt.x;
				if(diff > 0)
				{
					pos.x -= diff;
				}
			}

			diff = lb.y - areaLb.y;
			if(diff < 0)
			{
				pos.y -= diff;
			}
			else
			{
				diff = rt.y - areaRt.y;
				if(diff > 0)
				{
					pos.y -= diff;
				}
			}

			return pos;
		}

		// Use this for initialization
		void Start()
		{
			initCameraInfo(0, 0, 256, 160, 480, 256);
		}

		// Update is called once per frame
		void Update()
		{
			update();
		}
	}
}