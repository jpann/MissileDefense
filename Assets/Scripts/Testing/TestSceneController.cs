using UnityEngine;
using System.Collections;

public class TestSceneController : MonoBehaviour 
{
	public GameObject objDummyTarget;
	public GameObject objBullet;
	public GameObject objEnemyBullet;
	public GameObject objEnemyBomb;
	public GameObject objSwarmMissile;
	public GameObject objSmallMissile;
	
	enum MissileType
	{
		Bullet,
		EnemyBullet,
		EnemyBomb,
		SwarmMissile,
		SmallMissile
	};
	
	private MissileType typeOfProjectileToFire = MissileType.SmallMissile;
	
	void Start () 
	{
	
	}
	
	void Update () 
	{
		ProcessInput();
	}
	
	void ProcessInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			switch (typeOfProjectileToFire)
			{
				case MissileType.SmallMissile:
					FireSmallMissile();
					break;
				case MissileType.SwarmMissile:
					FireSwarmMissile();
					break;
				case MissileType.EnemyBomb:
					FireEnemyBomb();
					break;
				case MissileType.EnemyBullet:
					FireEnemyMissile();
					break;
				case MissileType.Bullet:
					FireMissile();
					break;
				default:
					FireSwarmMissile();
					break;
			}
		}
	}
	
	#region Button Methods
	void MainMenu()
	{
		Application.LoadLevel("MainMenu");	
	}
	
	void UseBullet()
	{
		typeOfProjectileToFire = MissileType.Bullet;
	}
	
	void UseEnemyBullet()
	{
		typeOfProjectileToFire = MissileType.EnemyBullet;
	}
	
	void UseEnemyBomb()
	{
		typeOfProjectileToFire = MissileType.EnemyBomb;
	}
	
	void UseSwarmMissile()
	{
		typeOfProjectileToFire = MissileType.SwarmMissile;
	}
	
	void UseSmallMissile()
	{
		typeOfProjectileToFire = MissileType.SmallMissile;
	}
	#endregion
	
	#region Direction Methods
	Vector3 GetFirePosition()
	{
		return transform.position;	
	}
	
	private Quaternion GetMouseFireRotation(Vector3 position, out Vector3 diff, float offset)
	{
		Vector3 targetPosition = Input.mousePosition;
		
		diff = targetPosition - position;
		
		diff.Normalize();
		
		float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		
		return Quaternion.Euler(0f, 0f, rotation + offset);	
	}
	#endregion
	
	#region Projectile Firing Methods
	void FireMissile()
	{
		
	}
	
	void FireSmallMissile()
	{
		Vector3 fromPosition = GetFirePosition();
		Vector3 diff;
		
		Quaternion rotation = GetMouseFireRotation(fromPosition, out diff, 90);
		
		GameObject objProjectile = (GameObject)Instantiate(objSmallMissile, 
			fromPosition, 
			Quaternion.identity);
	}
	
	void FireSwarmMissile()
	{
		Vector3 fromPosition = GetFirePosition();
		Vector3 diff;
		
		Quaternion rotation = GetMouseFireRotation(fromPosition, out diff, 90);
		
		GameObject objProjectile = (GameObject)Instantiate(objSwarmMissile, 
			fromPosition, 
			Quaternion.identity);
	}
	
	void FireEnemyMissile()
	{
		
	}
	
	void FireEnemyBomb()
	{
		
	}
	#endregion
}
