using UnityEngine;
using System.Collections;

public class TurretCollisionController : MonoBehaviour
{
	private TurretBarrelController controller;
	
	void Start() 
	{
		controller = GetComponent<TurretBarrelController>();
	}
	
	void Update() 
	{
	
	}
	
	#region Collision
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("TurretCollisionController:OnTriggerEnter - (" + gameObject.name + ") other.Name = " + other.name);	
	}
	
	void OnTriggerExit(Collider other)
	{
		Debug.Log("TurretCollisionController:OnTriggerExit - (" + gameObject.name + ") other.Name = " + other.name);	
	}
	
	void OnCollisionEnter(Collision col)
	{		
		//Debug.Log("TurretCollisionController:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);

//		EnemyMissileController enemy = col.gameObject.GetComponent<EnemyMissileController>();
//			
//		if (enemy != null)
//		{
//			//Debug.Log("TurretCollisionController:OnCollisionEnter - enemy != null");
//			
//			SendMessage("TakeDamage", enemy.damage);
//			//controller.TakeDamage(enemy.damage);
//		}
	}
	#endregion
}
