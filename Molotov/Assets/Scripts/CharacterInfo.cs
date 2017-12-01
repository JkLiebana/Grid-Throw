using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Actors/Character")]
public class CharacterInfo : ScriptableObject {

	public string Name;
	public GameObject CharacterPrefab;
	public int maxCellsMovement;
	public float speed;
	public Material Material;
}
