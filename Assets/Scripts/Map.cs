using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Map : MonoBehaviour
{
	public class Mass
	{
		public bool isMovable;
		public TileBase eventTile;
		public MassEvent massEvent;
		public CharacterBase character;
	}

	[SerializeField]
	List<MassEvent> _massEvents;

	public Grid Grid { get => GetComponent<Grid>(); }
	Dictionary<string, Tilemap> _tilemaps;
	HashSet<CharacterBase> _characters = new HashSet<CharacterBase>();
	public RandomEncount RandomEncount;

	readonly static string BACKGROND_TILEMAP_NAME = "Background";
	readonly static string NONE_OBJECTS_TILEMAP_NAME = "NoneObjects";
	readonly static string OBJECTS_TILEMAP_NAME = "Objects";
	readonly static string EVENT_BOX_TILEMAP_NAME = "EventBox";

	private void Awake()
	{
		_tilemaps = new Dictionary<string, Tilemap>();
		foreach (var tilemap in Grid.GetComponentsInChildren<Tilemap>())
		{
			_tilemaps.Add(tilemap.name, tilemap);
		}

		// EventBoxを非表示にする
		_tilemaps[EVENT_BOX_TILEMAP_NAME].gameObject.SetActive(false);

		AddCharacter(Object.FindObjectOfType<Player>());
	}

	public Vector3 GetWorldPos(Vector3Int pos)
	{
		// セル座標をワールド座標に変換する
		return Grid.CellToWorld(pos);
	}

	public Mass GetMassData(Vector3Int pos)
	{
		var mass = new Mass();
		mass.eventTile = _tilemaps[EVENT_BOX_TILEMAP_NAME].GetTile(pos);
		mass.isMovable = true;
		mass.character = GetCharacter(pos);

		if (mass.character != null)
		{
			mass.isMovable = false;
		}
		else if (mass.eventTile != null)
		{
			// イベントマスに該当するイベントを取得する
			mass.massEvent = FindMassEvent(mass.eventTile);
		}
		else if (_tilemaps[OBJECTS_TILEMAP_NAME].GetTile(pos))
		{
			mass.isMovable = false;
		}
		else if (_tilemaps[BACKGROND_TILEMAP_NAME].GetTile(pos) == null)
		{
			mass.isMovable = false;
		}
		return mass;
	}

	public MassEvent FindMassEvent(TileBase tile)
	{
		return _massEvents.Find(_c => _c.Tile == tile);
	}

	/// <summary>該当マスの位置を取得</summary>
	public bool FindMassEventPos(TileBase tile, out Vector3Int pos)
	{
		var eventLayer = _tilemaps[EVENT_BOX_TILEMAP_NAME];
		var renderer = eventLayer.GetComponent<Renderer>();
		var min = eventLayer.LocalToCell(renderer.bounds.min);
		var max = eventLayer.LocalToCell(renderer.bounds.max);
		pos = Vector3Int.zero;
		// イベントレイヤーの端から端までチェックして、該当のマスがあるかチェック
		for (pos.y = min.y; pos.y < max.y; ++pos.y)
		{
			for (pos.x = min.x; pos.x < max.x; ++pos.x)
			{
				var t = eventLayer.GetTile(pos);
				if (t == tile) return true;
			}
		}
		return false;
	}

	public void AddCharacter(CharacterBase character)
	{
		if (!_characters.Contains(character) && character != null)
		{
			_characters.Add(character);
		}
	}

	public CharacterBase GetCharacter(Vector3Int pos)
	{
		return _characters.FirstOrDefault(_c => _c.Pos == pos);
	}
}