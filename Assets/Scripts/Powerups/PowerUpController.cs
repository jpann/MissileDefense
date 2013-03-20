using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour 
{
	public float fallForce = 100.0f;
	public float timeToLive = 25.0f;
	
	public virtual void Start () 
	{
	
	}
	
	public virtual void Update ()
	{
	
	}
	
	public virtual void FixedUpdate()
	{
		rigidbody.AddForce(-Vector3.up * fallForce);  
	}
	
	#region Destroy Self	
	public void ExpireMe()
	{
		Destroy(gameObject);
	}
	
	public void RemoveMe()
	{
		Destroy(gameObject);
	}
	#endregion
	
}
