using UnityEngine;
using System.Collections;

public class MinigunBoxController : PowerUpController 
{
	public GameObject objMinigun;
	public float timeUntilDeploy = 1.0f;
	
	private tk2dAnimatedSprite objSprite;
	private bool waitToDeploy = false;
	private bool isDeploying = false;
	private float landTime;
	
	void Start() 
	{
		objSprite = GetComponent<tk2dAnimatedSprite>();
		objSprite.animationCompleteDelegate = HitCompleteDelegate;
		objSprite.animationEventDelegate = FrameDeploymentTrigger;
		
		base.Start();
	}
	
	void Update() 
	{
		float elapsed = Time.time;
		
		if (!isDeploying && waitToDeploy && ((elapsed - landTime) >= timeUntilDeploy))
		{
			objSprite.Play("Deploy");
			waitToDeploy = false;
			isDeploying = true;
		}
		
		base.Update();
	}
	
	public virtual void FixedUpdate()
	{
		base.FixedUpdate();
	}
	
	public void Deploy()
	{
		Debug.Log("MinigunBoxController:Deploy()");
		
		waitToDeploy = true;
		landTime = Time.time;
	}
	
	void FrameDeploymentTrigger(tk2dAnimatedSprite sprite, 
		tk2dSpriteAnimationClip clip, 
		tk2dSpriteAnimationFrame frame, 
		int frameNum)
    {
		//Debug.Log("MinigunBoxController:FrameDeploymentTrigger()");
		
		DeployGun();
	}
	
	void HitCompleteDelegate(tk2dAnimatedSprite sprite, int clipId)
    {
		//Debug.Log("MinigunBoxController:HitCompleteDelegate()");

		Destroy(gameObject);
    }
	
	private void DeployGun()
	{
		Debug.Log("MinigunBoxController:DeployGun()");
		
		Vector3 position = new Vector3(transform.position.x, transform.position.y, 1);
		
		GameObject objGun = (GameObject)Instantiate(objMinigun, 
			position, 
			Quaternion.identity);
	}
	
	#region Destroy Self
	public void ExpireMe()
	{
		Debug.Log("MinigunBoxController:ExpireMe()");
		
		Destroy(gameObject);
	}
	
	public void RemoveMe()
	{
		Debug.Log("MinigunBoxController:RemoveMe()");
		
		Destroy(gameObject);
	}
	#endregion
}
