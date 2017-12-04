using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text farText, closeText, turnsNumber, movingText, enemyTurnText, occupiedText, gameOverText;

	public Text[] CharacterSheetTexts;
	
	public void Initialize()
	{
		DisableAllWarnings();
		DisableMovingText();
		DisableEnemyTurnText();
		DisableGameOverText();
	}

	public void NextTurn()
	{
		if(MainManager.Instance.EnemyTurn || MainManager.Instance._PlayerController.characterAttacking)
			return;
			
		MainManager.Instance.StartEnemyTurn();
		EnableEnemyTurnText();
	}

	public void RefreshCharacterInfo()
	{
		CharacterSheetTexts[0].text = MainManager.Instance._PlayerController.CurrentCharacterSelected.name;
		CharacterSheetTexts[1].text = MainManager.Instance._PlayerController.CurrentCharacterSelected.movementsLeft.ToString();
		CharacterSheetTexts[2].text = MainManager.Instance._PlayerController.CurrentCharacterSelected.Life.ToString();
		CharacterSheetTexts[3].text = MainManager.Instance._PlayerController.CurrentCharacterSelected.CurrentWeaponSelected.Name;
	}

	public void ChangeWeapon()
	{
		MainManager.Instance._PlayerController.ChangeWeapon();
		RefreshCharacterInfo();
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

	public void EnableGameOverText()
	{
		gameOverText.enabled = true;
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
	
	public void DisableGameOverText()
	{
		gameOverText.enabled = false;
	}
	private IEnumerator Wait(int option)
	{
		yield return new WaitForSeconds(1);
		DisableAllWarnings();
	}
#endregion

}
