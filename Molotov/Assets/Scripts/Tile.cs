using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public int gCost, hCost;
	public bool walkable;
	public Tile parent;
	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	void Start()
	{
		walkable = true;
	}
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
			walkable = false;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Enemy")
		{
			walkable = false;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Enemy" || other.tag == "Character")
		{
			isOccupiedByCharacter = false;
			isOccupiedByEnemy = false;			
			CharacterOnTile = null;
			walkable = true;	
		}
	}

	public List<Tile> GetNeighbours()
	{
		List<Tile> neighbours = new List<Tile>();

		for(int i = -1; i <= 1; i++)
		{
			for(int j = -1; j <= 1; j++)
			{
				if(i == 0 && j == 0)
					continue;
				
				int finalX = xCoord + i;
				int finalY = yCoord + j;

				int mapWidth = MainManager.Instance._MapGenerator.width;
				int mapHeight = MainManager.Instance._MapGenerator.height;
				
				if(finalX >= 0 && finalX <= mapWidth && finalY >= 0 && finalY <= mapHeight)
				{	
					var tile = MainManager.Instance._MapGenerator.Tiles.Find(_t => _t.xCoord == finalX && _t.yCoord == finalY);
					neighbours.Add(tile);
				}				
			}
		}

		return neighbours;
	}
}
