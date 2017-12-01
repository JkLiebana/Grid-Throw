using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject MolotovPrefab, MolotovInstance;
	public bool movingCharacter = false, throwingMolotov = false;
	public Transform currentCharacterTarget;
	public Character CurrentCharacterSelected;

	public List<CharacterInfo> AvailableCharacters; 

	private List<Character> AliveCharacters;

	public void Initialize()
	{
		AliveCharacters = new List<Character>();
		for(int i = 0; i < AvailableCharacters.Count; i++)
		{
			var newCharacter = Instantiate(AvailableCharacters[i].CharacterPrefab, Vector3.zero, Quaternion.identity);
			
			newCharacter.gameObject.transform.position = new Vector3(0, 0, i);
			newCharacter.name = AvailableCharacters[i].name;

			newCharacter.GetComponent<Renderer>().material = AvailableCharacters[i].Material;
			newCharacter.GetComponent<Character>().speed = AvailableCharacters[i].speed;
			newCharacter.GetComponent<Character>().maxCellsMovement = AvailableCharacters[i].maxCellsMovement;
			newCharacter.GetComponent<Character>().movementsLeft = AvailableCharacters[i].maxCellsMovement;
			
			newCharacter.GetComponent<Character>().name = AvailableCharacters[i].name;
			

			AliveCharacters.Add(newCharacter.GetComponent<Character>());
		}

		CurrentCharacterSelected = AliveCharacters[0];

		MainManager.Instance._UIManager.RefreshCharacterInfo();
	}

	public void SetNewTarget(Transform target)
	{

		if(!CanCharacterMoveOnCurrentTurn())
		{
			return;
		}

		Vector3 distanceCharacterToTarget = target.position - CurrentCharacterSelected.transform.position;
		distanceCharacterToTarget.y = 0;
		var finalDistanceMagnitude = (int) Mathf.Abs(distanceCharacterToTarget.magnitude);

		if(finalDistanceMagnitude <= CurrentCharacterSelected.maxCellsMovement)
		{
			currentCharacterTarget = target;
			movingCharacter = true;
			MainManager.Instance._UIManager.EnableMovingText();
			CurrentCharacterSelected.movementsLeft -= finalDistanceMagnitude;
			MainManager.Instance._UIManager.RefreshCharacterInfo();		
		}
	}

	bool CanCharacterMoveOnCurrentTurn()
	{
		return CurrentCharacterSelected.movementsLeft > 0;
	}

	public void ResetMovements()
	{
		for(int i = 0; i < AliveCharacters.Count; i++)
		{
			AliveCharacters[i].movementsLeft = AliveCharacters[i].maxCellsMovement;
		}
	}

	public void SetCurrentCharacter(Character character)
	{
		if(movingCharacter)
			return;

		CurrentCharacterSelected = character;
		MainManager.Instance._UIManager.RefreshCharacterInfo();
	}

	void Update () {

		Movement();

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
		
		positionWhileMovement = Vector3.MoveTowards(CurrentCharacterSelected.transform.position, currentCharacterTarget.position, CurrentCharacterSelected.speed * Time.deltaTime);
		positionWhileMovement.y = 0;
		
		CurrentCharacterSelected.transform.position = positionWhileMovement;
		CheckIfPositionReached();
	}

	void CheckIfPositionReached()
	{
		Vector3 distance = CurrentCharacterSelected.transform.position - currentCharacterTarget.transform.position;
		distance.y = 0;

		if(distance.magnitude <= 0.01f)
		{	
			Vector3 finalPosition = currentCharacterTarget.transform.position;
			finalPosition.y = 0;
			CurrentCharacterSelected.gameObject.transform.position = finalPosition;
			movingCharacter = false;
			MainManager.Instance._UIManager.DisableMovingText();
		}
	}

	void CheckIfPossibleLaunch(Transform cell)
	{
		float distance = Vector3.Distance(cell.position, CurrentCharacterSelected.transform.position);
		if(distance <= 2)
		{
			MainManager.Instance._UIManager.EnableCloseText();
		}
		else if(distance >= 5)
		{
			MainManager.Instance._UIManager.EnableFarText();
		}
		else
		{
			ThrowMolotov(cell);
		}
	}
	void ThrowMolotov(Transform molotov)
	{
		MolotovInstance = Instantiate(MolotovPrefab, CurrentCharacterSelected.transform.position, Quaternion.identity);
		MolotovInstance.GetComponent<Molotov>().cell = molotov.gameObject;
	}
}
