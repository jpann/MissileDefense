using UnityEngine;
using System.Collections;

public class TorqueLookAt : MonoBehaviour 
{
	public Transform target;
	public float force = 0.1f;

	void Update() 
	{
		Vector3 targetDelta = target.position - transform.position;
		
		//get the angle between transform.forward and target delta
		float angleDiff = Vector3.Angle(transform.forward, targetDelta);
                           
		// get its cross product, which is the axis of rotation to
		// get from one vector to the other
		Vector3 cross = Vector3.Cross(transform.forward, targetDelta);
 
		// apply torque along that axis according to the magnitude of the angle.
		rigidbody.AddTorque(cross * angleDiff * force);	
	}
}
