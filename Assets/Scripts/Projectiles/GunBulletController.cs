using UnityEngine;
using System.Collections;

public class GunBulletController : MonoBehaviour 
{
	// Public variables defined in Inspector
	public float moveSpeed = 100.0f;
	public float damage = 10.0f;
	public float timeToLive = 20.0f;
	
	protected GameObject owner;
	
	protected float start;
	protected float timeSpentAlive;
	protected bool autoDestroy = true;
	
	// Rotation and movement
	protected Quaternion rotation;
	protected Vector3 direction;
	protected float speedModifier = 0.0f;
	
	#region Properties
	public GameObject Owner
	{
		get { return owner; }	
		set { owner = value; }
	}
	
	public Vector3 Direction
	{
		get { return direction; }	
		set { direction = value; }
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
	#endregion
	
	public virtual void Start ()
	{
		start = Time.time;
		
		this.transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);	
	}
	
	public virtual void Update () 
	{
		timeSpentAlive += Time.deltaTime;
	
		transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);
		
		transform.Translate(transform.up * (Time.deltaTime * (moveSpeed + speedModifier)),  Space.World);
		
		if (timeSpentAlive > timeToLive)
		{
			if (autoDestroy)
				ExpireMe();
		}
	}
	
	#region Destroy Self
	public void ExpireMe()
	{
		//Debug.Log("GunBulletController:ExpireMe()");
		
		Destroy(gameObject);
	}
	
	public void RemoveMe()
	{
		//Debug.Log("GunBulletController:removeMe()");
		
		Destroy(gameObject);
	}
	#endregion
}
