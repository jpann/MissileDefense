using UnityEngine;
using System.Collections;

public class EnemyBulletController : MonoBehaviour 
{
	public float moveSpeed = 10000.0f; // how fast the bullet moves
	public float damage = 10.0f;
	public float timeToLive = 20.0f;
	public float maxHealth = 10.0f;
	
	private float health;
	private float timeSpentAlive; // how long the bullet has stayed alive for
	private GameObject objSpawner;
	private EnemyVariables ptrScriptVariable;
	private Quaternion rotation;
	
	private GameObject parBulletTrail;
	
	void Start() 
	{		
		objSpawner = (GameObject) GameObject.FindWithTag("EnemySpawner");
		ptrScriptVariable = (EnemyVariables)objSpawner.GetComponent(typeof(EnemyVariables));
		
		parBulletTrail = (GameObject)Instantiate(ptrScriptVariable.parBulletTrail, transform.position, Quaternion.identity);

		this.transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);
		
		health = maxHealth;
	}
	
	public void Initialize(Quaternion rot, Vector3 direction, float speedModifer)
	{
		rotation = rot;	
		
		// now assign the roation using Euler angle function
		transform.rotation = rotation;
		
		// accelerate it
		rigidbody.AddForce(direction * (moveSpeed + speedModifer));
	}
	
	void Update() 
	{
		timeSpentAlive += Time.deltaTime;
		
		parBulletTrail.transform.position = transform.position;
		
		if (timeSpentAlive > timeToLive)
		{
			ExpireMe();
		}
		
		if (health <= 0.0f)
		{
			ExpireMe();	
		}
	}
	
	#region Collision
	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("EnemyBulletController:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
	
		if (col.gameObject.tag == "Structure")
		{
			col.gameObject.GetComponent<StructureController>().TakeDamage(5.0f);
	
		}
		
		ExpireMe();
	}
	#endregion
	
	public void DealDamage(float amount)
	{
		health -= amount;
		
		health = Mathf.Clamp(health, 0.0f, maxHealth); 
		
		//Debug.Log("EnemyBulletController:DealDamage - amount = " + amount + "; health = " + health);
	}
	
	#region Destroy Self	
	void ExpireMe()
	{
		//Debug.Log("EnemyBulletController:ExpireMe()");
		
		Instantiate(ptrScriptVariable.parExplosion, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
		
		parBulletTrail.particleEmitter.minEmission = 0.0f;
		parBulletTrail.particleEmitter.maxEmission = 0.0f;
		
		Destroy(parBulletTrail, 1.7f);
	}
	
	public void RemoveMe()
	{
		//Debug.Log("EnemyBulletController:RemoveMe()");
		
		Destroy(gameObject);
		Destroy(parBulletTrail, 1.7f);
	}
	#endregion
}
