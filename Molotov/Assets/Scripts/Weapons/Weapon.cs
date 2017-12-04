using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	[HideInInspector]
	public int Damage;

	public abstract void Initialize(Transform target, int damage);

	public virtual IEnumerator DestroyWeapon()
	{
		yield return new WaitForSeconds(2);
		MainManager.Instance._PlayerController.characterAttacking = false;

		Destroy(this.gameObject);
	}
}
