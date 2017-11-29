using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text farText, closeText;

	void Start()
	{
		DisableCloseText();
		DisableFarText();
	}
	public void EnableFarText()
	{
		farText.enabled = true;
		StartCoroutine(Wait(0));
	}

	public void DisableFarText()
	{

		farText.enabled = false;
	}

	public void EnableCloseText()
	{
		closeText.enabled = true;
		StartCoroutine(Wait(1));		
	}

	public void DisableCloseText()
	{
		closeText.enabled = false;
	}

	IEnumerator Wait(int option)
	{
		yield return new WaitForSeconds(1);

		if(option == 0)
		{
			DisableFarText();
		}
		else{
			DisableCloseText();
		}
	}

}
