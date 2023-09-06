using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
	public class Mass
	{
		public bool isMovable;
		public TileBase eventTile;
		public MassEvent massEvent;
	}

	public Grid Grid { get => GetComponent<Grid>(); }
	Dictionary<string, Tilemap> _tilemaps;

	[SerializeField]
	List<MassEvent> _massEvents;

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

		if(mass.eventTile != null)
		{
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

}