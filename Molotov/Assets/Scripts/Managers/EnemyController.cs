using System.Collections;
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
			EnemyPosition.z = i;
			
			var enemy = Instantiate(Enemies[i].EnemyPrefab, EnemyPosition, Quaternion.identity).GetComponent<Enemy>();
			
			enemy.Speed = Enemies[i].Speed;
			enemy.Life = Enemies[i].Life;
			enemy.Damage = Enemies[i].Damage;
			enemy.maxCellsMovement = Enemies[i].Movements;
			enemy.alreadyMoved = false;
			
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
			Attack();
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
			EnemyAttacking = true;
		}
	}
	
#endregion
#region Attack	
	private void Attack()
	{
		var tile = MainManager.Instance._MapGenerator.Tiles.Find(t => t.xCoord == CurrentTarget.x && t.yCoord == CurrentTarget.z);
		
		if(tile.isOccupiedByCharacter)
		{
			tile.CharacterOnTile.RecieveDamage(CurrentEnemy.Damage, tile);
		}
	
		EnemyAttacking = false;
	}
#endregion
#region Current Enemy on Movement selection
	private void SetCurrentEnemyMoving()
	{
		for(int i = 0; i < Enemies.Count; i++)
		{
			if(!AliveEnemies[i].alreadyMoved)
			{
				CurrentEnemy = AliveEnemies[i];
				MovingEnemy = true;
				SetNewTarget();
				
				return;
			}
		}
		MainManager.Instance.FinishEnemyTurn();
	}

	private void SetNewTarget()
	{
		EnemyAttacking = true;
		Vector3 CurrentCharacterSelected = MainManager.Instance._PlayerController.CurrentCharacterSelected.transform.position;
		Vector3 DistanceToCharacter = CurrentEnemy.transform.position - CurrentCharacterSelected;
				
		DistanceToCharacter.Normalize();
		DistanceToCharacter.x = Mathf.RoundToInt(DistanceToCharacter.x);
		DistanceToCharacter.z = Mathf.RoundToInt(DistanceToCharacter.z);

		CurrentTarget.x = CurrentEnemy.transform.position.x - (DistanceToCharacter.x * CurrentEnemy.maxCellsMovement);
		CurrentTarget.z = CurrentEnemy.transform.position.z - (DistanceToCharacter.z * CurrentEnemy.maxCellsMovement);

		var TargetTile = MainManager.Instance._MapGenerator.Tiles.Find(tile => tile.xCoord == CurrentTarget.x && tile.yCoord == CurrentTarget.z);

		if(TargetTile.isOccupiedByCharacter || TargetTile.isOccupiedByEnemy)
		{
			MovingEnemy = false;
			EnemyAttacking = true;
			CurrentEnemy.alreadyMoved = true;
			return;
		}	
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
