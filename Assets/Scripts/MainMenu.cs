using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : Menu
{
	[SerializeField] protected RPGSceneManager RPGSceneManager;

	public GameObject ParameterRoot;
	public MenuRoot ItemInventory;

	public void UseItem()
	{
		//TODO
		UpdateUI();
	}

	public override void Open()
	{
		base.Open();
		UpdateUI();
	}

	void UpdateUI()
	{
		UpdateItems();
		UpdateParameters();
	}

	void UpdateItems()
	{
		//アイテムは6個までを上限と想定して作成しています。(武器+防具+アイテム4個)
		var player = RPGSceneManager.Player;
		var items = player.BattleParameter.Items;
		var menuItems = ItemInventory.MenuItems;
		foreach (var menuItem in menuItems)
		{
			menuItem.gameObject.SetActive(false);
		}

		int i = 0;
		if (player.BattleParameter.AttackWeapon != null)
		{
			menuItems[i].gameObject.SetActive(true);
			menuItems[i].Text = player.BattleParameter.AttackWeapon.Name;
			i++;
		}
		if (player.BattleParameter.DefenseWeapon != null)
		{
			menuItems[i].gameObject.SetActive(true);
			menuItems[i].Text = player.BattleParameter.DefenseWeapon.Name;
			i++;
		}

		for (var itemIndex = 0; i < menuItems.Length && itemIndex < items.Count; ++i, ++itemIndex)
		{
			var menuItem = menuItems[i];
			menuItem.gameObject.SetActive(true);
			menuItem.Text = items[itemIndex].Name;
		}
	}

	void UpdateParameters()
	{
		var player = RPGSceneManager.Player;
		var param = player.BattleParameter;
		SetParameterText("LEVEL", param.Level.ToString());
		SetParameterText("EXP", param.Exp.ToString());
		SetParameterText("HP", $"{param.HP}/{param.MaxHP}");
		SetParameterText("ATK", param.Attack.ToString());
		SetParameterText("DEF", param.Defense.ToString());
		SetParameterText("MONEY", param.Money.ToString());
	}

	void SetParameterText(string name, string text)
	{
		var root = ParameterRoot.transform.Find(name);
		var textObj = root.Find("Text").GetComponent<Text>();
		textObj.text = text;
	}
}