using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public List<UIWindowController> Windows;
	public Canvas MainCanvas;

	private List<UIWindowView> InstantiatedWindows;

	private Canvas MainCanvasInstance;
	public UIWindowController CurrentWindow;

	public void Initialize()
	{
		InstantiatedWindows = new List<UIWindowView>();

		InstantiateAllWindows();
		for(int i = 0; i < InstantiatedWindows.Count; i++)
		{
			InstantiatedWindows[i].InitializeView(Windows[i].name);
		}

		InstantiatedWindows[0].EnableView();
		CurrentWindow = Windows[0];
	}

	private void InstantiateAllWindows()
	{
		MainCanvasInstance = Instantiate(MainCanvas, Vector3.zero, Quaternion.identity);
		MainCanvasInstance.transform.SetParent(gameObject.transform);
		MainCanvasInstance.name = "MainCanvas";

		for(int i = 0; i < Windows.Count; i++)
		{
			var window = Instantiate(Windows[i].WindowView.gameObject, Vector3.zero, Quaternion.identity);
			window.transform.SetParent(MainCanvasInstance.transform, false);
			window.name = Windows[i].Name;
			InstantiatedWindows.Add(window.GetComponent<UIWindowView>());
		}
	}

	public void GoToWindow(UIWindowController nextWindow)
	{
		CloseAllWindows();
		UIWindowView view = InstantiatedWindows.Find(w => nextWindow.Name == w.name);
		view.EnableView();
		CurrentWindow = nextWindow;
	}

	private void CloseAllWindows()
	{
		for(int i = 0; i < Windows.Count; i++)
		{
			InstantiatedWindows[i].DisableView();
		}
	}
}
