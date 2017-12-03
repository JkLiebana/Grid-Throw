using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public int xCoord, yCoord;
	public bool isOccupiedByCharacter, isOccupiedByEnemy;

	public Character CharacterOnTile;

	void OnMouseDown()
	{
		if(!MainManager.Instance.EnemyTurn)
			MainManager.Instance._PlayerController.SetNewTarget(gameObject.transform);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Character")
		{
			isOccupiedByCharacter = true;
			CharacterOnTile = other.gameObject.GetComponent<Character>();
		}
		else if(other.tag == "Enemy")
		{
			isOccupiedByEnemy = true;
		}
		
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag != "Ground")
		{
			isOccupiedByCharacter = false;
			isOccupiedByEnemy = false;			
			CharacterOnTile = null;
		}
	}
}
