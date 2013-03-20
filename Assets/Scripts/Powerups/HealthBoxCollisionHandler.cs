using UnityEngine;
using System.Collections;

public class HealthBoxCollisionHandler : MonoBehaviour 
{
	private HealthBoxController objController;
	private PlayerController objPlayerController;
	
	void Start() 
	{
		objPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		objController = GetComponent<HealthBoxController>();
	}
	
	void Update()
	{
	
	}
	
	#region Collision
	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("HealthBoxCollisionHandler:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
		
		if (col.gameObject.tag == "Projectile")
		{
			objPlayerController.HealTurretsByAmount(objController.healAmount);
			
			objController.ExpireMe();
		}
		else if (col.gameObject.tag == "Turret")
		{
			objController.ExpireMe();
		}
		else
		{
			objController.ExpireMe();
		}
	}
	#endregion
}
