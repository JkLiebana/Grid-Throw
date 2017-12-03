using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int Speed;
	public int Life;
	public int Damage;
	public int maxCellsMovement;
	public bool alreadyMoved;

	public UnityEngine.UI.Slider EnemyHealth;

	public void RecieveDamage(int Damage)
	{
		if(Life - Damage <= 0)
		{
			MainManager.Instance._EnemyController.EnemyKilled(this);
			Destroy(EnemyHealth.transform.parent.gameObject);
			Destroy(this.gameObject);
		}

		Life -= Damage;
		EnemyHealth.value = Life;		
	}
}
