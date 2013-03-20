using UnityEngine;
using System.Collections;

public class SwarmMissileController : MissileController 
{
	public GameObject objSmallMissile;
	public int numberOfSmallMissiles = 3;
	public float smallMissileSpeed = 1000.0f;
	
	void Start() 
	{
		autoDestroy = false;
		
		health = maxHealth;
		
		parTrail = (GameObject)Instantiate(parBulletTrail, transform.position, Quaternion.identity);

		this.transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);
	}
	
	void Update()
	{
		timeSpentAlive += Time.deltaTime;
		
		if (timeSpentAlive > timeToLive)
		{
			SpawnSmallMissiles();
		}	
	}

	public void SpawnSmallMissiles()
	{
		Debug.Log("SwarmMissileController:SpawnSmallMissiles()");
		
		Vector3 position = transform.position;
		
		BreakUp();
		
		for (int i = 0; i < numberOfSmallMissiles; i++)
		{
			SpawnSmallMissile(position);
		}
	}
	
	public void SpawnSmallMissile(Vector3 position)
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
	
	#region Damage 
	public void TakeDamage(float amount)
	{
		health -= amount;
		
		health = Mathf.Clamp(health, 0.0f, maxHealth);
	}
	#endregion

	void BreakUp()
	{
		Debug.Log("SwarmMissileController:BreakUp()");
		
		Destroy(gameObject);
		Destroy(parTrail, 1.7f);
	}
}
