using UnityEngine;
using System.Collections;

public enum PlaneDirection
{
	LEFT = -1,
	RIGHT = 1
};

public class EnemyPlaneController : MonoBehaviour 
{
	public float maxHealth = 100.0f;
	public float bombSpawnInterval = 0.1f;
	public GameObject parVaporTrail;
	public GameObject parExplosion;
	public GameObject objBomb;
	public GameObject objMissile;
	
	private float health = 0.0f;
	private float speed = 50.0f;
	private float speedModifier = 0.0f;
	private Transform trans;
	private PlaneDirection direction = PlaneDirection.LEFT;
	private GameObject parTrail;
	private tk2dCamera objCamera;
	private tk2dAnimatedSprite objSprite;
	
	private Transform trailTrans;
	private Transform launcherTrans;
	private float lastBombSpawn;
	
	#region Properties
	public PlaneDirection Direction
	{
		get { return direction; }
		set { direction = value; }
	}
	
	public float Speed
	{
		get { return speed; }
		set { speed = value; }
	}
	
	public float SpeedModifier
	{
		get { return speedModifier; }
		set { speedModifier = value; }
	}
	#endregion
	
	void Start() 
	{
		//Debug.Log("EnemyPlaneController:Start() - direction = " + direction);
		
		objCamera = (tk2dCamera) GameObject.FindWithTag("MainCamera").GetComponent<tk2dCamera>();
		objSprite = GetComponent<tk2dAnimatedSprite>();
		
		trans = transform;
		health = maxHealth;
		trailTrans = transform.root.Find("TrailPosition"); 
		launcherTrans = transform.root.Find("LauncherPosition");
		
		parTrail = (GameObject)Instantiate(parVaporTrail, trailTrans.position, Quaternion.identity);
		
		if (direction == PlaneDirection.RIGHT)
		{
			objSprite.FlipX();
		}
	}
	
	void Update() 
	{
		float elapsed = Time.time;
		
		float moveSpeed = speed * Time.deltaTime;
		
		transform.Translate((transform.right * ((int)direction)) * (Time.deltaTime * speed),  Space.World);
		
		parTrail.transform.position = new Vector3(trailTrans.position.x, trailTrans.position.y, 4);
		
		if ((elapsed - lastBombSpawn) >= bombSpawnInterval)
		{
			lastBombSpawn = elapsed;
			
			int x = Random.Range(1, 10);
			
			if ((x >= 1) && (x <= 3))
			{
				LaunchMissile();
			}
			else 
			{
				DropBomb();
			}
		}
		
		CheckIfOffScreen();
		
		if (health <= 0.0f)
		{
			ExpireMe();	
		}
	}
	
	public void TakeDamage(float amount)
	{
		health -= amount;
		health = Mathf.Clamp(health, 0.0f, maxHealth);
	}	
		
	
	void DropBomb()
	{
		Transform targetPosition = GameObjectUtilts.GetNearestObjectTransformByTag(transform.position, "Turret");
		if (targetPosition == null)
			return;
		
		//Vector3 dir = (transform.right * ((int)direction));
		Vector3 dir = -transform.up;
		dir.Normalize();
		
		GameObject objCreatedBomb = (GameObject)Instantiate(objBomb, 
			launcherTrans.position, 
			Quaternion.identity);
		
		float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion rotation =  Quaternion.Euler(0f, 0f, rotZ - 90);
		
		objCreatedBomb.GetComponent<EnemyBombController>().Direction = dir;
		objCreatedBomb.GetComponent<EnemyBombController>().Rotation = rotation;
	}
	
	void LaunchMissile()
	{
		Transform targetPosition = GameObjectUtilts.GetNearestObjectTransformByTag(transform.position, "Turret");
		if (targetPosition == null)
			return;
		
		Vector3 dir = (transform.right * ((int)direction));
		dir.Normalize();
		
		GameObject objCreatedMissile = (GameObject)Instantiate(objMissile, 
			launcherTrans.position, 
			Quaternion.identity);
		
		float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion rotation =  Quaternion.Euler(0f, 0f, rotZ - 90);
		
		objCreatedMissile.GetComponent<EnemyHomingMissileController>().Target = targetPosition.position;
		objCreatedMissile.GetComponent<EnemyHomingMissileController>().Direction = dir;
		objCreatedMissile.GetComponent<EnemyHomingMissileController>().Rotation = rotation;
		objCreatedMissile.GetComponent<EnemyHomingMissileController>().moveSpeed = 100.0f;
	}
	
	private Quaternion GetForwardRotation(out Vector3 diff)
	{
		diff = transform.position - launcherTrans.position;
		
		diff.Normalize();
		
		float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		
		return Quaternion.Euler(0f, 0f, rotation + 90);	
	}
	
	private Quaternion GetTargetRotation(Vector3 targetPosition, out Vector3 diff)
	{
		diff = targetPosition - launcherTrans.position;
		
		diff.Normalize();
		
		float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		
		return Quaternion.Euler(0f, 0f, rotation + 90);	
	}
	
	void CheckIfOffScreen()
	{
		Vector3 screenPos = objCamera.mainCamera.ScreenToWorldPoint(transform.position);
		
		if (screenPos.x <= -50)
		{
			//Debug.Log("EnemyPlaneController:CheckIfOffScreen()");
			ExpireMe();
		}
		
		if (screenPos.x >= (Screen.width + 50))
		{
			//Debug.Log("EnemyPlaneController:CheckIfOffScreen()");
				
			ExpireMe();
		}
	}
	
	#region Destroy Self	
	public void ExpireMe()
	{
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
}
