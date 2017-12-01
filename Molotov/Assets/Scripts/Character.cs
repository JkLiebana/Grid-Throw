using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public string Name;
	public float speed;
	public int maxCellsMovement;

	void OnMouseDown()
	{
		MainManager.Instance._PlayerController.SetCurrentCharacter(this);
	}

}
