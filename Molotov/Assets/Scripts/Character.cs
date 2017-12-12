using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public float Speed;
	public int Life;
	public int Damage;
	public int MaxAttacksPerTurn;
	public int CurrentAttacksPerTurn;
	public int maxCellsMovement;
	public int movementsLeft;
	public WeaponInfo CurrentWeaponSelected;

	[HideInInspector]
	public List<WeaponInfo> Weapons;
	public bool Alive = false;

	void OnMouseDown()
	{
		if(!MainManager.Instance._ActionPhaseManager.EnemyTurn)
			MainManager.Instance._ActionPhaseManager._PlayerController.SetCurrentCharacter(this);
	}

	public void RecieveDamage(int damageRecieved, Tile tile)
	{
		if(Life - damageRecieved <= 0)
		{
			Life = 0;
			MainManager.Instance._ActionPhaseManager._PlayerController.AliveCharacters.Remove(this);
			tile.CharacterOnTile = null;
			//tile.isOccupiedByCharacter = false;
			tile.walkable = true;

			if(MainManager.Instance._ActionPhaseManager._PlayerController.AliveCharacters.Count <= 0)
			{
				MainManager.Instance._ActionPhaseManager.GameOver();
				MainManager.Instance._PoolingManager.DespawnCharacter(this);
				return;
			}
			
			MainManager.Instance._ActionPhaseManager._PlayerController.CurrentCharacterSelected = MainManager.Instance._ActionPhaseManager._PlayerController.AliveCharacters[0];
			MainManager.Instance._PoolingManager.DespawnCharacter(this);
					}
		else
		{
			Life -= damageRecieved;		
		}
	}

	public void Kill()
	{
		Alive = false;
		gameObject.SetActive(false);
	}

	public void Revive(CharacterInfo characterInfo)
	{
		gameObject.name = characterInfo.Name;

		gameObject.GetComponent<Renderer>().material = characterInfo.Material;

		Speed = characterInfo.Speed;
		Life = characterInfo.Life;
		MaxAttacksPerTurn = characterInfo.MaxAttacksPerTurn;
		CurrentAttacksPerTurn = MaxAttacksPerTurn;
		maxCellsMovement = characterInfo.maxCellsMovement;
		movementsLeft = characterInfo.maxCellsMovement;
		
		Weapons.AddRange(characterInfo.Weapons);
		CurrentWeaponSelected = Weapons[0];
		
		Alive = true;
		gameObject.SetActive(true);
	}
}
