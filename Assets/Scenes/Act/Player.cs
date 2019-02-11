using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : CharacterObjectBase {

	GameObject m_chobj;
	GameObject m_stageObj;

	delegate void StateProc(ref Vector2 pos);
	StateProc m_stateProc;

	Animator m_animator;
	SpriteRenderer m_animSprite;

	int m_jumpCount = 0;
	float m_jump = 0;

	const float JUMP_PITCH = 4;
	const int JUMP_COUNT = 8;

	const float MOVE_INPUT_THRESHOLD = 0.2f;

	enum ANIM_STATE{
		WAIT,
		WALK,
		JUMP,
		FALL,
	}

	// Use this for initialization
	void Start () {
		var ctrl = Character.ControlObject.getControl();
		ctrl.addObject(this);

		var obj = Resources.Load<GameObject>("Player");
		m_chobj = GameObject.Instantiate<GameObject>(obj);
		m_chobj.name = "PlayerObject";
		m_chobj.transform.SetParent(gameObject.transform, false);
		m_position = new Vector2(24,128);

		m_hit = new Character.Hit();
		m_hit.setSize(new Vector2(16,16));
		m_hit.setOffset(new Vector2(-8,8));

		m_animator = m_chobj.GetComponent<Animator>();
		m_animSprite = m_chobj.GetComponent<SpriteRenderer>();

		type = CharacterObjectBase.Type.Player;

		m_stageObj = GameObject.Find("Stage");
		if(m_stageObj == null)
		{
			Debug.Log("ステージオブジェクトが無い");
		}
	}


	public override void update()
	{
		//updateProc();
		stateProc();
	}

	private void stateProc()
	{
		var pos = m_position;

		Vector2 back = Vector2.zero;

		if(m_stateProc != stateProcJump)
		{
			//	足元のみを見る
			if(!isGround(m_position, new Vector2(0, -1), ref back))
			{
				//m_stateProc = stateProcFall;
				changeState(stateProcFall, false, ref pos);
			}
			else
			{
				//m_stateProc = stateProcWait;
				changeState(stateProcWait, false, ref pos);
			}
		}

		if(m_stateProc != null)
		{
			m_stateProc(ref pos);
		}

		Vector2 move;
		int dir = 0;
		move = pos - m_position;
		//	キー入力による方向変更に対してのみ反転を切り替える
		m_animSprite.flipX = (move.x > 0)? true : ((move.x < 0)? false : m_animSprite.flipX);

		move.x += m_moveDirection.x;
		pos += m_moveDirection;
		m_moveDirection = Vector2.zero;   //	外部要因を反映済みなのでクリア
		m_isMoved = false;

		if(move.x != 0)
		{
			if(isWall(m_position, new Vector2(move.x, 0), ref back))
			{
				pos.x += back.x;
			}
		}
		m_position = pos;
	}


	private float getHorizontalInput()
	{
		float h = Input.GetAxis("Horizontal");
		if(Mathf.Abs(h) < MOVE_INPUT_THRESHOLD)
		{
			return 0;
		}

		return h;
	}

	private void moveHorizontal(ref Vector2 pos, float scale)
	{
		float h = getHorizontalInput();
		if(h == 0)
		{
			return;
		}

		//	アナログではなくON/OFFで判断するため、スケールをそのまま返す
		pos.x += ((h>0)?1:-1) * scale;
	}

	private void changeState(StateProc proc, bool isImmediate, ref Vector2 pos)
	{
		if(m_stateProc != proc)
		{
			m_stateProc = proc;
			if(m_stateProc == stateProcWalk)
			{
				m_animator.SetInteger("State", (int)ANIM_STATE.WALK);
			}
			else if(m_stateProc == stateProcWait)
			{
				m_animator.SetInteger("State", (int)ANIM_STATE.WAIT);
			}
			else if(m_stateProc == stateProcJump)
			{
				m_animator.SetInteger("State", (int)ANIM_STATE.JUMP);
				//とりあえず、拾ったSEを入れてみた
				var audio = m_chobj.GetComponent<AudioSource>();
				audio.PlayOneShot(audio.clip);
			}
			else if(m_stateProc == stateProcFall)
			{
				m_animator.SetInteger("State", (int)ANIM_STATE.FALL);
				//Debug.Log("Fall");
			}
		}

		if(isImmediate)
		{
			proc(ref pos);
		}
	}

	private void stateProcJump(ref Vector2 pos)
	{
		if(m_jump == 0)
		{
			m_jump = JUMP_PITCH;
			m_jumpCount = JUMP_COUNT;
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
				//m_stateProc = stateProcFall;
				changeState(stateProcFall, false, ref pos);
			}
		}

		var back = Vector2.zero;
		if(isCeiling(pos, new Vector2(0, m_jump), ref back))
		{
			pos.y += m_jump + back.y;
			m_jump = 0;
			m_jumpCount = 0;
			//m_stateProc = stateProcFall;
			changeState(stateProcFall, false, ref pos);
		}
		else
		{
			pos.y += m_jump;
		}

		//	ジャンプ中は真上だけを見ているので、左右移動は判定の後に行う
		moveHorizontal(ref pos, 1);
	}

	private void stateProcWalk(ref Vector2 pos)
	{
		if(Input.GetButtonDown("Jump"))
		{
			//	次のフレームでジャンプ開始すると、地面判定の引力から抜けられないので、次の処理へ即移る
			changeState(stateProcJump, true, ref pos);
			return;
		}
		moveHorizontal(ref pos, 2);
	}

	private void stateProcWait(ref Vector2 pos)
	{
		if(getHorizontalInput() != 0)
		{
			changeState(stateProcWalk, true, ref pos);
			return;
		}

		if(Input.GetButtonDown("Jump"))
		{
			changeState(stateProcJump, true, ref pos);
			return;
		}
	}

	private void stateProcFall(ref Vector2 pos)
	{
		Vector2 back = Vector2.zero;
		Vector2 move = new Vector2(0, -4);
		if(isGround(pos, move, ref back))
		{
			move += back;
		}
		pos += move;

		//	落下中は真下だけを見ているので、左右移動は判定の後に行う
		moveHorizontal(ref pos, 1);
	}

