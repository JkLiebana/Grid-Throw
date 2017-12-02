using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text farText, closeText, turnsNumber, movingText, enemyTurnText, occupiedText;

	public Text[] CharacterSheetTexts;
	
	public void Initialize()
	{
		DisableAllWarnings();
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

#region Enable Text
	public void EnableFarText()
	{
		farText.enabled = true;
		StartCoroutine(Wait(0));
	}

	public void EnableCloseText()
	{
		closeText.enabled = true;
		StartCoroutine(Wait(1));		
	}

	public void EnableOccupiedText()
	{
		occupiedText.enabled = true;
		StartCoroutine(Wait(1));		
	}

	public void EnableMovingText()
	{
		movingText.enabled = true;
	}

	public void EnableEnemyTurnText()
	{
		enemyTurnText.enabled = true;
	}

#endregion
	
#region Disable Text
	public void DisableAllWarnings()
	{
		occupiedText.enabled = false;
		closeText.enabled = false;
		farText.enabled = false;		
	}	

	public void DisableMovingText()
	{
		movingText.enabled = false;
	}

	public void DisableEnemyTurnText()
	{
		enemyTurnText.enabled = false;
	}

	private IEnumerator Wait(int option)
	{
		yield return new WaitForSeconds(1);
		DisableAllWarnings();
	}
#endregion

}
