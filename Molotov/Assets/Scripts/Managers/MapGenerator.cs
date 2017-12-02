using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public int width, height;
	public GameObject TileMap;
	public GameObject Map;

	public List<Tile> Tiles;
	

	public void Initialize()
	{
		GenerateMap();
	}

	private Vector3 tilePosition;
	void GenerateMap()
	{
		for(int i = 0; i <= width; i++)
		{
			for(int j = 0; j <= height; j++)
			{

				var cell = Instantiate(TileMap);

				tilePosition = new Vector3(i, -1f, j);
				cell.transform.position = tilePosition;
				cell.transform.SetParent(Map.transform, true);

				cell.GetComponent<Tile>().xCoord = i;
				cell.GetComponent<Tile>().yCoord = j;			

				Tiles.Add(cell.GetComponent<Tile>());
			}
		}
	}
}
