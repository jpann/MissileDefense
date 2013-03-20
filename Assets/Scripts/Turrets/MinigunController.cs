using UnityEngine;
using System.Collections;

public class MinigunController : MonoBehaviour 
{
	public GameObject objProjectile;
	public float turnSpeed = 1.0f;
	public float rateOfFire = 1.0f;
	public float maxHealth = 100.0f;
	public float detectionRadius = 100.0f;
	public float checkForTargetTime = 0.5f;
	public float targetTrackingTime = 1.0f;
	public LayerMask trackingLayer;
	public float timeUntilActive = 1.0f;
	public float timeToLive = 6.0f;
	public float timeTilExpire = 1.0f;
	public bool isDisabled = false;
	
	private tk2dSprite objSprite;
	private GameObject objBarrel;
	private float health;
	private bool isActive = false;
	private float start;
	private bool isExpiring = false;
	private float expireTime;
	private TurretState state = TurretState.FULL;
	
	// Firing
	private float lastFiredTime;
	private Transform barrelOrigin0;
	private Transform barrelOrigin1;
	
	// Target checking
	private float lastTargetCheckTime;
	private bool isTargetTracking = false;
	
	// Target tracking
	private Transform target;
	private float targetTrackingStartTime;
	
	// Aiming
	private float minRotation = 23.0f;
	private float maxRotation = 160.0f;
	
	#region Public Properties
	public bool IsActive
	{
		get { return isActive; }
		set { isActive = value; }
	}
	public TurretState State
	{
		get { return state; }	
	}
	
	public float Health
	{
		get { return health; }
		set
		{
			health = Mathf.Clamp(value, 0.0f, maxHealth);	
		}
	}
	
	#endregion
	
	void Start() 
	{
		//Debug.Log("MinigunController:Start(" + gameObject.name + ")");
		
		start = Time.time;
		health = maxHealth;
		
		objSprite = GetComponent<tk2dSprite>();
	
		// Get child barrel object which is what rotates and fires projectiles
		objBarrel = GameObjectUtilts.FindChild(gameObject.name, "Barrel");
		
		//TODO I think the bullet origin should be stored as a Transform array so
		// more it does not have to be hard coded.
		
		// Get the origin for projectiles on the barrel object.
		// This is where projectiles shot from this gun will originate.
		barrelOrigin0 = objBarrel.transform.Find("BarrelOrigin0");
		barrelOrigin1 = objBarrel.transform.Find("BarrelOrigin1");
		
		// Only track objects on this layer.
		trackingLayer = LayerMask.NameToLayer("Enemies");
		
	}
	
	void Update() 
	{
		float elapsed = Time.time;
		
		// Check if the turret is ready to be active
		if ((elapsed - start) >= timeUntilActive)
			isActive = true;
		
		if (!isActive)
			return;
		
		if (isExpiring)
		{
			if ((elapsed - expireTime) >= timeTilExpire)
			{
				ExpireMe();
			}
			
			return;
		}
		
		// If we're not tracking
		if (!isTargetTracking)
		{
			// ..see if it is time to check for a new target
			if ((elapsed - lastTargetCheckTime) >= checkForTargetTime)
			{
				lastTargetCheckTime = elapsed;
				
				// Check if there is a target within our search radius
				CheckForTargetsInRadius();
			}
		}
		
		// If we're currently tracking a target
		if (isTargetTracking)
		{
			if (target != null)
				Debug.DrawLine(barrelOrigin0.position, target.position, Color.green);
			
			// Track the current target by having the barrel's local rotation follow it
			TrackTarget();
			
			// After a certain amount of time we stop tracking a target
			// because we don't want to have the turret only track a single target
			if ((elapsed - targetTrackingStartTime) >= targetTrackingTime)
			{
				ResetTracking();	
			}
			
			// If we're within the rate of fire
			if ((elapsed - lastFiredTime) >= rateOfFire)
			{
				// ...fire in the direction the barrel is pointed
				Fire();	
				
				lastFiredTime = elapsed;
			}
		}
		
		// If the turret has lived long enough, expire it
		if ((elapsed - start) >= timeToLive)
		{
			objSprite.color = Color.red;
			
			isExpiring = true;
			expireTime = Time.time;
		}
	}
	
	#region Damage Methods
	public void TakeDamage(float amount)
	{
		Debug.Log("MinigunController:TakeDamage() - amount = " + amount);
		
		health -= amount;
		
		health = Mathf.Clamp(health, 0.0f, maxHealth);
		
		state = TurretStateManager.GetState(health);
		
		if (objSprite == null)
			return;
		
		UpdateSprite();
	}
	
	private void UpdateSprite()
	{
		string sName = "full";
		
		if (health >= 100.0f)
			sName = "full";
		else if (health >= 90)
			sName = "1";
		else if (health >= 80)
			sName = "2";
		else if (health >= 70)
			sName = "3";
		else if (health >= 60)
			sName = "4";
		else if (health >= 50)
			sName = "5";
		else if (health >= 40)
			sName = "6";
		else if (health >= 30)
			sName = "7";
		else if (health >= 20)
			sName = "8";
		else if (health >= 10)
			sName = "9";
		else if (health >= 0)
			sName = "9";
		
		objSprite.SetSprite(sName);
	}
	#endregion
	
