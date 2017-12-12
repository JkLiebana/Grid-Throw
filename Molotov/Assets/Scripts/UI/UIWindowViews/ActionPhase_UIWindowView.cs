using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionPhase_UIWindowView : UIWindowView {

	public Text warning_FarText, warning_CloseText, warning_MovingText, warning_EnemyTurnText, warning_OccupiedText;
	public Text turnsNumber; 
	public Text[] CharacterSheetTexts;

	public GameObject GameOverPanel;
	public UIWindowController MainMenuWindowController;
	
	public void Initialize()
	{
		DisableAllWarnings();
		DisableMovingText();
		DisableEnemyTurnText();
		DisableGameOverPanel();
	}

	public void NextTurn()
	{
		if(MainManager.Instance._ActionPhaseManager.EnemyTurn || MainManager.Instance._ActionPhaseManager._PlayerController.characterAttacking)
			return;
			
		MainManager.Instance._ActionPhaseManager.StartEnemyTurn();
		EnableEnemyTurnText();
	}

	public void GoToMenu()
	{
		MainManager.Instance._PoolingManager.DespawnAllTiles();
		MainManager.Instance._PoolingManager.DespawnAllCharacters();
		MainManager.Instance._PoolingManager.DespawnAllEnemies();
		MainManager.Instance._ActionPhaseManager._MapGenerator.Tiles.Clear();

		MainManager.Instance._UIManager.GoToWindow(MainMenuWindowController);

		MainManager.Instance._GameDirector.LoadMenu();
	}

	public void RefreshCharacterInfo()
	{
		CharacterSheetTexts[0].text = MainManager.Instance._ActionPhaseManager._PlayerController.CurrentCharacterSelected.name;
		CharacterSheetTexts[1].text = MainManager.Instance._ActionPhaseManager._PlayerController.CurrentCharacterSelected.movementsLeft.ToString();
		CharacterSheetTexts[2].text = MainManager.Instance._ActionPhaseManager._PlayerController.CurrentCharacterSelected.Life.ToString();
		CharacterSheetTexts[3].text = MainManager.Instance._ActionPhaseManager._PlayerController.CurrentCharacterSelected.CurrentWeaponSelected.Name;
		CharacterSheetTexts[4].text = MainManager.Instance._ActionPhaseManager._PlayerController.CurrentCharacterSelected.CurrentAttacksPerTurn.ToString();
	}

	public void ChangeWeapon()
	{
		MainManager.Instance._ActionPhaseManager._PlayerController.ChangeWeapon();
		RefreshCharacterInfo();
	}

#region Enable Text
	public void EnableFarText()
	{
		warning_FarText.enabled = true;
		StartCoroutine(Wait(0));
	}

	public void EnableCloseText()
	{
		warning_CloseText.enabled = true;
		StartCoroutine(Wait(1));		
	}

	public void EnableOccupiedText()
	{
		warning_OccupiedText.enabled = true;
		StartCoroutine(Wait(1));		
	}

	public void EnableMovingText()
	{
		warning_MovingText.enabled = true;
	}

	public void EnableEnemyTurnText()
	{
		warning_EnemyTurnText.enabled = true;
	}

	public void EnableGameOverPanel()
	{
		GameOverPanel.gameObject.SetActive(true);
	}

#endregion
	
#region Disable Text
	public void DisableAllWarnings()
	{
		warning_OccupiedText.enabled = false;
		warning_CloseText.enabled = false;
		warning_FarText.enabled = false;		
	}	

	public void DisableMovingText()
	{
		warning_MovingText.enabled = false;
	}

	public void DisableEnemyTurnText()
	{
		warning_EnemyTurnText.enabled = false;
	}
	
	public void DisableGameOverPanel()
	{
		GameOverPanel.gameObject.SetActive(false);
	}
	private IEnumerator Wait(int option)
	{
		yield return new WaitForSeconds(1);
		DisableAllWarnings();
	}
#endregion


}