//	参考までに、ステート制御以前の元の処理を残しておく
#if false
	private void updateProc()
	{
		var pos = m_position;
		float rate = 1.0f;

		float fallMove = -4;
		Vector2 backVector = Vector2.zero;

		//	ジャンプ中は足元の地面判定はスルー
		//	ジャンプ中にめり込み判定に引っかかって引き戻されるため
		if(m_jump > 0 || !isGround(m_position, new Vector2(0, fallMove), ref backVector))
		{
			if(m_jump == 0)
			{
				pos.y += fallMove;
				m_animator.SetInteger("State", (int)ANIM_STATE.FALL);
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

				m_animator.SetInteger("State", (int)ANIM_STATE.JUMP);
			}
		}

		float h = Input.GetAxis("Horizontal");
		float move = 0;
		int dir = 0;

		if(h >= 0.2f)
		{
			move = 2 * rate;
			dir = 1;
		}
		else if(h <= -0.2f)
		{
			move = -2 * rate;
			dir = -1;
		}

		//	地面の上を移動したときのアニメーション設定
		if(move != 0 && rate == 1)
		{
			m_animator.SetInteger("State", (int)ANIM_STATE.WALK);
		}

		if(move > 0)
		{
			m_animSprite.flipX = true;
		}
		else
		{
			m_animSprite.flipX = false;
		}


		move += m_moveDirection.x;
		pos += m_moveDirection;
		m_moveDirection = Vector2.zero;   //	外部要因を反映済みなのでクリア
		m_isMoved = false;

		if(dir != 0)
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
				m_animator.SetInteger("State", (int)ANIM_STATE.FALL);
			}
		}

		backVector = Vector2.zero;
		if(isCeiling(pos, new Vector2(0, m_jump), ref backVector))
		{
			pos.y += m_jump + backVector.y;
			m_jump = 0;
			m_jumpCount = 0;
			m_animator.SetInteger("State", (int)ANIM_STATE.FALL);
		}
		else
		{
			pos.y += m_jump;
		}

		m_position = pos;
	}
#endif

	public override void updateGraph()
	{
		m_chobj.transform.position = m_position;
	}

	public override void onHit(Character.ICore target)
	{

	}

	public Vector2 getSize()
	{
		return new Vector2(16,16);
	}

	bool m_isMoved;
	Vector2 m_moveDirection;
	public void move(Vector2 dir)
	{
		m_isMoved = true;
		m_moveDirection = dir;
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
		if(m_isMoved)
		{
			//	既に押し戻しがあったものとして打ち消す
			backVector -= offset;
			return true;
		}

		var objects = GameObject.FindObjectsOfType<Lift>();
		foreach(var obj in objects)
		{
			Vector2 backVec = Vector2.zero;
			if(obj.isOn(getSize(), m_position+offset, ref backVec))
			{
				backVector = backVec;
				return true;
			}
		}

		return isWall(pos, offset, ref backVector);
	}


	bool isWall(Vector2 pos, Vector2 offset, ref Vector2 backVector)
	{
		backVector = Vector2.zero;

		var obj = m_stageObj;

		var stageObj = obj.GetComponent<Stage.TilemapStageObject>();

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
				var chip = stageObj.getChip(x, y);

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
