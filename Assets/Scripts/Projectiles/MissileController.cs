using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour 
{
	// Public variables defined in Inspector
	public float moveSpeed = 100.0f;
	public float damage = 1000.0f;
	public float timeToLive = 20.0f;
	public float damageRadius = 100.0f;
	public float maxHealth = 100.0f;
	public GameObject parBulletTrail;
	public GameObject parExplosion;
	
	// Homing missile public variables
	public bool enableHoming = false;
	public float trackingTurnSpeed = 1.5f;
	
	// Sine wave public variables
	public bool enableSineWave = false;
	public float sineWaveMinBobTime = 2.0f;
	public float sineWaveMaxBobTime = 10.0f;
	public float sineWaveMinBobHeight = 1.0f;
	public float sineWaveMaxBobHeight = 5.0f;
	
	protected GameObject owner;
	protected GameObject parTrail;
	
	protected float health;
	protected float timeSpentAlive;
	protected bool autoDestroy = true;
	
	// Rotation and movement
	protected Quaternion rotation;
	protected Vector3 direction;
	protected float speedModifier = 0.0f;
	
	// Homing Missile
	protected Vector3 trackingTarget;
	
	// Sine wave
	protected float start;
	protected float sineWaveBobTime = 0.0f;
	protected float sineWaveBobHeight = 00.0f;	
	
	#region Properties
	public GameObject Owner
	{
		get { return owner; }	
		set { owner = value; }
	}
	
	public Vector3 Direction
	{
		get { return direction; }	
		set { direction = value; }
	}
	
	public Vector3 Target
	{
		get { return trackingTarget; }	
		set { trackingTarget = value; }
	}
	
	public Quaternion Rotation
	{
		get { return transform.rotation; }	
		set { transform.rotation = value; }
	}
	
	public float Speed
	{
		get { return moveSpeed; }	
		set { moveSpeed = value; }
	}
	
	public float SpeedModifier
	{
		get { return speedModifier; }
		set { speedModifier = value; }
	}
	#endregion
	
	public virtual void Start ()
	{
		start = Time.time;
		health = maxHealth;
		
		parTrail = (GameObject)Instantiate(parBulletTrail, transform.position, Quaternion.identity);

		this.transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);	
		
		if (enableSineWave)
		{
			sineWaveBobTime = Random.Range(sineWaveMinBobTime, sineWaveMaxBobTime);
			sineWaveBobHeight = Random.Range(sineWaveMinBobHeight, sineWaveMaxBobHeight);	
		}
	}
	
	public virtual void Update () 
	{
		timeSpentAlive += Time.deltaTime;
	
		transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);
		
		if (enableHoming)
		{
			direction = trackingTarget - transform.position;
			direction.Normalize();
			
			float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			Quaternion newRotation = Quaternion.Euler(0f, 0f, rotZ - 90); 
		
			transform.localRotation = Quaternion.Slerp(transform.rotation, newRotation, trackingTurnSpeed * Time.deltaTime);
		}
		
		if (enableSineWave)
		{
			transform.Rotate(0, 0, Mathf.Sin((Time.time - start + 1.3f) * sineWaveBobTime) * sineWaveBobHeight);	
		}
		
		transform.Translate(transform.up * (Time.deltaTime * (moveSpeed + speedModifier)),  Space.World);
		
		parTrail.transform.position = transform.position;
		
		if (timeSpentAlive > timeToLive)
		{
			if (autoDestroy)
				ExpireMe();
		}
		
		if (health <= 0.0f)
		{
			ExpireMe();	
		}
	}
	
	#region Damage 
	public void TakeDamage(float amount)
	{
		health -= amount;
		health = Mathf.Clamp(health, 0.0f, maxHealth);
	}
	#endregion
	
	#region Destroy Self
	public void ExpireMe()
	{
		//Debug.Log("MissileController:ExpireMe()");
		
		Instantiate(parExplosion, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
		parTrail.particleEmitter.minEmission = 0.0f;
		parTrail.particleEmitter.maxEmission = 0.0f;
		
		Destroy(parTrail, 1.7f);
	}
	
	public void RemoveMe()
	{
		Debug.Log("MissileController:removeMe()");
		
		Destroy(gameObject);
		Destroy(parTrail, 1.7f);
	}
	#endregion
}
