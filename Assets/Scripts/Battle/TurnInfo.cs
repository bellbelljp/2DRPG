using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnInfo
{
	public string Message;
	public string[] Params;
	public Animator[] Effects;

	public UnityAction DoneCommand;

	public void ShowMessageWindow(MessageWindow messageWindow)
	{
		messageWindow.Params = Params;
		messageWindow.Effects = Effects;
		if (!messageWindow.gameObject.activeSelf)
			messageWindow.gameObject.SetActive(true);
		messageWindow.StartMessage(Message);
	}
}