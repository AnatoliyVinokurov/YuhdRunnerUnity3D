using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
	public List<ShopItem> Items;
	public ShopItem.ItemType ActiveSkin;


	void Start ()
	{
		CheckItemButtons ();
	}

	public void OpenShop ()
	{
		CheckItemButtons ();
		gameObject.SetActive (true);
	}

	public void CloseShop ()
	{
		gameObject.SetActive (false);
	}

	public void CheckItemButtons ()
	{
		foreach (ShopItem item in Items) {
			item.SM = this;
			item.Init ();
			item.CheckButtons ();
		}
	}

}
