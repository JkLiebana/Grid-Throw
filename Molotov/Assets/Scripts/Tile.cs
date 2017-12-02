using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public int xCoord, yCoord;
	public bool isOccupied;

	void OnMouseDown()
	{
		if(!MainManager.Instance.EnemyTurn)
			MainManager.Instance._PlayerController.SetNewTarget(gameObject.transform);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Ground")
		{
			isOccupied = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag != "Ground")
		{
			isOccupied = false;
		}
	}
}
