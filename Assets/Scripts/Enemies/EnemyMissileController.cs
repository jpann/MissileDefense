using UnityEngine;
using System.Collections;

public class EnemyMissileController : MonoBehaviour 
{
	// Public variables defined in Inspector
	public float moveSpeed = 100.0f; // how fast the bullet moves
	public float damage = 10.0f;
	public float timeToLive = 20.0f;
	public float maxHealth = 10.0f;
	public bool enableDud = false;
	
	// Homing Missile public variables
	public bool enableHoming = false;
	public float trackingTurnSpeed = 1.5f;
	public float startHomingIn = 0.5f;
	
	// Sine wave public variables
	public bool enableSineWave = false;
	public float sineWaveMinBobTime = 2.0f;
	public float sineWaveMaxBobTime = 10.0f;
	public float sineWaveMinBobHeight = 1.0f;
	public float sineWaveMaxBobHeight = 5.0f;
	
	public GameObject parBulletTrail;
	public GameObject parExplosion;
	
	protected GameObject parTrail;
	protected GameObject objSpawner;
	
	protected float health;
	protected bool autoDestroy = true;
	protected float timeSpentAlive; // how long the bullet has stayed alive for
	
	// Rotation and movement
	protected Quaternion rotation;
	protected Vector3 direction;
	protected float speedModifier = 0.0f;
	
	// Homing Missile
	protected Vector3 target;
	
	// Sine wave
	protected float start;
	protected float sineWaveBobTime = 0.0f;
	protected float sineWaveBobHeight = 00.0f;
	
	protected bool isDud = false;
	
	#region Properties
	public Vector3 Direction
	{
		get { return direction; }
		set { direction = value; }
	}
	
	public Vector3 Target
	{
		get { return target; }
		set { target = value; }
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
	
	public bool EnableSineWave
	{
		get { return enableSineWave; }
		set { enableSineWave = value; }
	}
	
	public float SineWaveMinBobTime
	{
		get { return sineWaveMinBobTime; }
		set { sineWaveMinBobTime = value; }
	}
	
	public float SineWaveMaxBobTime
	{
		get { return sineWaveMaxBobTime; }
		set { sineWaveMaxBobTime = value; }
	}
	
	public float SineWaveMinBobHeight
	{
		get { return sineWaveMinBobHeight; }
		set { sineWaveMinBobHeight = value; }
	}
	
	public float SineWaveMaxBobHeight
	{
		get { return sineWaveMaxBobTime; }
		set { sineWaveMaxBobTime = value; }
	}
	#endregion
	
	public virtual void Start() 
	{
		//Debug.Log("EnemyMissileController:Start()");
		
		objSpawner = (GameObject) GameObject.FindWithTag("EnemySpawner");

		parTrail = (GameObject)Instantiate(parBulletTrail, transform.position, Quaternion.identity);

		start = Time.time;
		health = maxHealth;
		
		this.transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);	
		
		if (enableSineWave)
		{
			sineWaveBobTime = Random.Range(sineWaveMinBobTime, sineWaveMaxBobTime);
			sineWaveBobHeight = Random.Range(sineWaveMinBobHeight, sineWaveMaxBobHeight);	
		}
		
		if (enableDud)
		{
			int x = Random.Range(1, 10);
			if (x <= 2)
				isDud = true;
		}
	}
	
	public virtual void Update() 
	{
		//Debug.Log("EnemyMissileController:Update()");
		
		timeSpentAlive += Time.deltaTime;
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);
		
		if (enableHoming)
		{
			if (timeSpentAlive >= startHomingIn)
			{
				direction = target - transform.position;
				direction.Normalize();
				
				float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				Quaternion newRotation = Quaternion.Euler(0f, 0f, rotZ - 90); 
			
				transform.localRotation = Quaternion.Slerp(transform.rotation, newRotation, trackingTurnSpeed * Time.deltaTime);
			}
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
	
	public void TakeDamage(float amount)
	{
		//Debug.Log("EnemyMissileController:TakeDamage() - amount = " + amount);
		
		health -= amount;
		
		health = Mathf.Clamp(health, 0.0f, maxHealth);
	}
	
	#region Destroy Self	
	public void ExpireMe()
	{
		//Debug.Log("EnemyMissileController:ExpireMe()");
		
		Instantiate(parExplosion, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
		
		if (parTrail != null)
		{
			parTrail.particleEmitter.minEmission = 0.0f;
			parTrail.particleEmitter.maxEmission = 0.0f;
			
			Destroy(parTrail, 1.7f);
		}
	}
	
	public void RemoveMe()
	{
		Destroy(gameObject);
		if (parTrail != null)
			Destroy(parTrail, 1.7f);
	}
	#endregion
	
//	public virtual void OnCollisionEnter(Collision col)
//	{
//		Debug.Log("EnemyMissileController:OnCollisionEnter() - (" + col.gameObject.tag + ") transform.position = " + transform.position);
//		
//		if (col.gameObject.tag == "Structure")
//		{
//			col.gameObject.GetComponent<StructureController>().TakeDamage(5.0f);
//		}
//		else if (col.gameObject.tag == "Turret")
//		{
//			
//		}	
//	}
}
