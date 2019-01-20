using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{

	GameObject m_chobj;

	// Use this for initialization
	void Start()
	{
		var ctrl = Character.ControlObject.getControl();
		ctrl.addObject(this);

		var obj = Resources.Load<GameObject>("Enemy");
		m_chobj = GameObject.Instantiate<GameObject>(obj);
		m_chobj.name = "EnemyObject";
		m_chobj.transform.SetParent(gameObject.transform,false);

		hit = new Character.Hit();
		hit.setSize(new Vector2(1f,1f));
		hit.setOffset(new Vector2(-0.5f,0.5f));

		type = CharacterBase.Type.Enemy;

	}


	public override void update()
	{
	}

	public override void updateGraph()
	{
		m_chobj.transform.position = position;

		var lt = position + hit.getOffset();
		var rb = lt + hit.getSize() * new Vector2(1,-1);
		Vector3[] vec = new Vector3[]{
			new Vector3(lt.x, lt.y, 0),
			new Vector3(rb.x, lt.y, 0),
			new Vector3(rb.x, rb.y, 0),
			new Vector3(lt.x, rb.y, 0),
			new Vector3(lt.x, lt.y, 0),
		};
		for(int i = 0; i < 4; i++)
		{
			Debug.DrawLine(vec[i], vec[i+1], Color.red);
		}
	}

	public override void onHit(Character.ICore target)
	{
		CharacterBase cbase = (CharacterBase)target;
		if(cbase.type == CharacterBase.Type.Player)
		{
			//Debug.Log(cbase.type);
			isRemoved = true;
		}
	}

}
