using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : Weapon {

	public override void Initialize(Transform target, int damage)
	{
		if(target.GetComponent<Enemy>())
		{
			target.GetComponent<Enemy>().RecieveDamage(damage);
		}

		StartCoroutine(DestroyWeapon());
	}
}
