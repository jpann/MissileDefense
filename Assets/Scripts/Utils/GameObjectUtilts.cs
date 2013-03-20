using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectUtilts
{
	public static Transform GetNearestObjectTransformByTag(Vector3 pSource, string pTag)
	{
		float nearestDistanceSqr = Mathf.Infinity;
		GameObject nearestObject = null;
		
		GameObject[] oObjects = GameObject.FindGameObjectsWithTag(pTag);
		
		foreach (GameObject oObject in oObjects)
		{			
			Vector3 objectPosition = oObject.transform.position;
			float distanceSqr = (objectPosition - pSource).sqrMagnitude;
			
			if (distanceSqr < nearestDistanceSqr)
			{
				nearestObject = oObject;
				nearestDistanceSqr = distanceSqr;
			}
		}
		
		if (nearestObject != null)
			return nearestObject.transform;
		else
			return null;
	}
	
	public static Vector3 GetNearestObjectPositionByTag(Vector3 pSource, string pTag)
	{
		float nearestDistanceSqr = Mathf.Infinity;
		GameObject nearestObject = null;
		
		GameObject[] oObjects = GameObject.FindGameObjectsWithTag(pTag);
		
		foreach (GameObject oObject in oObjects)
		{			
			Vector3 objectPosition = oObject.transform.position;
			float distanceSqr = (objectPosition - pSource).sqrMagnitude;
			
			if (distanceSqr < nearestDistanceSqr)
			{
				nearestObject = oObject;
				nearestDistanceSqr = distanceSqr;
			}
		}
		
		if (nearestObject != null)
			return nearestObject.transform.position;
		else
			return Vector3.zero;
	}
	
	#region Finding Child Objects
	public static GameObject FindChild(string pRoot, string pName)
	{
	    Transform pTransform = GameObject.Find(pRoot).GetComponent<Transform>();
	
	    foreach (Transform trs in pTransform) 
		{
	        if (trs.gameObject.name == pName)
	            return trs.gameObject;
	    }
	
	   return null;
	}
	
	public static GameObject[] FindChildren(string pRoot, string pName)
	{
		var objects = new List<GameObject>();
		
	    Transform pTransform = GameObject.Find(pRoot).GetComponent<Transform>();
	
	    foreach (Transform trs in pTransform) 
		{
	        if (trs.gameObject.name == pName)
				objects.Add(trs.gameObject);
	    }
	
	   return objects.ToArray();
	}
	#endregion
	
	#region Finding Child Transforms
	public static Transform FindChildTransform(string pRoot, string pName)
	{
		Transform pTransform = GameObject.Find(pRoot).GetComponent<Transform>();
	
	    foreach (Transform trs in pTransform) 
		{
	        if (trs.gameObject.name == pName)
	            return trs.gameObject.transform;
	    }
	
	   return null;	
	}
	
	public static Transform[] FindChildrenTransform(string pRoot, string pName)
	{
		var objects = new List<Transform>();
		
		Transform pTransform = GameObject.Find(pRoot).GetComponent<Transform>();
	
	    foreach (Transform trs in pTransform) 
		{
	        if (trs.gameObject.name == pName)
				objects.Add(trs.gameObject.transform);
	    }
	
	  return objects.ToArray();
	}
	#endregion
}
