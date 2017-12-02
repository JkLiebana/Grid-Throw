using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public List<EnemyInfo> Enemies;
	public List<Enemy> AliveEnemies;

	public Enemy CurrentEnemy;

	private bool movingEnemy = false;

	public void Initialize()
	{
		SpawnInitialEnemies();
	}


	void SpawnInitialEnemies()
	{
		AliveEnemies = new List<Enemy>();
		Vector3 position = new Vector3(MainManager.Instance._MapGenerator.width, 0, MainManager.Instance._MapGenerator.height);
		for(int i = 0; i < Enemies.Count; i++)
		{
			position.z = i;
			
			var enemy = Instantiate(Enemies[i].EnemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
			
			enemy.speed = Enemies[i].speed;
			enemy.life = Enemies[i].Life;
			enemy.alreadyMoved = false;
			enemy.maxCellsMovement = Enemies[i].Movements;
			
			AliveEnemies.Add(enemy);
		}
	}

	void Update()
	{
		if(!MainManager.Instance.EnemyTurn)
			return;
		
		if(movingEnemy)
		{
			MoveEnemy();
		}
		else
		{
			SetCurrentEnemyMoving();
		}
	}

	private Vector3 positionWhileMovement;
	Vector3 currentTarget;
	void MoveEnemy()
	{	
		positionWhileMovement = Vector3.MoveTowards(CurrentEnemy.transform.position, currentTarget, CurrentEnemy.speed * Time.deltaTime);
		positionWhileMovement.y = 0;
		
		CurrentEnemy.transform.position = positionWhileMovement;
		CheckIfPositionReached();		
	}

	void CheckIfPositionReached()
	{
		Vector3 distance = CurrentEnemy.transform.position - currentTarget;
		distance.y = 0;

		if(distance.magnitude <= 0.01f)
		{	
			Vector3 finalPosition = currentTarget;
			finalPosition.y = 0;
			CurrentEnemy.gameObject.transform.position = finalPosition;
			CurrentEnemy.alreadyMoved = true;
			movingEnemy = false;
		}
	}

	public void ResetEnemies()
	{
		for(int i = 0; i < AliveEnemies.Count; i++)
		{
			AliveEnemies[i].alreadyMoved = false;
		}
	}
	void SetCurrentEnemyMoving()
	{
		for(int i = 0; i < Enemies.Count; i++)
		{
			if(!AliveEnemies[i].alreadyMoved)
			{
				CurrentEnemy = AliveEnemies[i];
				movingEnemy = true;
				SetNewTarget();
				
				return;
			}
		}
		MainManager.Instance.FinishEnemyTurn();
	}

	void SetNewTarget()
	{
		Vector3 playerPos = MainManager.Instance._PlayerController.CurrentCharacterSelected.transform.position;
		Vector3 temp = CurrentEnemy.transform.position - playerPos;
				
		temp.Normalize();
		temp.x = Mathf.RoundToInt(temp.x);
		temp.z = Mathf.RoundToInt(temp.z);
								
		currentTarget.x = CurrentEnemy.transform.position.x - (temp.x * CurrentEnemy.maxCellsMovement);
		currentTarget.z = CurrentEnemy.transform.position.z - (temp.z * CurrentEnemy.maxCellsMovement);
	}
}
