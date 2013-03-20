using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private tk2dCamera objCamera;
	private GameObject objPlayer;
	private VariableScript ptrScriptVariable;
	private GameObject[] turrets;
	
	private GameObject enemySpawner;
	private GameInformation objGameInfo;
	
	#region Properties
	public GameInformation GameInfo
	{
		get { return objGameInfo; }	
	}
	#endregion
	
	void Start() 
	{
		objCamera = (tk2dCamera) GameObject.FindWithTag("MainCamera").GetComponent<tk2dCamera>();
		objPlayer = (GameObject)GameObject.FindWithTag("Player");
		ptrScriptVariable = (VariableScript)objPlayer.GetComponent(typeof(VariableScript));
		objGameInfo = GameObject.FindGameObjectWithTag("GameInformation").GetComponent<GameInformation>();
		
		// Get turret GameObjects
		turrets = GameObject.FindGameObjectsWithTag("Turret");
		//Debug.Log("PlayerController:Start - turrets.Count = " +turrets.Length);
		
		enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");
	}
	
	void Update() 
	{
		// Aim turrets at mouse cursor
		foreach (GameObject turret in turrets)
		{
			// Skip any turrets that are destroyed
			if (turret.GetComponent<TurretBarrelController>().State == TurretState.DESTROYED)
				continue;
			
			turret.GetComponent<TurretBarrelController>().AimAt(Input.mousePosition);
		}
		
		HandleInput();
		
		if (CheckForGameOver())
		{
			StartCoroutine("LoadGameOver");
		}
	}
	
	#region Input Methods
	void HandleInput()
	{
		Vector3 mousePosition = objCamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		
		// Left mouse click
		if (Input.GetMouseButtonDown(0))
		{
			GameObject turret = GetNearestTurretObject(mousePosition);
			
			if (turret != null)
			{
				//Debug.Log("PlayerController:HandleInput:GetMouseButtonDown - turret = " + turret.name);	
				
				turret.GetComponent<TurretBarrelController>().Fire(mousePosition);
			}
			else
			{
				//Debug.Log("PlayerController:HandleInput:GetMouseButtonDown - turret == null");	
			}
		}
		// Right mouse click
		else if (Input.GetMouseButtonDown(1))
		{
			if (enemySpawner != null)
			{
				//enemySpawner.GetComponent<EnemySpawner>().Spawn();
				enemySpawner.GetComponent<EnemySpawner>().FireAtMouse();
			}
			else
			{
				//Debug.Log("PlayerController:HandleInput - enemySpawner == null");	
			}
		}
		else if (Input.GetKeyDown(KeyCode.F1))
		{
			StartCoroutine("LoadGameOver");	
		}
	}
	#endregion
	
	#region Turret Methods
	public void HealTurretsByAmount(float amount)
	{
		foreach (GameObject turret in turrets)
		{
			// Skip any turrets that are destroyed
			if (turret.GetComponent<TurretBarrelController>().State == TurretState.DESTROYED)
				continue;
			
			turret.GetComponent<TurretController>().Heal(amount);
		}
	}
	
	public void HealTurretsByPercent(float percent)
	{
		foreach (GameObject turret in turrets)
		{
			// Skip any turrets that are destroyed
			if (turret.GetComponent<TurretBarrelController>().State == TurretState.DESTROYED)
				continue;
			
			turret.GetComponent<TurretController>().HealByPercent(percent);
		}
	}
	
	public void ReviveTurrets()
	{
		
	}
	
	GameObject GetNearestTurretObject(Vector3 position)
	{
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
	#endregion
	
	bool CheckForGameOver()
	{
		int count = turrets.Length;	
		int destroyedCount = 0;
		
		for (int i = 0; i < count; i++)
		{
			if (turrets[i].GetComponent<TurretBarrelController>().State == TurretState.DESTROYED)
				destroyedCount++;
		}
		
		if (destroyedCount >= count)
			return true;
		else
			return false;
	}
	
	IEnumerator LoadGameOver()
	{
		yield return new WaitForSeconds(2);
		
		//objGameInfo.PostPlayerScore("jasper", PlayerController.score);
		
		Application.LoadLevel("GameOver");	
	}
}
