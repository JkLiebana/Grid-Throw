using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
public class WeaponInfo : ScriptableObject {

	public string Name;
	public int Damage;
	public int minRange, maxRange;
	public int AttackCost;
}
