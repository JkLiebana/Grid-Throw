using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

#region Public Variables
	public GameObject MolotovPrefab;
	public bool movingCharacter = false;
	public bool characterAttacking = false;
	
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
		for(int i = 0; i < AvailableCharacters.Count; i++)
		{
			var newCharacter = MainManager.Instance._PoolingManager.SpawnCharacter(AvailableCharacters[i]);
		
			newCharacter.transform.position = new Vector3(0, 0, i);			
			AliveCharacters.Add(newCharacter);
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
				CheckIfPossibleAttack(hit.transform);
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

	public void ResetMovementsAndAttacks()
	{
		for(int i = 0; i < AliveCharacters.Count; i++)
		{
			AliveCharacters[i].movementsLeft = AliveCharacters[i].maxCellsMovement;
			AliveCharacters[i].CurrentAttacksPerTurn = AliveCharacters[i].MaxAttacksPerTurn;
		}
	}
#endregion

#region Weapon Management

	public void ChangeWeapon()
	{
		var currentIndexOf =  CurrentCharacterSelected.Weapons.IndexOf( CurrentCharacterSelected.CurrentWeaponSelected);
		var nextWeapon =  CurrentCharacterSelected.Weapons[(currentIndexOf + 1) ==  CurrentCharacterSelected.Weapons.Count ? 0 : (currentIndexOf + 1)];
		
		CurrentCharacterSelected.CurrentWeaponSelected = nextWeapon;
	}

#endregion

#region Internal Movement Utilities
	private bool CheckIfTargetOccupied(Vector3 possibleTargetPosition)
	{
		var possibleTargetTile = MainManager.Instance._MapGenerator.Tiles.Find(tile => tile.xCoord == possibleTargetPosition.x && tile.yCoord == possibleTargetPosition.z);
		return !possibleTargetTile.walkable;
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

	private void CheckIfPossibleAttack(Transform target)
	{
		Vector3 distance = CurrentCharacterSelected.transform.position - target.position;
		distance.y = 0;

		if(distance.magnitude < CurrentCharacterSelected.CurrentWeaponSelected.minRange)
		{
			MainManager.Instance._UIManager.EnableCloseText();			
		}
		else if(distance.magnitude >= CurrentCharacterSelected.CurrentWeaponSelected.maxRange)
		{
			MainManager.Instance._UIManager.EnableFarText();			
		}
		else if(CurrentCharacterSelected.CurrentAttacksPerTurn >= CurrentCharacterSelected.CurrentWeaponSelected.AttackCost)
		{
			Attack(target);
		}
	}

	private void Attack(Transform target)
	{
		CurrentCharacterSelected.CurrentAttacksPerTurn -= CurrentCharacterSelected.CurrentWeaponSelected.AttackCost;

		var weapon = MainManager.Instance._PoolingManager.SpawnWeapon(CurrentCharacterSelected.CurrentWeaponSelected);
		weapon.gameObject.transform.position = CurrentCharacterSelected.transform.position;
		weapon.Initialize(target, CurrentCharacterSelected.CurrentWeaponSelected.Damage);
		characterAttacking = true;
	}

#endregion

}
