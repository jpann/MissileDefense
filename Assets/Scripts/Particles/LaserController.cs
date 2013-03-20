using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour 
{
	private tk2dCamera objCamera;
	private LineRenderer objLineRenderer;
	
	public bool isLaserShowing = false;
	
	void Start()
	{
		objCamera = (tk2dCamera) GameObject.FindWithTag("MainCamera").GetComponent<tk2dCamera>();
		objLineRenderer = GetComponent<LineRenderer>();
	}
	
	void Update()
	{
		Vector3 mousePosition = objCamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		
		Ray ray = objCamera.mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			Vector3 pos = ray.origin + ray.direction * hit.distance;
			objLineRenderer.SetPosition(0, transform.position);
			objLineRenderer.SetPosition(1, pos);
			
			Debug.DrawLine(transform.position, pos, Color.red);
		}
	}
	
	public IEnumerator ShowLaser()
	{
		if (!isLaserShowing)
		{
			isLaserShowing = true;
			this.renderer.enabled = true;
			
			yield return new WaitForSeconds(0.05f);
			ResetLaser();
			isLaserShowing = false;
		}
	}
	
	public void ResetLaser()
	{
		this.renderer.enabled = false;	
	}
	
}
