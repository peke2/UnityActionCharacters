﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
	public class Stage
	{
		protected int[][] chipInfo;
		public int width {get; set;}
		public int height {get; set;}

		protected float chipWidth = 0.5f;
		protected float chipHeight = 0.5f;

		public Stage()
		{
		}

		public void initChips(int w, int h, int[][] chips)
		{
			width = w;
			height = h;
			chipInfo = chips;
		}

		public int getChip(int x, int y)
		{
			if(chipInfo == null) return -1;
			//	パターンの配置は配列の順番で上から下に並ぶので、Y座標指定を反転
			return chipInfo[height-y-1][x];
		}

		public Vector2 getCenter()
		{
			return new Vector2(width * chipWidth/2, height * chipHeight/2);
		}
	}
}