	#region Fire Methods
	private void Fire()
	{
		//Debug.Log("MinigunController:Fire(" + gameObject.name + ")");
		
		if (isDisabled)
			return;
		
		Vector3 pos0 = barrelOrigin0.position;
		Vector3 pos1 = barrelOrigin1.position;
		
		// First bullet
		GameObject objBullet = (GameObject)Instantiate(objProjectile, 
			pos0, 
			Quaternion.identity);
		
		//Debug.Log("MinigunController:Fire(" + gameObject.name + ") 1");
		
		Vector3 direction = objBarrel.transform.up;
		direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation =  Quaternion.Euler(0f, 0f, rotZ - 90);
		
		objBullet.GetComponent<GunBulletController>().Direction = direction;
		objBullet.GetComponent<GunBulletController>().Owner = gameObject;
		objBullet.GetComponent<GunBulletController>().Rotation = rotation;
		
		//Debug.Log("MinigunController:Fire(" + gameObject.name + ") 2");
		
		// Second bullet
		objBullet = (GameObject)Instantiate(objProjectile, 
			pos1, 
			Quaternion.identity);
		
		//Debug.Log("MinigunController:Fire(" + gameObject.name + ") 3");
		
		direction = objBarrel.transform.up;
		direction.Normalize();
		
		rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		rotation =  Quaternion.Euler(0f, 0f, rotZ - 90);
		
		objBullet.GetComponent<GunBulletController>().Direction = direction;
		objBullet.GetComponent<GunBulletController>().Owner = gameObject;
		objBullet.GetComponent<GunBulletController>().Rotation = rotation;
		
		Debug.DrawRay(pos0, direction * 500, Color.yellow, 0);
		Debug.DrawRay(pos1, direction * 500, Color.yellow, 0);
		
		//Debug.Log("MinigunController:Fire(" + gameObject.name + ") 4");
	}
	#endregion
	
	#region Target checking methods
	private void ResetTracking()
	{
		//Debug.Log("MinigunController:ResetTracking(" + gameObject.name + ")");
		
		isTargetTracking = false;
		target = null;
	}
	
	void CheckForTargetsInRadius()
	{
		//Debug.Log("MinigunController:CheckForTargetsInRadius(" + gameObject.name + ") - trackingLayer = " + LayerMask.LayerToName(trackingLayer.value));
		
		float nearestDistanceSqr = Mathf.Infinity;
		
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, detectionRadius, 1 << trackingLayer);

		foreach (Collider col in objectsInRange)
		{
//			Debug.Log("MinigunController:CheckForTargetsInRadius() - Name = " + col.gameObject.name +
//				"; Tag = " + col.gameObject.tag +
//				"; Layer = " + col.gameObject.layer);
			
			if (col.gameObject.tag  != "Projectile")
				return;
			
			Vector3 targetPos = col.gameObject.transform.position;
			float distanceSqr = (targetPos - transform.position).sqrMagnitude;
			
			if (distanceSqr < nearestDistanceSqr)
			{
				nearestDistanceSqr = distanceSqr;
				
				target = col.gameObject.transform;
			
				isTargetTracking = true;
				targetTrackingStartTime = Time.time;
			
				//Debug.Log("MinigunController:CheckForTargetsInRadius(" + gameObject.name + ") - Target found [" + col.gameObject.tag + "]");
			}
		}
	}	
	#endregion
	
	#region Aiming Methods
	private void TrackTarget()
	{
		if (!isTargetTracking || target == null)
			return;
		
		//Debug.Log("MinigunController:TrackTarget()" + gameObject.name + ")");
		
//		Vector3 direction = target.position - objBarrel.transform.position;
//		direction.Normalize();
//		
//		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//		Quaternion rot = Quaternion.Euler(0f, 0f, rotZ - 90);
//		
//		objBarrel.transform.localRotation = Quaternion.Slerp(
//			objBarrel.transform.rotation, 
//			rot, 
//			turnSpeed * Time.deltaTime);
		
		Vector3 direction = target.position - barrelOrigin0.position;
		direction.Normalize();
		
		direction += direction.magnitude *
			target.transform.up * 
			50 / 500;
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rot = Quaternion.Euler(0f, 0f, rotZ - 90);

		objBarrel.transform.localRotation = Quaternion.Slerp(
			barrelOrigin0.rotation, 
			rot, 
			turnSpeed * Time.deltaTime);
		
		Debug.DrawRay(barrelOrigin0.position, direction * 500, Color.red, 0);
	}
	
	private void AimAt(Vector3 target)
	{
		Vector3 diff = target - transform.position;
		
		// Always normalize the difference vector before using Atan2 function
		diff.Normalize();

		// calculate the Z rotation angle using atan2
		// Atan2 will give you an angle in radians, so you
		// must use Rad2Deg constant to convert it to degrees
		float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		
		if ((rotZ > maxRotation) || ((rotZ < -90) && (rotZ > -180.0f)))
			rotZ = maxRotation;
		
		if (rotZ < minRotation)
			rotZ = minRotation;
		
		if (objBarrel != null)
		{
			//Debug.Log("Turret.Barrel (" + gameObject.name + ") Rotation.Z = " + rotZ);
			
			objBarrel.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
		}
		else
		{
			//Debug.Log("TurretBarrelController:AimAt - (" + gameObject.name + ") barrel == null");
		}	
	}
	#endregion
	
	#region Destroy Self
	public void ExpireMe()
	{
		Debug.Log("MinigunController:ExpireMe()");
		
		Destroy(gameObject);
	}
	
	public void RemoveMe()
	{
		Debug.Log("MinigunController:RemoveMe()");
		
		Destroy(gameObject);
	}
	#endregion
}
