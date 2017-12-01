using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	void OnMouseDown()
	{
		if(!MainManager.Instance.EnemyTurn)
			MainManager.Instance._PlayerController.SetNewTarget(gameObject.transform);
	}
}
