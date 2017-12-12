using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public int width, height;
	public List<Tile> Tiles;
	

	public void Initialize()
	{
		Tiles = new List<Tile>();
		GenerateMap();
	}

	private Vector3 tilePosition;
	void GenerateMap()
	{
		for(int i = 0; i <= width; i++)
		{
			for(int j = 0; j <= height; j++)
			{
				var cell = MainManager.Instance._PoolingManager.SpawnTile();
				
				tilePosition = new Vector3(i, -1f, j);
				cell.gameObject.transform.position = tilePosition;

				cell.xCoord = i;
				cell.yCoord = j;			

				Tiles.Add(cell);
			}
		}
	}
}
