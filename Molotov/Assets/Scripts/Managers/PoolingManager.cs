using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour {

#region Enemy Pooling Variables
 
 	[Header("Enemy Pooling")]
	public GameObject EnemyPrefab;
	public GameObject EnemyHealthBarPrefab;
	public GameObject EnemiesParent;
	public RectTransform EnemyHealthCanvas;
	private RectTransform HealthCanvas;
	public int EnemyPoolSize;

	[HideInInspector]
	public List<Enemy> EnemyPool;
#endregion

#region Character Pooling Variables

 	[Header("Character Pooling")]	
	public GameObject CharacterPrefab;
	public int CharacterPoolSize;

	[HideInInspector]
	public List<Character> CharacterPool;
#endregion

#region Weapons Pooling Variables

 	[Header("Weapons Pooling")]
	public GameObject WeaponsPrefab;
	public int WeaponsPoolSize;

	[HideInInspector]
	public List<Weapon> WeaponPool;

#endregion

#region Tiles Pooling Variables

 	[Header("Tiles Pooling")]
	public GameObject TilePrefab;
	public GameObject TilesParent;
	public int TilePoolSize;
	[HideInInspector]
	public List<Tile> TilesPool;

#endregion

	
	public void Initialize()
	{
		InitializeEnemies();
		InitializeCharacters();
		InitializeWeapons();
		InitializeTiles();
	}

#region Enemies
	private void InitializeEnemies()
	{
		HealthCanvas = Instantiate(EnemyHealthCanvas, Vector3.zero, Quaternion.identity);
		HealthCanvas.name = "Health Bars Canvas";
		HealthCanvas.transform.SetParent(MainManager.Instance._UIManager.transform);

		for(int i = 0; i < EnemyPoolSize; i++)
		{
			var enemyGameObject = Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity);
			var enemyHealthBar = Instantiate(EnemyHealthBarPrefab, Vector3.zero, Quaternion.identity);
			var enemy = enemyGameObject.GetComponent<Enemy>();
			
			enemyHealthBar.transform.SetParent(HealthCanvas);
			enemyHealthBar.GetComponent<UI_FollowEnemy>().objectToFollow = enemyGameObject.transform;
			enemyHealthBar.GetComponent<UI_FollowEnemy>()._myCanvas = HealthCanvas;

			enemy.EnemyHealth = enemyHealthBar.GetComponentInChildren<UnityEngine.UI.Slider>();		
			EnemyPool.Add(enemy);		

			enemyGameObject.transform.SetParent(EnemiesParent.transform);
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

	public void DespawnAllEnemies()
	{
		for(int i = 0; i < EnemyPool.Count; i++)
		{
			EnemyPool[i].Kill();
		}
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

	public void DespawnAllCharacters()
	{
		for(int i = 0; i < CharacterPool.Count; i++)
		{
			CharacterPool[i].Kill();
		}
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

#region Tiles

	public void InitializeTiles()
	{
		for(int i = 0; i < TilePoolSize; i++)
		{
			var tileGameObject = Instantiate(TilePrefab, Vector3.zero, Quaternion.identity);
			
			tileGameObject.transform.SetParent(TilesParent.transform);
			TilesPool.AddRange(tileGameObject.GetComponentsInChildren<Tile>());
			
			TilesPool.ForEach(delegate(Tile w){ w.Kill();});
		}
	}
	public Tile SpawnTile()
	{
		for(int i = 0; i < TilesPool.Count; i++)
		{
			if(!TilesPool[i].Alive)
			{
				var tile = TilesPool[i];
				tile.Revive();
				return tile;
			}		
		}
		return null;
	}

	public void DespawnTile(Tile tile)
	{
		tile.Kill();
	}

	public void DespawnAllTiles()
	{
		for(int i = 0; i < TilesPool.Count; i++)
		{
			TilesPool[i].Kill();
		}
	}

#endregion

}
