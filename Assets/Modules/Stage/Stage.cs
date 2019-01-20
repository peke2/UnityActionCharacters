using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Stage
{
	public interface IStage
	{
		//protected int[][] m_chipInfo;
		//int width {get; set;}
		//int height {get; set;}

		//protected float chipWidth = 0.5f;
		//protected float chipHeight = 0.5f;


		//void initChips(int w, int h, int[][] chips);
		/*		{
					width = w;
					height = h;
					m_chipInfo = chips;
				}*/

		int getChip(int x, int y);
#if false
		{/*
			if(chipInfo == null) return -1;

			if((x < 0 || x >= width) || (y < 0 || y >= height))
			{
				return -1;
			}

			//	パターンの配置は配列の順番で上から下に並ぶので、Y座標指定を反転
			return chipInfo[height-y-1][x];*/
/*			var tile = m_tileMap.GetTile(new Vector3Int(x, y, 0));
			if(tile == null) return -1;
			return 1;
		}
#endif
		//Vector2 getCenter();
		/*		{
					return new Vector2(width * chipWidth/2, height * chipHeight/2);
				}*/

		float getWidth();
		float getHeight();

		float getChipWidth();
		float getChipHeight();
	}
}