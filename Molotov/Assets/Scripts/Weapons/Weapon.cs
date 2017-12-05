using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	
	public string Name;
	[HideInInspector]
	public int Damage;
	[HideInInspector]
	public Transform Target;
	[HideInInspector]
	public bool Alive = false;
	public virtual void Initialize(Transform target, int damage)
	{
		Target = target;
		Damage = damage;
	}

	public virtual IEnumerator DestroyWeapon()
	{
		yield return new WaitForSeconds(2);
		MainManager.Instance._PlayerController.characterAttacking = false;

		MainManager.Instance._PoolingManager.DespawnWeapon(this);
	}

	public void Kill()
	{
		Alive = false;
		gameObject.SetActive(false);
	}

	public void Revive()
	{
		Alive = true;
		gameObject.SetActive(true);
	}
}
