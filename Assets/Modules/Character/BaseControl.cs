using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Character
{
	public class BaseControl
	{
		const int LIST_LAYER_MAX = 4;
		List<ICore>[] m_characterList;

		public BaseControl()
		{
			m_characterList = new List<ICore>[LIST_LAYER_MAX];
			for(var i = 0; i < LIST_LAYER_MAX; i++)
			{
				m_characterList[i] = new List<ICore>();
			}
		}


		public void update()
		{
			foreach(var list in m_characterList)
			{
				foreach(var obj in list)
				{
					obj.update();
				}
			}

			collide();
			updateGraph();
			remove();
		}

		void updateGraph()
		{
			foreach(var list in m_characterList)
			{
				foreach(var obj in list)
				{
					obj.updateGraph();
				}
			}
		}


		void remove()
		{
			//	対象をリストから除去
			foreach(var list in m_characterList)
			{
				list.RemoveAll(o => {
					if(!o.isRemoved)
						return false;
					o.onRemoved();
					return true;
				});
			}
		}


		public void addObject(ICore cobj, int layer=1)
		{
			if(layer < 0 || layer >= LIST_LAYER_MAX)
			{
				return;
			}

			var list = m_characterList[layer];
			if(list.Exists(o=>o == cobj))
			{
				return;
			}
			list.Add(cobj);
			cobj.init();
		}


		public void collide()
		{
			var hitList = new List<Tuple<ICore, ICore>>();

			foreach(var list in m_characterList)
			{
				//	判定は同じレイヤー同士でのみ行う
				var nums = hitList.Count;
				for(var i = 0; i < nums - 1; i++)
				{
					var obj = list[i];
					var objHit = obj.m_hit;

					if(objHit == null)
					{
						continue;
					}

					for(var j = i + 1; j < nums; j++)
					{
						var target = list[j];
						var targetHit = target.m_hit;

						if(targetHit == null)
							continue;

						if(objHit.collides(obj.m_position, target.m_position, targetHit))
						{
							hitList.Add(new Tuple<ICore, ICore>(obj, target));
						}
					}
				}
			}

			//	対象に対してイベントを投げる
			//	判定中に随時処理を行わず、すべての判定が終了したらまとめて対応
			foreach(var t in hitList)
			{
				//	お互いにイベントを送る
				(t.Item1).onHit(t.Item2);
				(t.Item2).onHit(t.Item1);
			}
		}

	}
}