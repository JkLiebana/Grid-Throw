﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

#region Public Variables
	public List<EnemyInfo> Enemies;
	public Enemy CurrentEnemy;

#endregion

#region Private Variables
	private List<Enemy> AliveEnemies;
	private Vector3 CurrentTarget;
	private bool MovingEnemy = false;
	private bool EnemyAttacking = false;
	
#endregion	

	public void Initialize()
	{
		SpawnInitialEnemies();
	}

	void SpawnInitialEnemies()
	{
		AliveEnemies = new List<Enemy>();

		Vector3 EnemyPosition = new Vector3(MainManager.Instance._MapGenerator.width, 0, MainManager.Instance._MapGenerator.height);
		for(int i = 0; i < Enemies.Count; i++)
		{			
			var enemy = MainManager.Instance._PoolingManager.SpawnEnemy(Enemies[i]);

			EnemyPosition.z = i;				
			enemy.transform.position = EnemyPosition;

			AliveEnemies.Add(enemy);
		}
	}

	void Update()
	{
		if(!MainManager.Instance.EnemyTurn || MainManager.Instance.isGameOver)
			return;
		
		if(MovingEnemy)
		{
			MoveEnemy();
		}
		else if(EnemyAttacking)
		{
			CheckIfPossibleAttack();
		}
		else
		{
			SetCurrentEnemyMoving();
		}
	}


#region Movement
	private Vector3 positionWhileMovement;
	private void MoveEnemy()
	{	
		positionWhileMovement = Vector3.MoveTowards(CurrentEnemy.transform.position, CurrentTarget, CurrentEnemy.Speed * Time.deltaTime);
		positionWhileMovement.y = 0;
		
		CurrentEnemy.transform.position = positionWhileMovement;
		CheckIfPositionReached();		
	}

	private void CheckIfPositionReached()
	{
		Vector3 DistanceToTarget = CurrentEnemy.transform.position - CurrentTarget;
		DistanceToTarget.y = 0;

		if(DistanceToTarget.magnitude <= 0.01f)
		{	
			Vector3 finalPosition = CurrentTarget;
			finalPosition.y = 0;

			CurrentEnemy.gameObject.transform.position = finalPosition;
			CurrentEnemy.alreadyMoved = true;
			MovingEnemy = false;
			CheckIfPossibleAttack();
		}
	}
	
#endregion
#region Attack	

	private void CheckIfPossibleAttack()
	{
		var currentTile = MainManager.Instance._MapGenerator.Tiles.Find(t => t.xCoord == CurrentEnemy.transform.position.x && t.yCoord == CurrentEnemy.transform.position.z);

		var character = MainManager.Instance._PlayerController.CurrentCharacterSelected;
		var characterTile = MainManager.Instance._MapGenerator.Tiles.Find(t => t.xCoord == character.transform.position.x && t.yCoord == character.transform.position.z);

		if(currentTile.GetNeighbours().Contains(characterTile))
		{
			EnemyAttacking = true;
			Attack(characterTile);
		}
	}
	private void Attack(Tile characterTile)
	{		
		if(characterTile.isOccupiedByCharacter)
		{
			characterTile.CharacterOnTile.RecieveDamage(CurrentEnemy.Damage, characterTile);
		}
	
		EnemyAttacking = false;
	}
#endregion
#region Current Enemy on Movement selection
	private void SetCurrentEnemyMoving()
	{
		for(int i = 0; i < AliveEnemies.Count; i++)
		{
			if(!AliveEnemies[i].alreadyMoved)
			{
				CurrentEnemy = AliveEnemies[i];
				SetPathToTarget();				
				return;
			}
		}
		MainManager.Instance.FinishEnemyTurn();
	}


	private void SetPathToTarget()
	{
		Transform target = MainManager.Instance._PlayerController.CurrentCharacterSelected.transform;
		List<Tile> path = new List<Tile>();

		path = MainManager.Instance._PathfindingManager.FindPath(CurrentEnemy.transform, target);

		if(path.Count <= 0)
		{
			MovingEnemy = false;
			EnemyAttacking = true;
			CurrentEnemy.alreadyMoved = true;
			return;
		}
		CurrentTarget.x = path[path.Count - CurrentEnemy.maxCellsMovement].xCoord;
		CurrentTarget.z = path[path.Count - CurrentEnemy.maxCellsMovement].yCoord;

		MovingEnemy = true;
	}
	
#endregion
#region External Access
	public void ResetEnemies()
	{
		for(int i = 0; i < AliveEnemies.Count; i++)
		{
			AliveEnemies[i].alreadyMoved = false;
		}
	}

	public void GameOver()
	{
		EnemyAttacking = false;
		MovingEnemy = false;
	}

	public void EnemyKilled(Enemy enemy)
	{
		AliveEnemies.Remove(enemy);
		if(AliveEnemies.Count <= 0)
		{
			MainManager.Instance.GameOver();
		}
	}
#endregion

}
