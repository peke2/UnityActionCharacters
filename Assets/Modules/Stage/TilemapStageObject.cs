using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

namespace Stage
{
	public class TilemapStageObject : MonoBehaviour, IStage
	{
		//public IStage stage { get; set; }

		//GameObject m_wall;
		Tilemap m_tileMap;

		public float getWidth()
		{
			return m_tileMap.size.x;
		}

		public float getHeight()
		{
			return m_tileMap.size.y;
		}

		public float getChipWidth()
		{
			return m_tileMap.cellSize.x;
		}

		public float getChipHeight()
		{
			return m_tileMap.cellSize.y;
		}

		public int getChip(int x, int y)
		{
			var tile = m_tileMap.GetTile(new Vector3Int(x, y, 0));
			if(tile == null) return -1;
			return 1;
		}

		// Use this for initialization
		void Start()
		{
			var tileObj = GameObject.Find("Tilemap");
			m_tileMap = tileObj.GetComponent<Tilemap>();

			/*m_wall = Resources.Load<GameObject>("Wall");
			float size = 0.5f;
			for(int y = 0; y<getHeight(); y++)
			{
				for(int x = 0; x < getWidth(); x++)
				{
					if(getChip(x, y) != 1) continue;
					var obj = GameObject.Instantiate<GameObject>(m_wall);
					obj.transform.SetParent(gameObject.transform, false);
					obj.transform.position = new Vector3(x * size, y* size, 0);
				}
			}*/
		}

		// Update is called once per frame
		void Update()
		{
		/*	var center = stage.getCenter();
			var cameraObj = GameObject.Find("Main Camera");
			if(cameraObj)
			{
				var camera = cameraObj.GetComponent<Camera>();
				var pos = camera.transform.position;
				camera.transform.position = new Vector3(center.x, center.y, pos.z);
			}

			drawMarks();*/
		}


		List<Vector2> marks = new List<Vector2>();

		public void markChip(int x, int y)
		{
			marks.Add(new Vector2(x * 0.5f, y * 0.5f));
		}

		public void drawMarks()
		{
			foreach(var pos in marks)
			{
				drawRectLine(pos);
			}

			marks.Clear();
		}

		void drawRectLine(Vector2 center)
		{
			var lt = center + new Vector2(-0.25f, 0.25f);
			var rb = center + new Vector2(0.25f, -0.25f);
			Vector3[] vec = new Vector3[]{
			new Vector3(lt.x, lt.y, 0),
			new Vector3(rb.x, lt.y, 0),
			new Vector3(rb.x, rb.y, 0),
			new Vector3(lt.x, rb.y, 0),
			new Vector3(lt.x, lt.y, 0),
		};
			for(int i = 0; i < 4; i++)
			{
				Debug.DrawLine(vec[i], vec[i + 1], Color.white);
			}
		}

	}
}
