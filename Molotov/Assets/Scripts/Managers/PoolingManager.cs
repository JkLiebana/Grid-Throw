using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour {

#region Enemy Pooling Variables
	public GameObject EnemyPrefab, EnemyHealthBarPrefab;
	public RectTransform EnemyHealthCanvas;
	public int EnemyPoolSize;
	public List<Enemy> EnemyPool;
#endregion

#region Character Pooling Variables
	public GameObject CharacterPrefab;
	public int CharacterPoolSize;
	public List<Character> CharacterPool;
#endregion

	public void Initialize()
	{
		InitializeEnemies();
		InitializeCharacters();
		
	}

	private void InitializeEnemies()
	{
		for(int i = 0; i < EnemyPoolSize; i++)
		{
			var enemyGameObject = Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity);
			var enemy = enemyGameObject.GetComponent<Enemy>();
			
			EnemyPool.Add(enemy);
			enemyGameObject.transform.SetParent(this.gameObject.transform);
			enemyGameObject.name = "Enemy";
			

			var enemyHealthBar = Instantiate(EnemyHealthBarPrefab, Vector3.zero, Quaternion.identity);
			enemyHealthBar.transform.SetParent(EnemyHealthCanvas);

			enemyHealthBar.GetComponent<UI_FollowEnemy>().objectToFollow = enemyGameObject.transform;
			enemyHealthBar.GetComponent<UI_FollowEnemy>()._myCanvas = EnemyHealthCanvas;

			enemy.EnemyHealth = enemyHealthBar.GetComponentInChildren<UnityEngine.UI.Slider>();		

			enemyGameObject.GetComponent<Enemy>().Kill();
			
		}
	}

	private void InitializeCharacters()
	{
		for(int i = 0; i < CharacterPoolSize; i++)
		{
			var characterGameObject = Instantiate(CharacterPrefab, Vector3.zero, Quaternion.identity);
			characterGameObject.transform.SetParent(this.gameObject.transform);
			characterGameObject.GetComponent<Character>().Kill();
			CharacterPool.Add(characterGameObject.GetComponent<Character>());
		}
	}

	public Enemy SpawnEnemy(EnemyInfo enemyInfo)
	{
		for(int i = 0; i < EnemyPool.Count; i++)
		{
			if(!EnemyPool[i].Alive)
			{
				EnemyPool[i].Revive(enemyInfo);
				return EnemyPool[i];
			}	
		}

		var enemy = Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity).GetComponent<Enemy>();
		EnemyPool.Add(enemy);
		enemy.Revive(enemyInfo);

		return enemy;
	}

	public Character SpawnCharacter(CharacterInfo characterInfo)
	{
		for(int i = 0; i < CharacterPool.Count; i++)
		{
			if(!CharacterPool[i].Alive)
			{
				return CharacterPool[i].Revive(characterInfo);
			}
		}

		var newCharacter = Instantiate(CharacterPrefab, Vector3.zero, Quaternion.identity);
		var character = newCharacter.GetComponent<Character>();
		CharacterPool.Add(character);

		return character.Revive(characterInfo);
	}
	public void DespawnEnemy(Enemy enemy)
	{
		enemy.Kill();
	}

	public void DespawnCharacter(Character character)
	{
		character.Kill();
	}

}
