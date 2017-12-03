using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour {

	public GameObject FirePS;
	public Transform target;
	public float speed = 2;
	public float throwAngle = 45f;
	public float gravity = 9.8f;

	private int Damage;

	public Material cellDestroyedMaterial;
	

	public void Initialize(Transform _target, int characterDamage)
	{
		target = _target;
		Damage = characterDamage;
		StartCoroutine(ThrowMolotov());
	}

	IEnumerator ThrowMolotov()
    {
        yield return new WaitForSeconds(0.05f);
              
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float molotovInitialSpeed = Mathf.Sqrt((gravity * distanceToTarget) / (Mathf.Sin(2 * throwAngle * Mathf.Deg2Rad)));
 

        float Vx = molotovInitialSpeed * Mathf.Cos(throwAngle * Mathf.Deg2Rad);
        float Vy = molotovInitialSpeed * Mathf.Sin(throwAngle * Mathf.Deg2Rad);
 
        float flightTime = distanceToTarget / Vx;
   
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
       
        float timeFlying = 0;
 
        while (timeFlying < flightTime)
        {
            transform.Translate(0, (Vy - (gravity * timeFlying)) * Time.deltaTime, Vx * Time.deltaTime);
           
            timeFlying += Time.deltaTime;
 
            yield return null;
        }

		StartCoroutine(DestroyMolotov());
    }  

	IEnumerator DestroyMolotov()
	{
		yield return new WaitForSeconds(0.5f);
		target.gameObject.GetComponent<Renderer>().material = cellDestroyedMaterial;
		
		if(target.gameObject.GetComponent<Enemy>())
		{
			target.gameObject.GetComponent<Enemy>().RecieveDamage(Damage);
		}

		Destroy(this.gameObject);
	}
}
