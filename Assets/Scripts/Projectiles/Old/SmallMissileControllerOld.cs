using UnityEngine;
using System.Collections;

public class SmallMissileControllerOld : MonoBehaviour 
{
	// public variables
	public float moveSpeed = 1000.0f; // how fast the bullet moves
	public float damage = 10.0f;
	public float timeToLive = 20.0f;
	public float damageRadius = 100.0f;
	public float maxHealth = 100.0f;
	
	private float health;
	public GameObject parBulletTrail;
	public GameObject parExplosion;
	
	private float timeSpentAlive; 
	
	private GameObject parTrail;
	private Quaternion rotation;
	
	void Start() 
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
			ExpireMe();
		}
		
		if (health <= 0.0f)
		{
			ExpireMe();	
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		Debug.Log("SmallMissileController:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
	
		if (col.gameObject.tag == "Structure")
		{
			col.gameObject.GetComponent<StructureController>().TakeDamage(5.0f);
		}
		
		//ExpireMe();
	}
	
	public void TakeDamage(float amount)
	{
		health -= amount;
		
		health = Mathf.Clamp(health, 0.0f, maxHealth); 
		
		Debug.Log("SmallMissileController:TakeDamage - amount = " + amount + "; health = " + health);
	}
	
	#region Destroy Self
	void ExpireMe()
	{
		Debug.Log("SmallMissileController:ExpireMe()");
		
		Instantiate(parExplosion, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
		parTrail.particleEmitter.minEmission = 0.0f;
		parTrail.particleEmitter.maxEmission = 0.0f;
		
		Destroy(parTrail, 1.7f);
	}
	
	void RemoveMe()
	{
		Debug.Log("SmallMissileController:removeMe()");
		
		Destroy(gameObject);
		Destroy(parTrail, 1.7f);
	}
	#endregion
}
