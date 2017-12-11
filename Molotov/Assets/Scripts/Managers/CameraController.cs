using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float panSpeed = 20f;
	public float panBorderThickness = 10f;
	public float scrollSpeed = 20;

	public float minPositionX = -10f, maxPositionX = 15f;
	public float minPositionZ, maxPositionZ;
	public float minPositionY, maxPositionY;

	private Vector3 position;
	void Update () {
		
		position = gameObject.transform.position;

		if(Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
			position.z += panSpeed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
		{
			position.z -= panSpeed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
		{
			position.x -= panSpeed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			position.x += panSpeed * Time.deltaTime;
		}

		float scrollVal = Input.GetAxis("Mouse ScrollWheel");

		position.y -= scrollVal * scrollSpeed * Time.deltaTime;

		position.x = Mathf.Clamp(position.x, minPositionX, maxPositionX);
		position.y = Mathf.Clamp(position.y, minPositionY, maxPositionY);		
		position.z = Mathf.Clamp(position.z, minPositionZ, maxPositionZ);			

		transform.position = position;
	}
}
