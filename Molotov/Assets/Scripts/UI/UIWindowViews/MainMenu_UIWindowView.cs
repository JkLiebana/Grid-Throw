using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UIWindowView : UIWindowView {


	AsyncOperation asyncLoad;
	[SerializeField]
	public Object NextScene;
	public UIWindowController NextWindowController;
	public void OnRaidButtonPressed()
	{
		SceneManager.sceneLoaded += OnSceneLoad;
		MainManager.Instance._UIManager.GoToWindow(NextWindowController);		
		asyncLoad = SceneManager.LoadSceneAsync(NextScene.name, LoadSceneMode.Single);
	}

	IEnumerator LoadSceneAsync()
	{
		while(!asyncLoad.isDone)
		{
			yield return null;
		}
		
	}

	void OnSceneLoad(Scene scene, LoadSceneMode mode)
	{
		if(scene.name == NextScene.name)
		{
			ActionPhaseManager.Instance.Initialize();
			SceneManager.sceneLoaded -= OnSceneLoad;
			
		}
	}

}
