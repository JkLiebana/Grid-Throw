using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>{


	public MapGenerator _MapGenerator;
	public UIManager _UIManager;
	public PlayerController _PlayerController;

	public int CurrentTurn = 0;

	public bool EnemyTurn = false;

	void Start()
	{
		_MapGenerator.Initialize();
		_UIManager.Initialize();
		_PlayerController.Initialize();
	}	


	public void StartEnemyTurn()
	{
		EnemyTurn = true;
		StartCoroutine(ProcessEnemyTurn());
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
	}


}
