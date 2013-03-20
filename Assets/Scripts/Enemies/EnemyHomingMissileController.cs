using UnityEngine;
using System.Collections;

public class EnemyHomingMissileController : EnemyMissileController
{	
	public void Start()
	{
		autoDestroy = true;
		
		GameObject turret = GetNearestTurretObject(transform.position);
		if (turret != null)
		{
			turret.GetComponent<TurretController>().SetTarget();
				
			target = turret.transform.position;	
		}
		
		base.Start();
	}
	
	void Update()
	{
		base.Update();
	}
	
	GameObject GetNearestTurretObject(Vector3 position)
	{
		GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
		
		float nearestDistanceSqr = Mathf.Infinity;
		GameObject nearestTurret = null;
		
		foreach (GameObject turret in turrets)
		{
			// Skip any turrets that are destroyed
			if (turret.GetComponent<TurretBarrelController>().State == TurretState.DESTROYED)
				continue;
			
			Vector3 turretPos = turret.transform.position;
			float distanceSqr = (turretPos - position).sqrMagnitude;
			
			if (distanceSqr < nearestDistanceSqr)
			{
				nearestTurret = turret;
				nearestDistanceSqr = distanceSqr;
			}
		}
		
		return nearestTurret;
	}
}
