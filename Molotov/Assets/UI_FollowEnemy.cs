using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FollowEnemy : MonoBehaviour {

    public Transform objectToFollow;
    public RectTransform _myCanvas;
    
    public Vector3 localOffset;
    public Vector3 screenOffset;


    void Start () {
		gameObject.transform.SetParent(_myCanvas.transform);
    }

   
    void LateUpdate () {

        Vector3 worldPoint = objectToFollow.TransformPoint(localOffset);

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPoint);

      
        viewportPoint -= 0.5f * Vector3.one; 
        viewportPoint.z = 0;

      
        Rect rect = _myCanvas.rect;
        viewportPoint.x *= rect.width;
        viewportPoint.y *= rect.height;

        transform.localPosition = viewportPoint + screenOffset;
    }
}