using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopItem : MonoBehaviour
{

	public enum ItemType
	{
		FIRST_SKIN,
		SECOND_SKIN,
		THERD_SKIN,
		FORT_SKIN
	}

	public ItemType Type;
	public Button BuyButton, ActivateButton;
	public bool IsBought;
	public int Cost;


	bool IsActive {
		get {
			return Type == SM.ActiveSkin;
		}
	}

	GameManager gm;
	public ShopManager SM;

	public void Init ()
	{
		gm = FindObjectOfType<GameManager> ();
	}
		
	public void CheckButtons ()
	{
		BuyButton.gameObject.SetActive (!IsBought);
		BuyButton.interactable = CanBuy ();

		ActivateButton.gameObject.SetActive (IsBought);
		ActivateButton.interactable = !IsActive;
	}



	bool CanBuy ()
	{
		return gm.Coins >= Cost;
	}


	public void BuyItem ()
	{
		if (!CanBuy ())
			return;

		IsBought = true;
		gm.Coins -= Cost;

		CheckButtons ();


		SaveManager.Instance.SaveGame ();

		gm.RefreshText ();

	}


	public void ActivateItem ()
	{
		SM.ActiveSkin = Type;
		SM.CheckItemButtons ();

		switch (Type) {
		case ItemType.FIRST_SKIN:
			gm.ActivateSkin (0, true);
			break;
		case ItemType.SECOND_SKIN:
			gm.ActivateSkin (1, true);
			break;
		case ItemType.THERD_SKIN:
			gm.ActivateSkin (2, true);
			break;
		case ItemType.FORT_SKIN:
			gm.ActivateSkin (3, true);
			break;
		}


		SaveManager.Instance.SaveGame ();

	}
		
}
