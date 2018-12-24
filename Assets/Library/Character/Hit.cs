using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	//	2Dと3Dの変更を考えて別クラスで管理
	public class Hit
	{
		Vector2 size;
		Vector2 offset;

		public void setSize(Vector2 size)
		{
			this.size = size;
		}

		public Vector2 getSize()
		{
			return size;
		}

		public void setOffset(Vector2 offset)
		{
			this.offset = offset;
		}

		public Vector2 getOffset()
		{
			return offset;
		}

		public bool collides(Vector2 pos, Vector2 targetPos, Hit targetHit)
		{
			var lt = pos + offset;
			var rb = lt + size;

			var targetLt = targetPos + targetHit.getOffset();
			var targetRb = targetLt + targetHit.getSize();

			var rect = size + targetHit.getSize();
			var areaLt = Vector2.zero;
			var areaRb = Vector2.zero;

			areaLt.x = (lt.x < targetLt.x) ? lt.x : targetLt.x;
			areaLt.y = (lt.y < targetLt.y) ? lt.y : targetLt.y;
			areaRb.x = (rb.x > targetRb.x) ? rb.x : targetRb.x;
			areaRb.y = (rb.y > targetRb.y) ? rb.y : targetRb.y;

			if((areaRb.x - areaLt.x < rect.x) && (areaRb.y - areaLt.y < rect.y))
			{
				return true;
			}

			return false;
		}
	}
}
