using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager> {

	public PoolingManager _PoolingManager;
	public UIManager _UIManager;
	public GameDirector _GameDirector;


	public ActionPhaseManager _ActionPhaseManager;

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);

		_UIManager.Initialize();
		_PoolingManager.Initialize();
		_GameDirector.LoadMenu();

	}

}
