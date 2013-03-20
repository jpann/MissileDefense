using UnityEngine;
using System.Collections;

public class GunBulletCollisionHandler : MonoBehaviour 
{
	private GunBulletController objController;
	private GameObject objPlayer;
	
	void Start() 
	{
		objController = GetComponent<GunBulletController>();
		objPlayer = (GameObject) GameObject.FindWithTag("Player");
	}
	
	void Update()
	{
		
	}
	
	public void OnCollisionEnter(Collision col)
	{
		//Debug.Log("GunBulletCollisionHandler:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
		
		float damage = objController.damage;
		
		if (col.gameObject.tag == "Projectile")
		{
			EnemyMissileController enemy = col.gameObject.GetComponent<EnemyMissileController>();
				
			if (enemy != null)
			{
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
				enemy.TakeDamage(damage);
			
				GameInfoManager.Instance.IncrementDamageDealt(damage);
				GameInfoManager.Instance.IncrementScore();
			}
		}
		
		objController.ExpireMe();
	}
}
