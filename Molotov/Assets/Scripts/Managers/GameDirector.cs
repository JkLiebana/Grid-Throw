using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour {


	public void LoadMenu()
	{
		SceneManager.LoadScene(1);
	}

	public void LoadMainAction()
	{
		SceneManager.LoadScene(2);
	}

}
