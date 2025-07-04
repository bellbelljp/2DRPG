using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "MassEvent/Move Map")]
public class MoveMapEvent : MassEvent
{
	public Map MoveMapPrefab;
	public TileBase StartPosTile;
	public Direction StartDirection;

	public override void Exec(RPGSceneManager manager)
	{
		var saveData = Object.FindObjectOfType<SaveData>();
		saveData.SaveTemporary(manager.ActiveMap);

		Destroy(manager.ActiveMap.gameObject);
		manager.ActiveMap = Instantiate(MoveMapPrefab);

		if (manager.ActiveMap.FindMassEventPos(StartPosTile, out var pos))
		{
			manager.Player.SetPosNoCoroutine(pos);
			manager.Player.CurrentDir = StartDirection;
		}
	}
}
