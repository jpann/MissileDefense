using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour 
{
	public GameObject parSmFire;
	public GameObject parFire;
	public GameObject parSmSmoke;
	public GameObject parMdSmoke;
	public GameObject parLgSmoke;
	public GameObject parHealthEffect;
	
	public GameObject objTurretTarget;
	private GameObject objTarget;
	private float targetSetLimit = 3.0f;
	private float targetLastTimeSet;
	
	private TurretBarrelController controller;
	private GameObject parMySmFire;
	private GameObject parMyFire;
	private GameObject parMySmSmoke;
	private GameObject parMyMdSmoke;
	private GameObject parMyLgSmoke;
	private GameObject parHealth;
	
	void Start() 
	{
		controller = gameObject.GetComponentInChildren<TurretBarrelController>();
		
		// Particles
		parMySmFire = (GameObject)Instantiate(parSmFire, transform.position, Quaternion.identity);
		parMyFire = (GameObject)Instantiate(parFire, transform.position, Quaternion.identity);
		parMySmSmoke = (GameObject)Instantiate(parSmSmoke, transform.position, Quaternion.identity);
		parMyMdSmoke = (GameObject)Instantiate(parMdSmoke, transform.position, Quaternion.identity);
		parMyLgSmoke = (GameObject)Instantiate(parLgSmoke, transform.position, Quaternion.identity);
		
		parHealth = (GameObject)Instantiate(parHealthEffect, transform.position, Quaternion.identity);
		
		parMySmFire.transform.position = new Vector3(parMySmFire.transform.position.x, parMySmFire.transform.position.y, 4);
		parMyFire.transform.position = new Vector3(parMyFire.transform.position.x, parMyFire.transform.position.y, 4);
		parHealth.transform.position = new Vector3(parHealth.transform.position.x, parHealth.transform.position.y, 4);
		
		parMySmFire.particleEmitter.emit = false;
		parMyFire.particleEmitter.emit = false;
		parMySmSmoke.particleEmitter.emit = false;
		parMyMdSmoke.particleEmitter.emit = false;
		parMyLgSmoke.particleEmitter.emit = false;
		parHealth.particleEmitter.emit = false;
	}
	
	void Update()
	{
		UpdateParticles();
	}
	
	#region Health Methods
	public void Heal(float amount)
	{
		//Debug.Log("TurretController:Heal() - amount = " + amount);
		
		controller.Heal(amount);
		
		EmitOnHealParticle();
	}
	
	public void HealByPercent(float percent)
	{
		percent = Mathf.Clamp(percent, 0.00f, 100.0f);
		
		float amount = MathUtils.GetPercentOf(percent, controller.maxHealth);
		
		controller.Heal(amount);
		
		EmitOnHealParticle();
	}
	#endregion
	
	public void SetTarget()
	{
		//Debug.Log("TurretController:SetTarget() - targetLastTimeSet = " + targetLastTimeSet);
		
		float elapsed = Time.time;
		
		if (((elapsed - targetLastTimeSet) >= targetSetLimit) || targetLastTimeSet == 0.0f)
		{
			targetLastTimeSet = elapsed;
			
			Instantiate(objTurretTarget, 
				transform.position, 
				Quaternion.identity);
		}
		else
		{
			//Debug.Log("TurretController:SetTarget() - Too soon! elapsed = " + elapsed);
		}
	}
	
	#region Particle Methods
	void UpdateParticles()
	{
		if (controller.State == TurretState.SLIGHTLY_DAMAGED)
		{
			parMySmSmoke.particleEmitter.emit = true;
		}
		else if (controller.State == TurretState.DAMAGED)
		{
			parMySmFire.particleEmitter.emit = true;
			parMySmSmoke.particleEmitter.emit = true;
		}
		else if (controller.State == TurretState.MORE_DAMAGED)
		{
			parMySmFire.particleEmitter.emit = true;
			parMySmSmoke.particleEmitter.emit = true;
		}
		else if (controller.State == TurretState.HEAVILY_DAMAGED)	
		{
			parMySmFire.particleEmitter.emit = false;
			parMyFire.particleEmitter.emit = true;
			parMySmSmoke.particleEmitter.emit = false;
			parMyMdSmoke.particleEmitter.emit = false;
			parMyLgSmoke.particleEmitter.emit = true;
		}
		else if (controller.State == TurretState.DESTROYED)
		{
			parMySmFire.particleEmitter.emit = false;
			parMyFire.particleEmitter.emit = true;
			parMySmSmoke.particleEmitter.emit = false;
			parMyMdSmoke.particleEmitter.emit = false;
			parMyLgSmoke.particleEmitter.emit = true;
		}
		else
		{
			parMySmFire.particleEmitter.emit = false;
			parMyFire.particleEmitter.emit = false;
			parMySmSmoke.particleEmitter.emit = false;
			parMyMdSmoke.particleEmitter.emit = false;
			parMyLgSmoke.particleEmitter.emit = false;
		}
	}
	
	void EmitOnHealParticle()
	{
		parHealth.particleEmitter.Emit();
	}
	#endregion
}
