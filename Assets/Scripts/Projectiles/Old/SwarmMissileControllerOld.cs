using UnityEngine;
using System.Collections;

public class SwarmMissileControllerOld : MonoBehaviour 
{
	public float moveSpeed = 1000.0f; // how fast the bullet moves
	public float damage = 10.0f;
	public float timeToLive = 3.0f;
	public float damageRadius = 100.0f;
	public GameObject objSmallMissile;
	public int numberOfSmallMissiles = 3;
	public float smallMissileSpeed = 1000.0f;
	public float maxHealth = 100.0f;
	
	public GameObject parBulletTrail;
	public GameObject parExplosion;
	
	private float timeSpentAlive; 
	private Vector3 previousPosition;
	
	private Quaternion rotation;
	private GameObject parTrail;
	private float health;
	
	void Start () 
	{
		health = maxHealth;
		
		parTrail = (GameObject)Instantiate(parBulletTrail, transform.position, Quaternion.identity);

		this.transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);
	}
	
	public void Initialize(Quaternion rot, Vector3 direction, float speedOffset)
	{
		rotation = rot;	

		// now assign the roation using Euler angle function
		transform.rotation = rotation;
		
		// accelerate it
		rigidbody.AddForce(direction * (moveSpeed + speedOffset));
	}
	
	void Update () 
	{
		timeSpentAlive += Time.deltaTime;
		
		// Check if bullet is off screen
		//TODO
		
		parTrail.transform.position = transform.position;

		if (timeSpentAlive > timeToLive)
		{
			SpawnSmallMissiles();
		}
		
		if (health <= 0.0f)
		{
			ExpireMe();	
		}
		
		previousPosition = transform.position;
	}
		
	void SpawnSmallMissiles()
	{
		Debug.Log("SwarmMissileController:SpawnSmallMissiles()");
		
		Vector3 position = transform.position;
		
		BreakUp();
		
		for (int i = 0; i < numberOfSmallMissiles; i++)
		{
			SpawnSmallMissile(position);
		}
	}
	
	
	void SpawnSmallMissile(Vector3 position)
	{
		Debug.Log("SwarmMissileController:SpawnSmallMissile1 - position = " + position);
		
		Transform trans = transform;
		Quaternion rotation = trans.rotation;

		if ((Random.Range(0, 10) % 2) == 0)
			rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 45.0f));
		else
			rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(315.0f, 360.0f));
		
		Vector3 dir = rotation * (-trans.up);
		
		GameObject objProjectile = (GameObject)Instantiate(objSmallMissile, 
			position, 
			rotation);

		objProjectile.rigidbody.AddForce(dir * smallMissileSpeed);
	}
	
//	void SpawnSmallMissileOLD(Vector3 position)
//	{
//		Debug.Log("SwarmMissileController:SpawnSmallMissile()");
//		
//		Vector3 fromPosition = position;
//
//		Vector3 dir = transform.forward;
//		dir.x += Random.Range(-1.5f, 2.5f);
//		dir.y += Random.Range(-1.5f,-5.5f);
//		dir.z = 0.0f;
//		dir.Normalize();
//		
//		Debug.Log("SpawnSmallMissile - Rotation = " + transform.rotation + "; Direction = " + dir + "; forward = " + transform.forward);
//	
//		Quaternion qRot = transform.rotation;
//
//		Debug.Log("SpawnSmallMissile - qRot = " + qRot);
//		
//		GameObject objProjectile = (GameObject)Instantiate(objSmallMissile, 
//			fromPosition, 
//			qRot);
//		
//		objProjectile.rigidbody.AddForce(dir * 500);
//	}
//	
	
	#region Damage 
	public void TakeDamage(float amount)
	{
		health -= amount;
		
		health = Mathf.Clamp(health, 0.0f, maxHealth);
	}
	#endregion
	
	#region Collision
	void CheckForEnemiesInRadius()
	{
		int hits = 0;
		
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, damageRadius);
		
		foreach (Collider col in objectsInRange)
		{
			if (col.gameObject.tag == "Structure")
			{
				StructureController structure = col.gameObject.GetComponent<StructureController>();
				
				if (structure != null)
				{
					structure.TakeDamage(damage);	
				}
			}
			else if (col.gameObject.tag == "Turret")
			{
				
			}
			else if (col.gameObject.tag == "Enemy")
			{
				EnemyBulletController enemy = col.GetComponent<EnemyBulletController>();
				
				if (enemy != null)
				{
					float proximity = (transform.position - enemy.transform.position).magnitude;
					float effect = 1 - (proximity / damageRadius);
					float damageAmount = (damage * effect);
					damageAmount = Mathf.Clamp(damageAmount, 1.0f, damage);
					
					enemy.DealDamage(damage);
					
					hits++;
				}
			}
			
			
		}
		
		if (hits > 0)
		{
			ExpireMe();
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		Debug.Log("SwarmMissileController:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
		
		ExpireMe();
	}
	#endregion
	
	#region Destroy Self
	void ExpireMe()
	{
		Debug.Log("SwarmMissileController:ExpireMe()");
		
		Instantiate(parExplosion, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
		parTrail.particleEmitter.minEmission = 0.0f;
		parTrail.particleEmitter.maxEmission = 0.0f;
		
		Destroy(parTrail, 1.7f);
	}
	
	void BreakUp()
	{
		Debug.Log("SwarmMissileController:BreakUp()");
		
		Destroy(gameObject);
		Destroy(parTrail, 1.7f);
	}
	
	void RemoveMe()
	{
		Debug.Log("SwarmMissileController:RemoveMe()");
		
		Destroy(gameObject);
		Destroy(parTrail, 1.7f);
	}
	#endregion
}
