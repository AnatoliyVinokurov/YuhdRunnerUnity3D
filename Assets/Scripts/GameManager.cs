using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public GameObject ResultObj;
	public GameObject MainMenuObj;
	public GameObject SettingsObj;
	public Character PM;
	public RoadSpawner RS;
	public float MoveSpeed = 1;
	public List<Skin> Skins;
	public Text ScoreTxt, 
		CoinsTxt;
	float Score;
	float ScoreMultiplier = 1;
	//it will be bought in the boosts store (in main menu)
	public int Coins = 0;

	public Camera Cam;

	void Start ()
	{
		MainMenuObj.SetActive (true);
		PM.CanPlay = false;
		Cam = GetComponent<Camera> ();
		Cam = Camera.main;
		Cam.transform.Rotate (0, -90, 15);

	}

	public void ContinueTheGame ()
	{
		ResultObj.SetActive (false);
		PM.CanPlay = true;
		PM.m_Animator.ResetTrigger ("death");
		PM.m_Animator.SetTrigger ("run");
		PM.RespawnColider ();
		PM.ImmunityVb ();//select invulnerability
		if (MoveSpeed > 1) {
			MoveSpeed = MoveSpeed - MoveSpeed / 4;
		}
		;
	}

	public void RestartTheGame ()
	{
		ResultObj.SetActive (false);
		RS.StartTheGame ();
		PM.CanPlay = true;
		PM.m_Animator.ResetTrigger ("death");
		PM.m_Animator.SetTrigger ("run");
		PM.RespawnColider ();
		Score = 0;
		Coins = 0;
		MoveSpeed = 1;
		PM.ReturnToMidline ();
		PM.StopAllCoroutines ();
	}

	public void PlayTheGame ()
	{
		Cam.transform.Rotate (15, 90, 0);
		//Cam.transform.Translate(0,5,-6);
		MainMenuObj.SetActive (false);
		RS.StartTheGame ();
		PM.CanPlay = true;
		PM.m_Animator.ResetTrigger ("death");
		PM.m_Animator.SetTrigger ("run");
		PM.RespawnColider ();
		Score = 0;
		Coins = 0;
		MoveSpeed = 1;
		PM.ReturnToMidline ();
		PM.StopAllCoroutines ();
	}

	public void GoInMainMenu ()
	{
		ResultObj.SetActive (false);
		MainMenuObj.SetActive (true);
		RS.StartTheGame ();
		PM.ReturnToMidline ();
		Cam.transform.Rotate (0, -90, 15);
	}

	public void GoInSettingsScreen ()
	{
		SettingsObj.SetActive (true);
	}

	public void CloseSettingsScreen ()
	{
		SettingsObj.SetActive (false);
	}

	public void PauseTheGame ()
	{
		//Pause the game script
	}

	void Update ()
	{
		if (PM.CanPlay) {
			Score = PM.CalculatingTheDistanceTraveled * ScoreMultiplier;
			MoveSpeed += .01f * Time.deltaTime;
			MoveSpeed = Mathf.Clamp (MoveSpeed, 1, 2);
		}
		ScoreTxt.text = ((int)Score).ToString ();
	}

	public void ShowResult ()
	{
		ResultObj.SetActive (true);
		SaveManager.Instance.SaveGame ();
	}

	public void AddCoins (int number)
	{
		Coins += number;
		RefreshText ();
	}

	public void RefreshText ()
	{
		CoinsTxt.text = Coins.ToString ();
	}



	public void ActivateSkin (int skinIndex, bool setTrigger = false)
	{
		
		foreach (var skin in Skins)
			skin.HideSkin ();

		Skins [skinIndex].ShowSkin ();
		PM.m_Animator = Skins [skinIndex].AC;

		if (setTrigger)
			PM.m_Animator.SetTrigger ("death");
	}


}
