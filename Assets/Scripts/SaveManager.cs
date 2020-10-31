using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveManager : MonoBehaviour
{

	public ShopManager SM;
	public GameManager GM;

	string filePath;

	public static SaveManager Instance;

	private void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else {
			Destroy (gameObject);
			return;
		}

		GM = FindObjectOfType<GameManager> ();
		filePath = Application.persistentDataPath + "data.gamesave";

		LoadGame ();
		SaveGame ();
	}

	public void SaveGame ()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = new FileStream (filePath, FileMode.Create);

		Save save = new Save ();
		save.Coins = GM.Coins;
		save.ActiveSkinIndex = (int)SM.ActiveSkin;
		save.SaveBoughtItems (SM.Items);

		bf.Serialize (fs, save);
		fs.Close ();
	}

	public void LoadGame ()
	{
		if (!File.Exists (filePath))
			return;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = new FileStream (filePath, FileMode.Open);

		Save save = (Save)bf.Deserialize (fs);

		GM.Coins = save.Coins;
		SM.ActiveSkin = (ShopItem.ItemType)save.ActiveSkinIndex;

		for (int i = 0; i < save.BoughtItems.Count; i++)
			SM.Items [i].IsBought = save.BoughtItems [i];

		fs.Close ();

		GM.RefreshText ();
		GM.ActivateSkin ((int)SM.ActiveSkin);
	}

}

[System.Serializable]
public class Save
{
	public int Coins;
	public int ActiveSkinIndex;
	public List<bool> BoughtItems = new List<bool> ();

	public void SaveBoughtItems (List<ShopItem> items)
	{
		foreach (var item in items)
			BoughtItems.Add (item.IsBought);
	}








}
