using UnityEngine;
using System.Collections;

public class EnemySwarmMissileController : EnemyMissileController
{
	public GameObject objSmallMissile;
	public int numberOfSmallMissiles = 3;
	public float smallMissileSpeed = 1000.0f;
	
	void Start()
	{
		//Debug.Log("EnemySwarmMissileController:Start()");
		
		autoDestroy = false;
		
		base.Start();
	}
	
	void Update()
	{
		if (timeSpentAlive > timeToLive)
		{
			SpawnSmallMissiles();
		}
		
		base.Update();
	}
	
	public void SpawnSmallMissiles()
	{
		//Debug.Log("EnemySwarmMissileController:SpawnSmallMissiles()");
		
		Vector3 position = transform.position;
		
		BreakUp();
		
		for (int i = 0; i < numberOfSmallMissiles; i++)
		{
			SpawnSmallMissile(position);
		}
	}
	
	public void SpawnSmallMissile(Vector3 position)
	{
		//Debug.Log("EnemySwarmMissileController:SpawnSmallMissile - position = " + position);
		
		Transform trans = transform;

		GameObject objProjectile = (GameObject)Instantiate(objSmallMissile, 
			position, 
			Quaternion.identity);
		
		Vector3 direction = transform.up;
		direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation =  Quaternion.Euler(0f, 0f, rotZ - 90);
		
		//objCreatedMissile.GetComponent<TestMissileController>().Target = target;
		objProjectile.GetComponent<EnemyMissileController>().Direction = direction;
		objProjectile.GetComponent<EnemyMissileController>().Rotation = rotation;
		objProjectile.GetComponent<EnemyMissileController>().Speed = 300.0f;
		objProjectile.GetComponent<EnemyMissileController>().EnableSineWave = true;
		objProjectile.GetComponent<EnemyMissileController>().SineWaveMaxBobHeight = 1.0f;
		objProjectile.GetComponent<EnemyMissileController>().SineWaveMinBobHeight = 1.0f;
		objProjectile.GetComponent<EnemyMissileController>().SineWaveMaxBobTime = 6.0f;
		objProjectile.GetComponent<EnemyMissileController>().SineWaveMinBobTime = 6.0f;

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
		//Debug.Log("EnemySwarmMissileController:BreakUp()");
		
		Destroy(gameObject);
		Destroy(parTrail, 1.7f);
	}
}
