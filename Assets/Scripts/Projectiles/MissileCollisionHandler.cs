using UnityEngine;
using System.Collections;

public class MissileCollisionHandler : MonoBehaviour 
{
	private GameObject objPlayer;
	private MissileController controller;
	
	void Start() 
	{
		controller = GetComponent<MissileController>();
		objPlayer = (GameObject) GameObject.FindWithTag("Player");
	}
	
	void Update()
	{
		// Check for enemies within radius
		CheckForEnemiesInRadius();
	}
	
	public virtual void OnCollisionEnter(Collision col)
	{		
		//Debug.Log("MissileCollisionHandler:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
		
		float damage = controller.damage;
		
		if (col.gameObject.tag == "Projectile")
		{
			EnemyMissileController enemy = col.gameObject.GetComponent<EnemyMissileController>();
				
			if (enemy != null)
			{
				float proximity = (transform.position - enemy.transform.position).magnitude;
				float effect = 1 - (proximity / controller.damageRadius);
				
				float damageAmount = (damage * effect);
				damageAmount = Mathf.Clamp(damageAmount, 1.0f, damage);
				
				enemy.TakeDamage(damage);
			
				GameInfoManager.Instance.IncrementDamageDealt(damage);
				GameInfoManager.Instance.IncrementScore();
				GameInfoManager.Instance.IncrementEnemyMissilesHit();
			}
		}
		else if (col.gameObject.tag == "EnemyPlane")
		{
			EnemyPlaneController enemy = col.gameObject.GetComponent<EnemyPlaneController>();
				
			if (enemy != null)
			{
				float proximity = (transform.position - enemy.transform.position).magnitude;
				float effect = 1 - (proximity / controller.damageRadius);
				
				float damageAmount = (damage * effect);
				damageAmount = Mathf.Clamp(damageAmount, 1.0f, damage);
				
				enemy.TakeDamage(damage);
			
				GameInfoManager.Instance.IncrementDamageDealt(damage);
				GameInfoManager.Instance.IncrementScore();
			}
		}
		
		controller.ExpireMe();
	}
	
	void CheckForEnemiesInRadius()
	{
		int hits = 0;
		
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, controller.damageRadius);
		
		float damage = controller.damage;
		
		foreach (Collider col in objectsInRange)
		{
			EnemyMissileController enemy = col.gameObject.GetComponent<EnemyMissileController>();
			
			if (enemy != null)
			{
				//Debug.Log("MissileCollisionHandler:CheckForEnemiesInRadius - enemy != null");
				
				float proximity = (transform.position - enemy.transform.position).magnitude;
				float effect = 1 - (proximity / controller.damageRadius);
				
				float damageAmount = (damage * effect);
				damageAmount = Mathf.Clamp(damageAmount, 1.0f, damage);
				
				enemy.TakeDamage(10000.0f);
				
				GameInfoManager.Instance.IncrementDamageDealt(damage);
				GameInfoManager.Instance.IncrementScore();
				
				hits++;
			}
			else
			{
				//Debug.Log("MissileCollisionHandler:CheckForEnemiesInRadius - enemy == null");	
			}
		}
		
		if (hits > 0)
		{
			GameInfoManager.Instance.IncrementEnemyMissilesHit(hits);
			controller.ExpireMe();
		}
	}
}
