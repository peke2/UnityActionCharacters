using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Character
{
	public class BaseControl
	{
		List<ICore> characterList;

		public BaseControl()
		{
			characterList = new List<ICore>();
		}


		public void update()
		{
			foreach(var obj in characterList)
			{
				obj.update();
			}

			collide();
			updateGraph();
			remove();
		}

		void updateGraph()
		{
			foreach(var obj in characterList)
			{
				obj.updateGraph();
			}
		}


		void remove()
		{
			//	対象をリストから除去
			characterList.RemoveAll( o=> {
				if(!o.isRemoved) return false;
				o.onRemoved();
				return true;
			});
		}


		public void addObject(ICore cobj)
		{
			if(characterList.Exists(o=>o == cobj))
			{
				return;
			}
			characterList.Add(cobj);
			cobj.init();
		}


		public void collide()
		{
			var list = new List<Tuple<ICore, ICore>>();

			var nums = characterList.Count;
			for(var i=0; i<nums-1; i++)
			{
				var obj = characterList[i];
				var objHit = obj.hit;

				if(objHit == null) continue;
				for(var j=i+1; j<nums; j++)
				{
					var target = characterList[j];
					var targetHit = target.hit;

					if(targetHit == null) continue;

					if(objHit.collides(obj.position, target.position, targetHit))
					{
						list.Add( new Tuple<ICore, ICore>(obj, target));
					}
				}
			}

			//	対象に対してイベントを投げる
			//	判定中に随時処理を行わず、すべての判定が終了したらまとめて対応
			foreach(var t in list)
			{
				//	お互いにイベントを送る
				(t.Item1).onHit(t.Item2);
				(t.Item2).onHit(t.Item1);
			}
		}

	}
}