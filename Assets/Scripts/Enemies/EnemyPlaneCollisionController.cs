using UnityEngine;
using System.Collections;

public class EnemyPlaneCollisionController : MonoBehaviour
{
	private EnemyPlaneController controller;
	
	void Start() 
	{
		controller = GetComponent<EnemyPlaneController>();
	}
	
	void Update() 
	{
	
	}
	
	#region Collision
	void OnCollisionEnter(Collision col)
	{		
		//Debug.Log("EnemyPlaneCollisionController:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
		
		controller.ExpireMe();
	}
	#endregion
}
