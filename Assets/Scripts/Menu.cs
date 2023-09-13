using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Menu : MonoBehaviour
{
	public bool DoOpen { get => gameObject.activeSelf; }

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	public void Open()
	{
		gameObject.SetActive(true);
	}
}