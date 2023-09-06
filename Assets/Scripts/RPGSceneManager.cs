using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSceneManager : MonoBehaviour
{
	public Player Player;
	public Map ActiveMap;

	Coroutine _currentCoroutine;

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
				var massData = ActiveMap.GetMassData(movedPos);
				Player.SetDir(move);
				if (massData.isMovable)
				{
					Player.Pos = movedPos;
					yield return new WaitWhile(() => Player.IsMoving);
				}
			}
			yield return null;
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
}
