using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	//	2Dと3Dの変更を考えて別クラスで管理
	public class Hit
	{
		Vector2 m_size;
		Vector2 m_offset;

		public void setSize(Vector2 size)
		{
			this.m_size = size;
		}

		public Vector2 getSize()
		{
			return m_size;
		}

		public void setOffset(Vector2 offset)
		{
			this.m_offset = offset;
		}

		public Vector2 getOffset()
		{
			return m_offset;
		}

		public bool collides(Vector2 pos, Vector2 targetPos, Hit targetHit)
		{
			var rev = new Vector2(1f,-1f);
			var lt = pos + m_offset;
			var rb = lt + m_size * rev;

			var targetLt = targetPos + targetHit.getOffset();
			var targetRb = targetLt + targetHit.getSize() * rev;

			var rect = m_size + targetHit.getSize();
			var areaLt = Vector2.zero;
			var areaRb = Vector2.zero;

			areaLt.x = (lt.x < targetLt.x) ? lt.x : targetLt.x;
			areaLt.y = (lt.y > targetLt.y) ? lt.y : targetLt.y;
			areaRb.x = (rb.x > targetRb.x) ? rb.x : targetRb.x;
			areaRb.y = (rb.y < targetRb.y) ? rb.y : targetRb.y;

			if((areaRb.x - areaLt.x < rect.x) && (areaLt.y - areaRb.y < rect.y))
			{
				return true;
			}

			return false;
		}
	}
}
