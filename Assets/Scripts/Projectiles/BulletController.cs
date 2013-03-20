using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour 
{
	public float moveSpeed = 5000.0f; // how fast the bullet moves
	public float damage = 10.0f;
	public float timeToLive = 20.0f;
	public float damageRadius = 100.0f;
	
	private float timeSpentAlive; // how long the bullet has stayed alive for
	private GameObject objPlayer;
	private VariableScript ptrScriptVariable;
	private Quaternion rotation;
	private GameObject parBulletTrail;
	
	void Start() 
	{
		objPlayer = (GameObject) GameObject.FindWithTag("Player");
		ptrScriptVariable = (VariableScript)objPlayer.GetComponent(typeof(VariableScript));

		parBulletTrail = (GameObject)Instantiate(ptrScriptVariable.parBulletTrail, transform.position, Quaternion.identity);

		this.transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);	
	}
	
	public void Initialize(Quaternion rot, Vector3 direction, float speed)
	{
		rotation = rot;	
		//moveSpeed = speed;

		// now assign the roation using Euler angle function
		transform.rotation = rotation;
		
		// accelerate it
		rigidbody.AddForce(direction * moveSpeed);
	}
	
	void Update() 
	{
		timeSpentAlive += Time.deltaTime;
		
		// Check if bullet is off screen
		//TODO
		
		parBulletTrail.transform.position = transform.position;

		if (timeSpentAlive > timeToLive)
		{
			ExpireMe();
		}
		
		// Check for enemies within radius
		//CheckForEnemiesInRadius();	
	}
	
	#region Collision
//	void CheckForEnemiesInRadius()
//	{
//		int hits = 0;
//		
//		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, damageRadius);
//		
//		foreach (Collider col in objectsInRange)
//		{
//			EnemyBulletController enemy = col.GetComponent<EnemyBulletController>();
//			if (enemy != null)
//			{
//				float proximity = (transform.position - enemy.transform.position).magnitude;
//				float effect = 1 - (proximity / damageRadius);
//				float damageAmount = (damage * effect);
//				damageAmount = Mathf.Clamp(damageAmount, 1.0f, damage);
//				
//				enemy.DealDamage(damage);
//				
//				hits++;
//			}
//		}
//		
//		if (hits > 0)
//		{
//			ExpireMe();
//		}
//	}
//	
//	void OnCollisionEnter(Collision col)
//	{
//		Debug.Log("BulletController:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
//	
//		if (col.gameObject != objPlayer) 
//		{			
//			ExpireMe();
//		}
//		
//		//Instantiate(ptrScriptVariable.parExplosion, transform.position, Quaternion.identity);
//		ExpireMe();
//	}
	#endregion
	
	#region Destroy Self
	void ExpireMe()
	{
		Debug.Log("BulletController:ExpireMe()");
		
		Instantiate(ptrScriptVariable.parExplosion, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
		parBulletTrail.particleEmitter.minEmission = 0.0f;
		parBulletTrail.particleEmitter.maxEmission = 0.0f;
		
		Destroy(parBulletTrail, 1.7f);
	}
	
	void RemoveMe()
	{
		Debug.Log("BulletController:removeMe()");
		
		Destroy(gameObject);
		Destroy(parBulletTrail, 1.7f);
	}
	#endregion
}
