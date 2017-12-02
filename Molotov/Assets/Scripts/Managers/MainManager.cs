using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>{


	public MapGenerator _MapGenerator;
	public UIManager _UIManager;
	public PlayerController _PlayerController;
	public EnemyController _EnemyController;

	public int CurrentTurn = 0;

	public bool EnemyTurn = false;

	void Start()
	{
		_MapGenerator.Initialize();
		_UIManager.Initialize();
		_PlayerController.Initialize();
		_EnemyController.Initialize();
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
		_UIManager.DisableEnemyTurnText();
		_PlayerController.ResetMovements();

		_UIManager.turnsNumber.text = MainManager.Instance.CurrentTurn.ToString();
		_UIManager.RefreshCharacterInfo();
		_EnemyController.ResetEnemies();
	}


}
