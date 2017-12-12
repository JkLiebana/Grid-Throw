using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float panSpeed = 20f;
	public float scrollSpeed = 20;

	public Vector2 limitsXPosition, limitsYPosition, limitsZPosition;

	private Vector3 position;
	void Update () {
		
		position = gameObject.transform.position;

		if(Input.GetKey(KeyCode.W))
		{
			position.z += panSpeed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S))
		{
			position.z -= panSpeed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.A))
		{
			position.x -= panSpeed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.D))
		{
			position.x += panSpeed * Time.deltaTime;
		}

		float scrollVal = Input.GetAxis("Mouse ScrollWheel");

		position.y -= scrollVal * scrollSpeed * Time.deltaTime;

		position.x = Mathf.Clamp(position.x, limitsXPosition.x, limitsXPosition.y);
		position.y = Mathf.Clamp(position.y, limitsYPosition.x, limitsYPosition.y);		
		position.z = Mathf.Clamp(position.z, limitsZPosition.x, limitsZPosition.y);			

		transform.position = position;
	}
}
