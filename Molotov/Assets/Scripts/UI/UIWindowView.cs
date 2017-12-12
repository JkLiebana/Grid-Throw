using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowView : MonoBehaviour {

    public string Name;

    public virtual void InitializeView(string name)
    {
        Name = name;
        gameObject.SetActive(false);
    }

    public virtual void EnableView()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisableView()
    {
        gameObject.SetActive(false);
    }
}
