using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPhaseManager : Singleton<ActionPhaseManager>{

	public MapGenerator _MapGenerator;	
	public PlayerController _PlayerController;
	public EnemyController _EnemyController;
	public PathfindingManager _PathfindingManager;
	public ActionPhase_UIWindowView ActionPhaseWindowView;

	public int CurrentTurn = 0;

	public bool EnemyTurn = false;
	public bool isGameOver = false;

	public void Initialize()
	{
		isGameOver = false;
		EnemyTurn = false;
		
		_MapGenerator.Initialize();
		_PlayerController.Initialize();
		_EnemyController.Initialize();

		ActionPhaseWindowView = GameObject.FindObjectOfType<ActionPhase_UIWindowView>();
		ActionPhaseWindowView.Initialize();

		ActionPhaseWindowView.RefreshCharacterInfo();
	}	
	public void StartEnemyTurn()
	{
		EnemyTurn = true;
	}
	public void FinishEnemyTurn()
	{
		EnemyTurn = false;
		NextTurn();
	}
	IEnumerator ProcessEnemyTurn()
	{
		yield return new WaitForSeconds(3);
		NextTurn();
	}

	public void NextTurn()
	{
		CurrentTurn++;
		EnemyTurn = false;
		ActionPhaseWindowView.DisableEnemyTurnText();
		_PlayerController.ResetMovementsAndAttacks();

		ActionPhaseWindowView.turnsNumber.text = MainManager.Instance._ActionPhaseManager.CurrentTurn.ToString();
		ActionPhaseWindowView.RefreshCharacterInfo();
		_EnemyController.ResetEnemies();
	}

	public void GameOver()
	{
		isGameOver = true;
		ActionPhaseWindowView.DisableEnemyTurnText();
		_EnemyController.GameOver();

		ActionPhaseWindowView.EnableGameOverPanel();
	}

}
