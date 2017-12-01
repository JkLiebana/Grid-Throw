using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public string Name;
	public float speed;
	public int maxCellsMovement;

	public int movementsLeft;

	void OnMouseDown()
	{
		if(!MainManager.Instance.EnemyTurn)
			MainManager.Instance._PlayerController.SetCurrentCharacter(this);
	}

}
