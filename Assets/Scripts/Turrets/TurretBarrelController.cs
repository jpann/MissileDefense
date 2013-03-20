using UnityEngine;
using System.Collections;

public class TurretBarrelController : MonoBehaviour 
{
	public float minRotation = 23.0f;
	public float maxRotation = 160.0f;
	public float maxHealth = 100.0f;
	public GameObject parBarrelFlash;
	
	private tk2dSprite objSprite;
	private GameObject objPlayer;
	private VariableScript objScriptVariable;
	private tk2dCamera objCamera;
	
	private Vector3 inputDirection;
	private GameObject objBarrel;
	private Transform missileOrigin;
	public float health;
	private TurretState state = TurretState.FULL;
	private GameObject parMyBarrelFlash;
	
	#region Properties
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
		objCamera = (tk2dCamera) GameObject.FindWithTag("MainCamera").GetComponent<tk2dCamera>();
		
		objPlayer = (GameObject) GameObject.FindWithTag("Player");
		objScriptVariable = (VariableScript)objPlayer.GetComponent(typeof(VariableScript));
		objSprite = GetComponent<tk2dSprite>();

		objBarrel = GameObjectUtilts.FindChild(gameObject.name, "Barrel");
		missileOrigin = objBarrel.transform.Find("BulletOrigin");

		health = maxHealth;
	}
	
	void Update() 
	{	
		//parMyFireFlash.transform.position = bulletOrigin.position;
	}
	
	#region Healing
	public void Heal(float amount)
	{
		this.Health = this.Health + amount;	
		
		state = TurretStateManager.GetState(health);
		
		if (objSprite == null)
			return;
		
		objSprite.color = TurretStateManager.GetStateColor(state);
	}
	#endregion
	
	#region Damage
	public void TakeDamage(float amount)
	{
		//Debug.Log("TurretBarrelController:TakeDamage() - amount = " + amount);	
		
		health -= amount;
		
		health = Mathf.Clamp(health, 0.0f, maxHealth);
		
		state = TurretStateManager.GetState(health);
		
		if (objSprite == null)
			return;
		
		objSprite.color = TurretStateManager.GetStateColor(state);
		
		GameInfoManager.Instance.IncrementDamageReceived(amount);
	}
	#endregion
	
	#region Shooting Projectiles	
	public void Fire(Vector3 target)
	{
		//Debug.Log("TurretBarrelController:Fire()");
		
		GameObject objCreatedMissile = (GameObject)Instantiate(objScriptVariable.objMissile, 
			missileOrigin.position, 
			Quaternion.identity);
		
		Vector3 direction = objBarrel.transform.up;
		direction.Normalize();
		
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation =  Quaternion.Euler(0f, 0f, rotZ - 90);
		
		//objCreatedMissile.GetComponent<TestMissileController>().Target = target;
		objCreatedMissile.GetComponent<MissileController>().Direction = direction;
		objCreatedMissile.GetComponent<MissileController>().Owner = objPlayer;
		objCreatedMissile.GetComponent<MissileController>().Rotation = rotation;
		
		GameInfoManager.Instance.IncrementMissilesFired();
	}
	#endregion
	
	#region Aiming	
	public void AimAt(Vector3 target)
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
}
