using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : CharacterBase {

	GameObject chobj;

	System.Action stateProc;

	// Use this for initialization
	void Start () {
		var ctrl = Character.ControlObject.getControl();
		ctrl.addObject(this);

		var obj = Resources.Load<GameObject>("Player");
		chobj = GameObject.Instantiate<GameObject>(obj);
		chobj.name = "PlayerObject";
		chobj.transform.SetParent(gameObject.transform, false);
		position = new Vector2(2.5f,8);

		hit = new Character.Hit();
		hit.setSize(new Vector2(1f,1f));
		hit.setOffset(new Vector2(-0.5f,0.5f));

		type = CharacterBase.Type.Player;
	}


	int jumpCount = 0;
	float jump = 0;
	public override void update()
	{
		var pos = position;
		float rate = 1.0f;

		if(!isGround(position))
		{
			if(jump == 0)
			{
				pos.y -= 0.2f;
			}
			rate = 0.4f;
		}
		else
		{
			if(Input.GetButtonDown("Jump") && jump == 0)
			{
				jump = 0.4f;
				jumpCount = 8;
			}
		}

		float h = Input.GetAxis("Horizontal");
		float move = 0;
		int dir = 0;

		if(h >= 0.2f)
		{
			move = 0.2f;
			dir = 1;
		}
		else if(h <= -0.2f)
		{
			move = -0.2f;
			dir = -1;
		}

		if(dir!=0)
		{
			pos.x += move * rate;

			if(isWall(pos, dir, 0))
			{
				//move = 0;
				pos.x -= move * rate;
			}
		}


		if(jumpCount > 0)
		{
			jumpCount--;
		}
		else
		{
			jump = jump / 2;
			if(jump < 0.1f)
			{
				jump = 0;
			}
		}

		pos.y += jump;
		if(isCeiling(pos))
		{
			pos.y -= jump;
			jump = 0;
			jumpCount = 0;
		}

		position = pos;
	}

	public override void updateGraph()
	{
		chobj.transform.position = position;
	}

	public override void onHit(Character.ICore target)
	{

	}

	bool isCeiling(Vector2 pos)
	{
		return isWall(pos, 0, 1);
	}

	bool isGround(Vector2 pos)
	{
		return isWall(pos, 0, -1);
	}

	bool isWall(Vector2 pos, int hdir, int vdir)
	{
		var x = Mathf.Round(pos.x / 0.5f) * 0.5f;
		var y = Mathf.Round(pos.y / 0.5f) * 0.5f;

		var obj = GameObject.Find("Stage");
		if(obj == null)
		{
			return false;
		}
		var stageObj = obj.GetComponent<Stage.StageObject>();

		if(stageObj != null)
		{
			var chip = stageObj.stage.getChip((int)((x + 0.5f*hdir) / 0.5f), (int)((y + 0.5f*vdir) / 0.5f));

			if(chip == 1)
			{
				return true;
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
