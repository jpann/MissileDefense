using UnityEngine;
using System.Collections;

public class PowerUpSpawnController : MonoBehaviour 
{
	public float spawnInterval = 5.0f;
	public int powerUpsPerInterval = 3;
	
	public GameObject objHealthBox;
	public GameObject objHealthBoxPercent;
	
	// Missile spawn timer
	private bool disableSpawner = false;
	private float lastPowerUpSpawn = 0.0f;
	private float powerUpSpawnTimer = 0.0f;
	
	void Start ()
	{
		
	}

	void Update () 
	{
		float elapsed = Time.time;
		
		powerUpSpawnTimer += elapsed;
	
		if ((elapsed - lastPowerUpSpawn) >= spawnInterval)
		{
			lastPowerUpSpawn = elapsed;
			
			Spawn();
		}
	}
	
	private Vector3 GetRandomPosition()
	{
		Vector3 position = transform.position;	
		
		position.x = Random.Range(10.0f, Screen.width - 10.0f);
		position.z = 0.0f;
		
		return position;
	}
	
	public void Spawn()
	{
		if (disableSpawner)
			return;
		
		for (int i = 0; i < powerUpsPerInterval; i++)
		{
			SpawnHealthBox();
		}
	}
	
	public void SpawnHealthBox()
	{
		Vector3 fromPosition = GetRandomPosition();
		Vector3 diff;
		
		GameObject oHealthBox = (GameObject)Instantiate(objHealthBox, 
			fromPosition, 
			Quaternion.identity);	
	}
}
