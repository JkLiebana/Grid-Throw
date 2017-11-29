using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public int width, height;
	public GameObject TileMap;
	public GameObject Player;

	public GameObject Map;
	

	void Start()
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

				tilePosition = new Vector3(i + (i* 0.1f), -1f, j + (j* 0.1f));
				cell.transform.position = tilePosition;
				cell.transform.SetParent(Map.transform, true);

			}
		}

		var player = Instantiate(Player, new Vector3(width/2, 0f, height/2), Quaternion.identity);
	}
}
