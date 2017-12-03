using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Actors/Character")]
public class CharacterInfo : ScriptableObject {

	public GameObject CharacterPrefab;
	public Material Material;
	
	public string Name;
	public float Speed;
	public int Life;
	public int maxCellsMovement;
}
