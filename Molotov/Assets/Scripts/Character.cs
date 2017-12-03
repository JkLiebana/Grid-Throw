using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public float Speed;
	public int Life;
	public int Damage;
	public int maxCellsMovement;
	public int movementsLeft;

	void OnMouseDown()
	{
		if(!MainManager.Instance.EnemyTurn)
			MainManager.Instance._PlayerController.SetCurrentCharacter(this);
	}

	public void RecieveDamage(int damageRecieved, Tile tile)
	{
		if(Life - damageRecieved <= 0)
		{
			Life = 0;
			MainManager.Instance._PlayerController.AliveCharacters.Remove(this);
			tile.CharacterOnTile = null;
			tile.isOccupiedByCharacter = false;

			if(MainManager.Instance._PlayerController.AliveCharacters.Count <= 0)
			{
				MainManager.Instance.GameOver();
				Destroy(this.gameObject);
				return;
			}
			
			MainManager.Instance._PlayerController.CurrentCharacterSelected = MainManager.Instance._PlayerController.AliveCharacters[0];
			Destroy(this.gameObject);
		}
		else
		{
			Life -= damageRecieved;		
		}
	}
}
