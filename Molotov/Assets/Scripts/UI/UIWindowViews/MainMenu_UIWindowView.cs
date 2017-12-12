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
		SceneManager.LoadScene(2, LoadSceneMode.Single);
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
		if(scene.buildIndex == 2)
		{
			ActionPhaseManager.Instance.Initialize();
			SceneManager.sceneLoaded -= OnSceneLoad;
			
		}
	}

}
