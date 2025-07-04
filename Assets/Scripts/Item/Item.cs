using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
	public string Name;
	public string Description;
	public int Money;

	public virtual void Use(BattleParameterBase target) { }
}
