using UnityEngine;
using System.Collections;

public class HealthBoxController : PowerUpController 
{
	public float healAmount = 100.0f;
	public GameObject parHealthEffect;
	
	private GameObject parHealth;
	
	void Start() 
	{
		parHealth = (GameObject)Instantiate(parHealthEffect, transform.position, Quaternion.identity);
		
		base.Start();
	}
	
	void Update() 
	{
		parHealth.transform.position = new Vector3(transform.position.x, transform.position.y, 4);
		
		base.Update();
	}
	
	public virtual void FixedUpdate()
	{
		base.FixedUpdate();
	}
	
	#region Destroy Self
	public void ExpireMe()
	{
		//Debug.Log("HealthBoxController:ExpireMe()");

		Destroy(gameObject);
		parHealth.particleEmitter.minEmission = 0.0f;
		parHealth.particleEmitter.maxEmission = 0.0f;
		
		Destroy(parHealth, 5);
	}
	
	public void RemoveMe()
	{
		//Debug.Log("HealthBoxController:removeMe()");
		
		Destroy(gameObject);
		Destroy(parHealth, 5);
	}
	#endregion
}
