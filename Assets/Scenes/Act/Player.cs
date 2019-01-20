using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : CharacterBase {

	GameObject m_chobj;

	System.Action m_stateProc;

	const float JUMP_PITCH = 4;
	const int JUMP_COUNT = 8;

	// Use this for initialization
	void Start () {
		var ctrl = Character.ControlObject.getControl();
		ctrl.addObject(this);

		var obj = Resources.Load<GameObject>("Player");
		m_chobj = GameObject.Instantiate<GameObject>(obj);
		m_chobj.name = "PlayerObject";
		m_chobj.transform.SetParent(gameObject.transform, false);
		position = new Vector2(24,128);

		hit = new Character.Hit();
		hit.setSize(new Vector2(16,16));
		hit.setOffset(new Vector2(-8,8));

		type = CharacterBase.Type.Player;
	}


	int m_jumpCount = 0;
	float m_jump = 0;
	public override void update()
	{
		var pos = position;
		float rate = 1.0f;

		float fallMove = -4;
		Vector2 backVector = Vector2.zero;

		//	ジャンプ中は足元の地面判定はスルー
		//	ジャンプ中にめり込み判定に引っかかって引き戻されるため
		if(m_jump > 0 || !isGround(position, new Vector2(0, fallMove), ref backVector))
		{
			if(m_jump == 0)
			{
				pos.y += fallMove;
			}
			rate = 0.4f;
		}
		else
		{
			pos.y += fallMove + backVector.y;

			if(Input.GetButtonDown("Jump") && m_jump == 0)
			{
				m_jump = JUMP_PITCH;
				m_jumpCount = JUMP_COUNT;
			}
		}

		float h = Input.GetAxis("Horizontal");
		float move = 0;
		int dir = 0;

		if(h >= 0.2f)
		{
			move = 2*rate;
			dir = 1;
		}
		else if(h <= -0.2f)
		{
			move = -2*rate;
			dir = -1;
		}

		move += moveDirection.x;
		pos += moveDirection;
		moveDirection = Vector2.zero;   //	外部要因を反映済みなのでクリア
		isMoved = false;

		if(dir!=0)
		{
			if(!isWall(pos, new Vector2(move, 0), ref backVector))
			{
				pos.x += move;
			}
			else
			{
				pos.x += move + backVector.x;
			}
		}


		if(m_jumpCount > 0)
		{
			m_jumpCount--;
		}
		else
		{
			m_jump = m_jump / 2;
			if(m_jump < 0.1f)
			{
				m_jump = 0;
			}
		}

		backVector = Vector2.zero;
		if(isCeiling(pos, new Vector2(0, m_jump), ref backVector))
		{
			pos.y += m_jump + backVector.y;
			m_jump = 0;
			m_jumpCount = 0;
		}
		else
		{
			pos.y += m_jump;
		}

		position = pos;
	}

	public override void updateGraph()
	{
		m_chobj.transform.position = position;
	}

	public override void onHit(Character.ICore target)
	{

	}

	public Vector2 getSize()
	{
		return new Vector2(16,16);
	}

	bool isMoved;
	Vector2 moveDirection;
	public void move(Vector2 dir)
	{
		isMoved = true;
		moveDirection = dir;
	}

	//	天井判定
	bool isCeiling(Vector2 pos, Vector2 offset, ref Vector2 backVector)
	{
		return isWall(pos, offset, ref backVector);
	}

	//	地面判定
	bool isGround(Vector2 pos, Vector2 offset, ref Vector2 backVector)
	{
		//	外部の要因で移動した場合、地面はあるものとする
		if(isMoved)
		{
			//	既に押し戻しがあったものとして打ち消す
			backVector -= offset;
			return true;
		}

		var objects = GameObject.FindObjectsOfType<Lift>();
		foreach(var obj in objects)
		{
			Vector2 backVec = Vector2.zero;
			if(obj.isOn(getSize(), position+offset, ref backVec))
			{
				backVector = backVec;
				return true;
			}
		}

		return isWall(pos, offset, ref backVector);
	}

	/*
	Vector2 calcSnapVectorFloor(float x, float y, float snapx, float snapy)
	{
		var vec = new Vector2(Mathf.Floor(x/snapx)*snapx, Mathf.Floor(y/snapy)*snapy);
		return vec;
	}

	Vector2 calcSnapVectorCeil(float x, float y, float snapx, float snapy)
	{
		var vec = new Vector2(Mathf.Ceil(x / snapx) * snapx, Mathf.Ceil(y / snapy) * snapy);
		return vec;
	}*/

	bool isWall(Vector2 pos, Vector2 offset, ref Vector2 backVector)
	{
		backVector = Vector2.zero;

		var obj = GameObject.Find("Stage");
		if(obj == null)
		{
			return false;
		}

		var stageObj = obj.GetComponent<Stage.StageObject>();

		if(stageObj == null)
		{
			return false;
		}

		//	座標とサイズから左上と右下の座標を取得
		//	切り捨てでブロックサイズ(0.5*0.5)にマッピング
		var size = new Vector2(16, 16);
		var blockSize = new Vector2(8, 8);
		//var lt = calcSnapVectorFloor(pos.x - size.x / 2, pos.y + size.y / 2, 0.5f, 0.5f);
		//var rb = calcSnapVectorCeil(pos.x + size.x / 2, pos.y - size.y / 2, 0.5f, 0.5f);
		var lx = pos.x + offset.x - size.x / 2;
		var ly = pos.y + offset.y + size.y / 2;
		var rx = pos.x + offset.x + size.x / 2;
		var ry = pos.y + offset.y - size.y / 2;
		var snapx = 8;
		var snapy = 8;
		var lt = new Vector2(Mathf.Floor(lx / snapx) * snapx, Mathf.Ceil(ly / snapy) * snapy);
		var rb = new Vector2(Mathf.Ceil(rx / snapx) * snapx, Mathf.Floor(ry / snapy) * snapy);

		//	判定の範囲から判定対象のブロックを求める
		int x0 = (int)(lt.x / snapx);
		int x1 = (int)(rb.x / snapx);
		int y0 = (int)(rb.y / snapy);
		int y1 = (int)(lt.y / snapy);

		for(var y = y0; y < y1; y++)
		{
			for(var x = x0; x < x1; x++)
			{
				var chip = stageObj.stage.getChip(x, y);

				if(chip == 1)
				{
					var block = new Vector2(x * 8, y* 8) + blockSize / 2f;
					var dist = block - (pos + offset);
					var len = (blockSize + size) / 2;

					dist = new Vector2(Mathf.Abs(dist.x), Mathf.Abs(dist.y));

					var back = len - dist;

					backVector = back * (-offset.normalized);

					return true;
				}
			}
		}

		return false;
	}


	void drawRectLine(float x, float y)
	{
		var center = new Vector2(x, y);
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
			Debug.DrawLine(vec[i], vec[i + 1], Color.red);
		}
	}
}
