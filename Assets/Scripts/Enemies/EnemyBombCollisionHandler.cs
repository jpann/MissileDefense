using UnityEngine;
using System.Collections;

public class EnemyBombCollisionHandler : MonoBehaviour 
{
	private EnemyBombController controller;
	
	void Start() 
	{
		controller = GetComponent<EnemyBombController>();
	}
	
	void Update()
	{
	
	}
	
	#region Collision
	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("EnemyBombCollisionHandler:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
		
		if (col.gameObject.tag == "Structure")
		{
			col.gameObject.GetComponent<StructureController>().TakeDamage(controller.damage);
		}
		else if (col.gameObject.tag == "Turret")
		{
			col.gameObject.GetComponent<TurretBarrelController>().TakeDamage(controller.damage);
		}
		
		CheckForEnemiesInRadius();
		
		controller.ExpireMe();
	}
	#endregion
	
	void CheckForEnemiesInRadius()
	{
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, controller.damageRadius);
		
		float damage = controller.damage;
		
		foreach (Collider col in objectsInRange)
		{
			if (col.gameObject.tag == "Structure")
			{
				StructureController structure = col.gameObject.GetComponent<StructureController>();
				Debug.DrawLine(transform.position, col.gameObject.transform.position, Color.red, 2.0f);
				
				if (structure != null)
				{
					float proximity = (transform.position - structure.transform.position).magnitude;
					float effect = 1 - (proximity / controller.damageRadius);
					
					float damageAmount = (damage * effect);
					damageAmount = Mathf.Clamp(damageAmount, 1.0f, damage);
					
					structure.TakeDamage(damageAmount);
				}
			}
			else if (col.gameObject.tag == "Turret")
			{
				TurretBarrelController turret = col.gameObject.GetComponent<TurretBarrelController>();
				Debug.DrawLine(transform.position, col.gameObject.transform.position, Color.red, 2.0f);
				
				if (turret != null)
				{
					float proximity = (transform.position - turret.transform.position).magnitude;
					float effect = 1 - (proximity / controller.damageRadius);
					
					float damageAmount = (damage * effect);
					damageAmount = Mathf.Clamp(damageAmount, 1.0f, damage);
					
					turret.TakeDamage(damageAmount);
				}
			}
		}
	}
}
