using UnityEngine;
using System.Collections;

public class MinigunCollisionHandler : MonoBehaviour 
{
	private MinigunController objController;

	void Start() 
	{
		objController = GetComponent<MinigunController>();
	}
	
	void Update() 
	{
	
	}
	
	void OnCollisionEnter(Collision col)
	{
		//if (!objController.IsActive)
		//	return;
		
		Debug.Log("MinigunCollisionHandler:OnCollisionEnter(Self: " + gameObject.name + ") - (" + col.gameObject.tag + " : " + col.gameObject.name + ") transform.position = " + transform.position);
	}
}
