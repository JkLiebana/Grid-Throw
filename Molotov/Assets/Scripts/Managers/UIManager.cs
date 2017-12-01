using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text farText, closeText, turnsNumber, movingText, enemyTurnText;

	public Text[] CharacterSheetTexts;
	
	public void Initialize()
	{
		DisableCloseText();
		DisableFarText();
		DisableMovingText();
		DisableEnemyTurnText();
	}

	public void NextTurn()
	{
		if(MainManager.Instance.EnemyTurn)
			return;
			
		MainManager.Instance.StartEnemyTurn();
		EnableEnemyTurnText();
	}

	public void RefreshCharacterInfo()
	{
		CharacterSheetTexts[0].text = MainManager.Instance._PlayerController.CurrentCharacterSelected.name;
		CharacterSheetTexts[1].text = MainManager.Instance._PlayerController.CurrentCharacterSelected.movementsLeft.ToString();
	}
	public void EnableFarText()
	{
		farText.enabled = true;
		StartCoroutine(Wait(0));
	}

	public void DisableFarText()
	{

		farText.enabled = false;
	}

	public void EnableCloseText()
	{
		closeText.enabled = true;
		StartCoroutine(Wait(1));		
	}

	public void DisableCloseText()
	{
		closeText.enabled = false;
	}

	public void EnableMovingText()
	{
		movingText.enabled = true;
	}

	public void DisableMovingText()
	{
		movingText.enabled = false;
	}

	public void EnableEnemyTurnText()
	{
		enemyTurnText.enabled = true;
	}

	public void DisableEnemyTurnText()
	{
		enemyTurnText.enabled = false;
	}

	IEnumerator Wait(int option)
	{
		yield return new WaitForSeconds(1);

		if(option == 0)
		{
			DisableFarText();
		}
		else{
			DisableCloseText();
		}
	}

}
