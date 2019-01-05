using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


	public override void update()
	{
		var pos = position;
		if(!isGround())
		{
			pos.y -= 0.1f;
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

	bool isGround()
	{
		var x = Mathf.Round(position.x / 0.5f) * 0.5f;
		var y = Mathf.Round(position.y / 0.5f) * 0.5f;

		var obj = GameObject.Find("Stage");
		if(obj == null)
		{
			return false;
		}
		var stageObj = obj.GetComponent<Stage.StageObject>();

		if(stageObj != null)
		{
			var chip = stageObj.stage.getChip((int)(x/0.5f), (int)((y - 0.5f)/0.5f));

			drawRectLine(position.x, position.y-0.5f);

			if(chip == 1)
			{
				return true;
			}


			stageObj.markChip(0, 0);
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
