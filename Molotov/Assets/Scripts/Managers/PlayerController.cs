using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

#region Public Variables
	public GameObject MolotovPrefab;
	public bool movingCharacter = false;
	public bool throwingMolotov = false;
	
	public Character CurrentCharacterSelected;
	public List<CharacterInfo> AvailableCharacters; 

	[HideInInspector]
	public List<Character> AliveCharacters;

#endregion
#region Private Variables
	private Transform currentCharacterTarget;
	private GameObject MolotovInstance;
#endregion
	
	public void Initialize()
	{
		AliveCharacters = new List<Character>();
		InstantiateCharacters();		
	}

	private void InstantiateCharacters()
	{
		Character character;

		for(int i = 0; i < AvailableCharacters.Count; i++)
		{
			var newCharacter = Instantiate(AvailableCharacters[i].CharacterPrefab, Vector3.zero, Quaternion.identity);
			
			newCharacter.transform.position = new Vector3(0, 0, i);
			newCharacter.name = AvailableCharacters[i].name;

			newCharacter.GetComponent<Renderer>().material = AvailableCharacters[i].Material;

			character = newCharacter.GetComponent<Character>();
			character.Speed = AvailableCharacters[i].Speed;
			character.Life = AvailableCharacters[i].Life;
			character.Damage = AvailableCharacters[i].Damage;
			character.maxCellsMovement = AvailableCharacters[i].maxCellsMovement;
			character.movementsLeft = AvailableCharacters[i].maxCellsMovement;
			
			AliveCharacters.Add(character);
		}

		CurrentCharacterSelected = AliveCharacters[0];
		MainManager.Instance._UIManager.RefreshCharacterInfo();
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if(MainManager.Instance.EnemyTurn || MainManager.Instance.isGameOver)
			return;

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
	private void Movement()
	{
		if(!movingCharacter)
			return;
		
		positionWhileMovement = Vector3.MoveTowards(CurrentCharacterSelected.transform.position, currentCharacterTarget.position, CurrentCharacterSelected.Speed * Time.deltaTime);
		positionWhileMovement.y = 0;
		
		CurrentCharacterSelected.transform.position = positionWhileMovement;
		CheckIfPositionReached();
	}

#region Current Character and Target Setting - External Access

	public void SetCurrentCharacter(Character character)
	{
		if(movingCharacter)
			return;

		CurrentCharacterSelected = character;
		MainManager.Instance._UIManager.RefreshCharacterInfo();
	}

	public void SetNewTarget(Transform target)
	{
		if(CheckIfTargetOccupied(target.position))
		{
			MainManager.Instance._UIManager.EnableOccupiedText();
			return;
		}

		Vector3 distanceCharacterToTarget = target.position - CurrentCharacterSelected.transform.position;
		distanceCharacterToTarget.y = 0;
		var finalDistanceMagnitude = (int) Mathf.Abs(distanceCharacterToTarget.magnitude);

		if(!CanCharacterMoveOnCurrentTurn(finalDistanceMagnitude))
			return;

		if(finalDistanceMagnitude <= CurrentCharacterSelected.maxCellsMovement)
		{
			AssignTargetAndStartMovement(target, finalDistanceMagnitude);
			
		}
	}

	public void ResetMovementsAndThrows()
	{
		for(int i = 0; i < AliveCharacters.Count; i++)
		{
			AliveCharacters[i].movementsLeft = AliveCharacters[i].maxCellsMovement;
			AliveCharacters[i].MaxThrows = 1;
		}
	}
#endregion
	
#region Internal Movement Utilities
	private bool CheckIfTargetOccupied(Vector3 possibleTargetPosition)
	{
		var possibleTargetTile = MainManager.Instance._MapGenerator.Tiles.Find(tile => tile.xCoord == possibleTargetPosition.x && tile.yCoord == possibleTargetPosition.z);
		return possibleTargetTile.isOccupiedByCharacter || possibleTargetTile.isOccupiedByEnemy;
	}

	private bool CanCharacterMoveOnCurrentTurn(int distance)
	{
		return CurrentCharacterSelected.movementsLeft - distance >= 0;
	}

	private void AssignTargetAndStartMovement(Transform target, int finalDistanceMagnitude)
	{
		currentCharacterTarget = target;
		CurrentCharacterSelected.movementsLeft -= finalDistanceMagnitude;
		
		movingCharacter = true;

		MainManager.Instance._UIManager.EnableMovingText();		
		MainManager.Instance._UIManager.RefreshCharacterInfo();		
	}

	private void CheckIfPositionReached()
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

#endregion

#region Internal Attack Utilities
	private void CheckIfPossibleLaunch(Transform target)
	{
		float distance = Vector3.Distance(target.position, CurrentCharacterSelected.transform.position);
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
			if(CurrentCharacterSelected.MaxThrows > 0)
			{
				ThrowMolotov(target);
				CurrentCharacterSelected.MaxThrows--;			
			}
		}
	}

	private void ThrowMolotov(Transform target)
	{
		throwingMolotov = true;
		MolotovInstance = Instantiate(MolotovPrefab, CurrentCharacterSelected.transform.position, Quaternion.identity);
		MolotovInstance.GetComponent<Molotov>().Initialize(target, CurrentCharacterSelected.Damage);
	}
#endregion

}
