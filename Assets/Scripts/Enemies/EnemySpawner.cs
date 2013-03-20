using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
	public bool disableSpawner = false;
	public float missileSpawnInterval = 5.0f;
	public int missilesToSpawnPerInterval = 15;
	public float missileSpawnAmountIncreaseInterval = 20.0f;
	public int missileIncreaseAmountBy = 3;
	public float missileIncreaseSpeedBy = 500.0f;
	public float planeSpawnInterval = 10.0f;
	public int planesToSpawnPerInterval = 1;
	
	private EnemyVariables ptrScriptVariable;
	private tk2dCamera objCamera;
	
	// Missile spawn timer
	private float lastMissileSpawn = 0.0f;
	private float missileSpawnTimer = 0.0f;
	
	// Missile spawn increase timer
	private float lastMissileIncrease = 0.0f;
	private float missileIncreaseTimer = 0.0f;
	
	// Missile speed modifier
	private float missileSpeedModifier = 0.0f;
	
	// Plane spawn
	private float lastPlaneSpawn = 0.0f;
	private float planeSpawnTimer = 0.0f;
	
	void Start ()
	{
		objCamera = (tk2dCamera) GameObject.FindWithTag("MainCamera").GetComponent<tk2dCamera>();
		ptrScriptVariable = (EnemyVariables)GetComponent(typeof(EnemyVariables));
	}

	void Update () 
	{
		float elapsed = Time.time;
		
		missileSpawnTimer += elapsed;
		
		// Missile spawn timer
		if ((elapsed - lastMissileSpawn) >= missileSpawnInterval)
		{
			lastMissileSpawn = elapsed;
			
			SpawnMissileWave();
		}
		
		// Missile increase timer
		if ((elapsed - lastMissileIncrease) >= missileSpawnAmountIncreaseInterval)
		{
			missilesToSpawnPerInterval += missileIncreaseAmountBy;
			missileSpeedModifier += missileIncreaseSpeedBy;
			
			lastMissileIncrease = elapsed;
		}
		
		// Plane spawn timer
		if ((elapsed - lastPlaneSpawn) >= planeSpawnInterval)
		{
			lastPlaneSpawn = elapsed;
			
			SpawnPlanes();
		}
	}
	
	#region Random position and direction
	private Vector3 GetRandomFirePosition()
	{
		Vector3 position = transform.position;	
		
		position.x = Random.Range(10.0f, Screen.width - 10.0f);
		position.z = 0.0f;
		
		return position;
	}
	
	private Quaternion GetRandomFireRotation(Vector3 position, out Vector3 diff, float offset)
	{
		Vector3 targetPosition = new Vector3(Random.Range(10.0f, Screen.width - 10.0f), 1.0f, 0.0f);
		
		diff = targetPosition - position;
		
		diff.Normalize();
		
		float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		
		return Quaternion.Euler(0f, 0f, rotation + offset);
	}
	
	private Quaternion GetMouseFireRotation(Vector3 position, out Vector3 diff, float offset)
	{
		Vector3 targetPosition = objCamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		
		diff = targetPosition - position;
		
		diff.Normalize();
		
		float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		
		return Quaternion.Euler(0f, 0f, rotation + offset);	
	}
	#endregion
	
	#region Random Plane position and direction methods
	private Vector3 GetRandomPlanePosition()
	{
		Vector3 position = Vector3.zero;
		
		if (Random.Range(0, 1) == 0)
		{
			// Left side
			position = new Vector3(
				-10.0f, 			// 10 to the left of the left screen border
				Random.Range(230.0f, 661.0f), 	// random position on Y axis between 230 and 661
				1.0f);	
		}
		else
		{
			// Right side
			position = new Vector3(
				Screen.width + 10.0f, 			// 10 to the right of the right screen border
				Random.Range(230.0f, 661.0f), 	// random position on Y axis between 230 and 661
				1.0f);	
		}
		
		return position;
	}
	#endregion
	
	public void SpawnMissileWave()
	{
		if (disableSpawner)
			return;
		
		for (int i = 0; i < missilesToSpawnPerInterval; i++)
		{
			int x = Random.Range(1, 10);
			
			if ((x >= 1) && (x <= 1))
			{
				FireSwarm();
			}
			else if ((x >= 2) && (x <= 6))
			{
				FireSmall();
			}
			else
			{
				Fire();
			}
		}
	}
	
	public void SpawnPlanes()
	{
		if (disableSpawner)
			return;
		
		for (int i = 0; i < planesToSpawnPerInterval; i++)
		{
			int x = Random.Range(1, 10);
			
			SpawnPlane();
		}
	}
	
	#region Plane Spawning Methods
	public void SpawnPlane()
	{
		Vector3 position = GetRandomPlanePosition();
		
		PlaneDirection direction = PlaneDirection.LEFT;
		if (position.x <= 0.0f)
			direction = PlaneDirection.RIGHT;
		else
			direction = PlaneDirection.RIGHT;
		
		GameObject objCreatedPlane = (GameObject)Instantiate(ptrScriptVariable.objPlane, 
			position, 
			Quaternion.identity);
		
		objCreatedPlane.GetComponent<EnemyPlaneController>().Direction = direction;
	}
	#endregion
	
	#region Missile Firing Methods
	public void FireSwarm()
	{
		// Get random position to fire from
		Vector3 fromPosition = GetRandomFirePosition();
		Vector3 diff;
		
		// Get random rotation towards the ground to fire at, offset the rotation by 90 degrees so the projectile is straight
		Quaternion rotation = GetRandomFireRotation(fromPosition, out diff, -90);
		
		if (ptrScriptVariable == null)
		{
			return;
		}
		
		GameObject objCreatedMissile = (GameObject)Instantiate(ptrScriptVariable.objSwarmMissile, 
			fromPosition, 
			Quaternion.identity);
		
		Vector3 direction = diff;
		//direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		objCreatedMissile.GetComponent<EnemyMissileController>().Direction = direction;
		objCreatedMissile.GetComponent<EnemyMissileController>().Rotation = rotation;
	}
	
	public void FireSmall()
	{
		// Get random position to fire from
		Vector3 fromPosition = GetRandomFirePosition();
		Vector3 diff;
		
		// Get random rotation towards the ground to fire at, offset the rotation by 90 degrees so the projectile is straight
		Quaternion rotation = GetRandomFireRotation(fromPosition, out diff, -90);
		
		if (ptrScriptVariable == null)
		{
			return;
		}
		
		GameObject objCreatedMissile = (GameObject)Instantiate(ptrScriptVariable.objSmallMissile, 
			fromPosition, 
			Quaternion.identity);
		
		Vector3 direction = diff;
		//direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		objCreatedMissile.GetComponent<EnemyMissileController>().Direction = direction;
		objCreatedMissile.GetComponent<EnemyMissileController>().Rotation = rotation;
	}
	
	public void FireHoming()
	{
		// Get random position to fire from
		Vector3 fromPosition = GetRandomFirePosition();
		Vector3 diff;
		
		// Get random rotation towards the ground to fire at, offset the rotation by 90 degrees so the projectile is straight
		Quaternion rotation = GetRandomFireRotation(fromPosition, out diff, -90);
		
		if (ptrScriptVariable == null)
		{
			return;
		}
		
		GameObject objCreatedMissile = (GameObject)Instantiate(ptrScriptVariable.objHomingMossile, 
			fromPosition, 
			Quaternion.identity);
		
		Vector3 direction = diff;
		//direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		objCreatedMissile.GetComponent<EnemyHomingMissileController>().Direction = direction;
		objCreatedMissile.GetComponent<EnemyHomingMissileController>().Rotation = rotation;
	}
	
	public void FireSmallSine()
	{
		// Get random position to fire from
		Vector3 fromPosition = GetRandomFirePosition();
		Vector3 diff;
		
		// Get random rotation towards the ground to fire at, offset the rotation by 90 degrees so the projectile is straight
		Quaternion rotation = GetRandomFireRotation(fromPosition, out diff, -90);
		
		if (ptrScriptVariable == null)
		{
			return;
		}
		
		GameObject objCreatedMissile = (GameObject)Instantiate(ptrScriptVariable.objSmallMissile, 
			fromPosition, 
			Quaternion.identity);
		
		Vector3 direction = diff;
		//direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		objCreatedMissile.GetComponent<EnemyMissileController>().Direction = direction;
		objCreatedMissile.GetComponent<EnemyMissileController>().Rotation = rotation;
		objCreatedMissile.GetComponent<EnemyMissileController>().EnableSineWave = true;
		objCreatedMissile.GetComponent<EnemyMissileController>().SineWaveMaxBobHeight = 2;
		objCreatedMissile.GetComponent<EnemyMissileController>().SineWaveMaxBobTime = 6;
	}
	
	public void Fire()
	{
		// Get random position to fire from
		Vector3 fromPosition = GetRandomFirePosition();
		Vector3 diff;
		
		// Get random rotation towards the ground to fire at, offset the rotation by 90 degrees so the projectile is straight
		Quaternion rotation = GetRandomFireRotation(fromPosition, out diff, -90);
		
		if (ptrScriptVariable == null)
		{
			//Debug.Log("EnemySpawner:Fire() - ptrScriptVariable == null");
			
			return;
		}

		GameObject objCreatedMissile = (GameObject)Instantiate(ptrScriptVariable.objMissile, 
			fromPosition, 
			Quaternion.identity);
		
		Vector3 direction = diff;
		//direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		objCreatedMissile.GetComponent<EnemyMissileController>().Direction = direction;
		objCreatedMissile.GetComponent<EnemyMissileController>().Rotation = rotation;
	}
	
	public void FireAtMouse()
	{
		// Get random position to fire from
		Vector3 fromPosition = GetRandomFirePosition();
		Vector3 diff;
		
		Quaternion rotation = GetMouseFireRotation(fromPosition, out diff, -90);
		
		if (ptrScriptVariable == null)
		{
			return;
		}
		
		GameObject objCreatedMissile = (GameObject)Instantiate(ptrScriptVariable.objMissile, 
			fromPosition, 
			Quaternion.identity);
		
		Vector3 direction = diff;
		//direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		objCreatedMissile.GetComponent<EnemyMissileController>().Direction = direction;
		objCreatedMissile.GetComponent<EnemyMissileController>().Rotation = rotation;
	}
	#endregion
}
