using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Actors/Enemy")]
public class EnemyInfo : ScriptableObject {

	public int Life;
	public int Movements;
	public int speed;
	public int Damage;
	public GameObject EnemyPrefab;

}
