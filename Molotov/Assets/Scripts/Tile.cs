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

	[HideInInspector]
	public bool Alive = false;

	void Start()
	{
		walkable = true;
	}
	public int xCoord, yCoord;

	public Character CharacterOnTile;

	void OnMouseDown()
	{
		if(!MainManager.Instance._ActionPhaseManager.EnemyTurn)
			MainManager.Instance._ActionPhaseManager._PlayerController.SetNewTarget(gameObject.transform);
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Ground")
			return;

		if(other.tag != "Ground")
		{
			walkable = false;
		}	
		
		if(other.tag == "Character")
		{
			CharacterOnTile = other.gameObject.GetComponent<Character>();
		}
	}
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Enemy" || other.tag == "Character")
		{			
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

				int mapWidth = ActionPhaseManager.Instance._MapGenerator.width;
				int mapHeight = ActionPhaseManager.Instance._MapGenerator.height;
				
				if(finalX >= 0 && finalX <= mapWidth && finalY >= 0 && finalY <= mapHeight)
				{	
					var tile = ActionPhaseManager.Instance._MapGenerator.Tiles.Find(_t => _t.xCoord == finalX && _t.yCoord == finalY);
					neighbours.Add(tile);
				}				
			}
		}

		return neighbours;
	}

	public void Revive()
	{
		Alive = true;
		gameObject.SetActive(true);
	}

	public void Kill()
	{
		gCost = 0;
		hCost = 0;
		walkable = true;
		CharacterOnTile = null;
		xCoord = 0;
		yCoord = 0;

		Alive = false;
		gameObject.SetActive(false);
	}
}
