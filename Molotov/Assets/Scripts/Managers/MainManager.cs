using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>{


	public MapGenerator _MapGenerator;
	public UIManager _UIManager;

	public PlayerController _PlayerController;

	void Start()
	{
		_MapGenerator.Initialize();
		_UIManager.Initialize();
		_PlayerController.Initialize();
	}	


}
