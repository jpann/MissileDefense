using UnityEngine;
using System.Collections;

public class EnemyMissileCollisionController : MonoBehaviour
{
	private EnemyMissileController controller;
	
	void Start() 
	{
		controller = GetComponent<EnemyMissileController>();
	}
	
	void Update()
	{
	
	}
	
	#region Collision
	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("EnemyMissileCollisionController:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
		
		if (col.gameObject.tag == "Structure")
		{
			col.gameObject.GetComponent<StructureController>().TakeDamage(controller.damage);
		}
		else if (col.gameObject.tag == "Turret")
		{
			col.gameObject.GetComponent<TurretBarrelController>().TakeDamage(controller.damage);
		}
		else if (col.gameObject.tag == "AITurret")
		{
			col.gameObject.GetComponent<MinigunController>().TakeDamage(controller.damage);	
		}
		
		controller.ExpireMe();
	}
	#endregion
}
