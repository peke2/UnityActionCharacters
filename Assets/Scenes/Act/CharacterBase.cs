using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : Character.CharacterObject {
	public enum Type
	{
		Player,
		Enemy,
		Item,
	}

	public Type type { get; set; }
}
