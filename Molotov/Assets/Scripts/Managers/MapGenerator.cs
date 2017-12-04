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
				var cellGameObject = Instantiate(TileMap);
				var cell = cellGameObject.GetComponent<Tile>();
				
				tilePosition = new Vector3(i, -1f, j);
				cellGameObject.transform.position = tilePosition;
				cellGameObject.transform.SetParent(Map.transform, true);

				cell.xCoord = i;
				cell.yCoord = j;			

				Tiles.Add(cell);
			}
		}
	}
}
