using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	void OnMouseDown()
	{
		MainManager.Instance._PlayerController.SetNewTarget(gameObject.transform);
	}
}
