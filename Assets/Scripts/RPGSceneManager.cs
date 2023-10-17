using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSceneManager : MonoBehaviour
{
	public Player Player;
	public Map ActiveMap;
	public MessageWindow MessageWindow;
	public Menu Menu;
	public ItemShopMenu ItemShopMenu;
	public Vector3Int MassEventPos { get; private set; }
	public ItemList ItemList;
	[SerializeField] public BattleWindow BattleWindow;
	[SerializeField, TextArea(3, 15)] string GameOverMessage = "体力が無くなった...";
	[SerializeField] Map RespawnMapPrefab;
	[SerializeField] Vector3Int RespawnPos;
	[SerializeField, TextArea(3, 15)] string GameClearMessage = "ゲームクリアー";
	[SerializeField] GameClear gameClearObj;

	Coroutine _currentCoroutine;

	public bool IsPauseScene
	{
		get
		{
			return !MessageWindow.IsEndMessage || Menu.DoOpen ||
				ItemShopMenu.DoOpen || BattleWindow.DoOpen;
		}
	}

	void Start()
	{
		_currentCoroutine = StartCoroutine(MovePlayer());
	}

	/// <summary>プレイヤーの移動</summary>
	IEnumerator MovePlayer()
	{
		while (true)
		{
			if (GetArrowInput(out var move))
			{
				var movedPos = Player.Pos + move;
				// 移動先のマスを取得
				var massData = ActiveMap.GetMassData(movedPos);
				Player.SetDir(move);
				if (massData.isMovable)
				{
					Player.Pos = movedPos;
					yield return new WaitWhile(() => Player.IsMoving);
					if (massData.massEvent != null)
					{
						MassEventPos = movedPos;
						massData.massEvent.Exec(this);
					}
					else if (ActiveMap.RandomEncount != null)
					{
						var rnd = new System.Random();
						var encount = ActiveMap.RandomEncount.Encount(rnd);
						if (encount != null)
						{
							BattleWindow.SetUseEncounter(encount);
							BattleWindow.Open();
						}
					}
				}
				else if (massData.character != null && massData.character.Event != null)
				{
					MassEventPos = movedPos;
					massData.character.Event.Exec(this);
				}
			}
			yield return new WaitWhile(() => IsPauseScene);

			if (Player.BattleParameter.HP <= 0)
			{
				StartCoroutine(GameOver());
				yield break;
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				OpenMenu();
			}
		}
	}

	/// <summary>移動方向を取得</summary>
	bool GetArrowInput(out Vector3Int move)
	{
		var doMove = false;
		move = Vector3Int.zero;
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			move.x -= 1; doMove = true;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			move.x += 1; doMove = true;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			move.y += 1; doMove = true;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			move.y -= 1; doMove = true;
		}
		return doMove;
	}

	/// <summary>メッセージウィンドウを表示</summary>
	public void ShowMessageWindow(string message)
	{
		MessageWindow.StartMessage(message);
	}

	public void OpenMenu()
	{
		Menu.Open();
	}

	/// <summary>ゲームオーバー</summary>
	IEnumerator GameOver()
	{
		MessageWindow.StartMessage(GameOverMessage);
		yield return new WaitUntil(() => MessageWindow.IsEndMessage);

		RespawnMap(true);
	}

	/// <summary>リスポーン</summary>
	void RespawnMap(bool isGameOver)
	{
		Object.Destroy(ActiveMap.gameObject);
		ActiveMap = Object.Instantiate(RespawnMapPrefab);

		Player.SetPosNoCoroutine(RespawnPos);
		Player.CurrentDir = Direction.Down;
		if (isGameOver)
		{
			Player.BattleParameter.HP = 1;
			Player.BattleParameter.Money = 100;
		}

		if (_currentCoroutine != null)
		{
			StopCoroutine(_currentCoroutine);
		}
		_currentCoroutine = StartCoroutine(MovePlayer());
	}

	public void GameClear()
	{
		StopCoroutine(_currentCoroutine);

		_currentCoroutine = StartCoroutine(GameClearCoroutine());
	}

	IEnumerator GameClearCoroutine()
	{
		MessageWindow.StartMessage(GameClearMessage);
		yield return new WaitUntil(() => MessageWindow.IsEndMessage);

		gameClearObj.StartMessage(gameClearObj.Message);
		yield return new WaitWhile(() => gameClearObj.DoOpen);

		_currentCoroutine = null;
		RespawnMap(false);
	}
}
