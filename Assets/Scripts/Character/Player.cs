using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleParameterBase;
using static UnityEditor.Progress;

public enum Direction
{
	Up,
	Down,
	Left,
	Right,
}

public class Player : CharacterBase
{
	[System.Serializable]
	public class PlayerSaveData : SaveData
	{
		public BattleParameterBaseSaveData battleParameter;
		public PlayerSaveData() { }
		public PlayerSaveData(Player character, RPGSceneManager RPGManager) : base(character)
		{
			battleParameter = new BattleParameterBaseSaveData(character.BattleParameter, RPGManager.ItemList);
		}
	}

	public BattleParameter InitialBattleParameter;
	public BattleParameterBase BattleParameter;

	protected override void Start()
	{
		DoMoveCamera = true;
		base.Start();
		InitialBattleParameter.Data.CopyTo(BattleParameter);
	}

	public override SaveData GetSaveData()
	{
		return new PlayerSaveData(this, RPGSceneManager);
	}

	public override void LoadSaveData(string saveDataJson)
	{
		var saveData = JsonUtility.FromJson<PlayerSaveData>(saveDataJson);
		BattleParameter = saveData.battleParameter.Load(RPGSceneManager.ItemList);
	}
}