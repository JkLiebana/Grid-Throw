using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject MolotovPrefab, MolotovInstance;
	public bool movingCharacter = false, throwingMolotov = false;
	public float speed = 4f;
	public Transform currentCharacterTarget;

	public UIManager _UIManager;

	void Start()
	{
		_UIManager = GameObject.FindObjectOfType<UIManager>();		
	}

	void Update () {

		Movement();

		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
        	
			if (Physics.Raycast(ray, out hit, 100))
			{
				currentCharacterTarget = hit.transform;
				movingCharacter = true;
			}	
		}

		if(Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
        	
			if (Physics.Raycast(ray, out hit, 100))
			{
				CheckIfPossibleLaunch(hit.transform);
			}	
		}
	}

	private Vector3 positionWhileMovement;
	void Movement()
	{
		if(!movingCharacter)
			return;
		
		positionWhileMovement = Vector3.MoveTowards(transform.position, currentCharacterTarget.position, speed * Time.deltaTime);
		positionWhileMovement.y = 0;
		transform.position = positionWhileMovement;
	}

	void CheckIfPossibleLaunch(Transform cell)
	{
		float distance = Vector3.Distance(cell.position, this.transform.position);
		if(distance <= 2)
		{
			_UIManager.EnableCloseText();
		}
		else if(distance >= 5)
		{
			_UIManager.EnableFarText();
		}
		else
		{
			ThrowMolotov(cell);
		}
	}
	void ThrowMolotov(Transform molotov)
	{
		MolotovInstance = Instantiate(MolotovPrefab, transform.position, Quaternion.identity);
		MolotovInstance.GetComponent<Molotov>().cell = molotov.gameObject;
	}
}
