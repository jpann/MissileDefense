using UnityEngine;
using System.Collections;

public class MinigunBoxCollisionHandler : MonoBehaviour 
{
	private MinigunBoxController objController;
	
	void Start() 
	{
		objController = GetComponent<MinigunBoxController>();
	}
	
	void Update() 
	{
	
	}
	
	#region Collision
	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("MinigunBoxCollisionHandler:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
		
		if (col.gameObject.tag == "Turret" || 
			col.gameObject.tag == "Platform")
		{
			objController.Deploy();
		}
	}
	#endregion
}
