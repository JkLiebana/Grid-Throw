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

#region Weapons Pooling Variables

	public GameObject WeaponsPrefab;
	public int WeaponsPoolSize;
	public List<Weapon> WeaponPool;

#endregion

	
	public void Initialize()
	{
		InitializeEnemies();
		InitializeCharacters();
		InitializeWeapons();
	}

#region Enemies
	private void InitializeEnemies()
	{
		for(int i = 0; i < EnemyPoolSize; i++)
		{
			var enemyGameObject = Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity);
			var enemyHealthBar = Instantiate(EnemyHealthBarPrefab, Vector3.zero, Quaternion.identity);
			var enemy = enemyGameObject.GetComponent<Enemy>();
			
			enemyHealthBar.transform.SetParent(EnemyHealthCanvas);
			enemyHealthBar.GetComponent<UI_FollowEnemy>().objectToFollow = enemyGameObject.transform;
			enemyHealthBar.GetComponent<UI_FollowEnemy>()._myCanvas = EnemyHealthCanvas;

			enemy.EnemyHealth = enemyHealthBar.GetComponentInChildren<UnityEngine.UI.Slider>();		
			EnemyPool.Add(enemy);		

			enemyGameObject.transform.SetParent(this.gameObject.transform);
			enemyGameObject.name = "Enemy";
			enemyGameObject.GetComponent<Enemy>().Kill();
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

		enemy.Revive(enemyInfo);		
		EnemyPool.Add(enemy);

		return enemy;
	}

	public void DespawnEnemy(Enemy enemy)
	{
		enemy.Kill();
	}
#endregion

#region Characters
	private void InitializeCharacters()
	{
		for(int i = 0; i < CharacterPoolSize; i++)
		{
			var characterGameObject = Instantiate(CharacterPrefab, Vector3.zero, Quaternion.identity);
			var character = characterGameObject.GetComponent<Character>();

			characterGameObject.transform.SetParent(this.gameObject.transform);
			
			character.Kill();
			CharacterPool.Add(character);
		}
	}
	public Character SpawnCharacter(CharacterInfo characterInfo)
	{
		for(int i = 0; i < CharacterPool.Count; i++)
		{
			if(!CharacterPool[i].Alive)
			{
				CharacterPool[i].Revive(characterInfo);
				return CharacterPool[i];
			}
		}

		var newCharacter = Instantiate(CharacterPrefab, Vector3.zero, Quaternion.identity);
		var character = newCharacter.GetComponent<Character>();
		
		CharacterPool.Add(character);
		character.Revive(characterInfo);
		
		return character;
	}

	public void DespawnCharacter(Character character)
	{
		character.Kill();
	}
#endregion

#region Weapons


	public void InitializeWeapons()
	{
		for(int i = 0; i < WeaponsPoolSize; i++)
		{
			var weaponGameObject = Instantiate(WeaponsPrefab, Vector3.zero, Quaternion.identity);
			
			weaponGameObject.transform.SetParent(transform);
			WeaponPool.AddRange(weaponGameObject.GetComponentsInChildren<Weapon>());
			
			WeaponPool.ForEach(delegate(Weapon w){ w.Kill();});
		}
	}
	public Weapon SpawnWeapon(WeaponInfo weaponInfo)
	{
		for(int i = 0; i < WeaponPool.Count; i++)
		{
			if(WeaponPool[i].Name == weaponInfo.Name)
			{
				var weapon = WeaponPool[i];
				weapon.Revive();
				return weapon;
			}		
		}
		return null;
	}

	public void DespawnWeapon(Weapon weapon)
	{
		weapon.Kill();
	}


#endregion

	

	

}
