using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Up,
	Down,
	Left,
	Right,
}

public class Player : CharacterBase
{
	protected override void Start()
	{
		DoMoveCamera = true;
		base.Start();
	}
}

//public class Player : MonoBehaviour
//{
//	[Range(0, 2)] public float MoveSecond = 0.1f;
//	[SerializeField] RPGSceneManager RPGSceneManager;

//	Coroutine _moveCoroutine;
//	[SerializeField] Vector3Int _pos;
//	public Vector3Int Pos
//	{
//		get => _pos;
//		set
//		{
//			if (_pos == value) return;

//			if (RPGSceneManager.ActiveMap == null)
//			{
//				_pos = value;
//			}
//			else
//			{
//				if (_moveCoroutine != null)
//				{
//					StopCoroutine(_moveCoroutine);
//					_moveCoroutine = null;
//				}
//				_moveCoroutine = StartCoroutine(MoveCoroutine(value));
//			}

//		}
//	}

//	[SerializeField] Direction _currentDir = Direction.Down;
//	public Direction CurrentDir
//	{
//		get => _currentDir;
//		set
//		{
//			if (_currentDir == value) return;
//			_currentDir = value;
//			SetDirAnimation(value);
//		}
//	}

//	Animator Animator { get => GetComponent<Animator>(); }
//	static readonly string TRIGGER_MOVEDOWN = "MoveDownTrigger";
//	static readonly string TRIGGER_MOVELEFT = "MoveLeftTrigger";
//	static readonly string TRIGGER_MOVERIGHT = "MoveRightTrigger";
//	static readonly string TRIGGER_MOVEUP = "MoveUpTrigger";

//	private void Awake()
//	{
//		SetDirAnimation(_currentDir);
//	}

//	private void Start()
//	{
//		if (RPGSceneManager == null) RPGSceneManager = Object.FindObjectOfType<RPGSceneManager>();

//		_moveCoroutine = StartCoroutine(MoveCoroutine(Pos));
//	}

//	/// <summary>コルーチンなしで移動</summary>
//	public void SetPosNoCoroutine(Vector3Int pos)
//	{
//		_pos = pos;
//		transform.position = RPGSceneManager.ActiveMap.Grid.CellToWorld(pos);
//		// カメラを追従させる
//		Camera.main.transform.position = transform.position + Vector3.forward * -10;
//	}
//	public bool IsMoving { get => _moveCoroutine != null; }

//	/// <summary>移動コルーチン</summary>
//	IEnumerator MoveCoroutine(Vector3Int pos)
//	{
//		var startPos = transform.position;
//		var goalPos = RPGSceneManager.ActiveMap.Grid.CellToWorld(pos);
//		var t = 0f;
//		while (t < MoveSecond)
//		{
//			yield return null;
//			t += Time.deltaTime;
//			transform.position = Vector3.Lerp(startPos, goalPos, t / MoveSecond);
//			// カメラを追従させる
//			Camera.main.transform.position = transform.position + Vector3.forward * -10;
//		}
//		_pos = pos;
//		_moveCoroutine = null;
//	}

//	private void OnValidate()
//	{
//		if (RPGSceneManager != null && RPGSceneManager.ActiveMap != null)
//		{
//			transform.position = RPGSceneManager.ActiveMap.Grid.CellToWorld(Pos);
//		}
//	}

//	/// <summary>方向をセット</summary>
//	public void SetDir(Vector3Int move)
//	{
//		if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
//		{
//			CurrentDir = move.x > 0 ? Direction.Right : Direction.Left;
//		}
//		else
//		{
//			CurrentDir = move.y > 0 ? Direction.Up : Direction.Down;
//		}
//	}

//	/// <summary>方向アニメーションをセット</summary>
//	public void SetDirAnimation(Direction dir)
//	{
//		if (Animator == null || Animator.runtimeAnimatorController == null)
//			return;

//		string triggerName = null;
//		switch (dir)
//		{
//			case Direction.Up: triggerName = TRIGGER_MOVEUP; break;
//			case Direction.Down: triggerName = TRIGGER_MOVEDOWN; break;
//			case Direction.Left: triggerName = TRIGGER_MOVELEFT; break;
//			case Direction.Right: triggerName = TRIGGER_MOVERIGHT; break;
//			default: throw new System.NotImplementedException("");
//		}
//		Animator.SetTrigger(triggerName);
//	}
//}