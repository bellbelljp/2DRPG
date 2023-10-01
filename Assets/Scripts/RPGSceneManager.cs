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
	[SerializeField] public BattleWindow BattleWindow;

	Coroutine _currentCoroutine;

	public bool IsPauseScene
	{
		get
		{
			return !MessageWindow.IsEndMessage || Menu.DoOpen ||
				ItemShopMenu.DoOpen || BattleWindow.DoOpen;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			BattleWindow.Open();
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
						massData.massEvent.Exec(this);
					}
				}
				else if (massData.character != null && massData.character.Event != null)
				{
					massData.character.Event.Exec(this);
				}
			}
			yield return new WaitWhile(() => IsPauseScene);

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
}
