using UnityEngine;
using System.Collections;

public class StructureController : MonoBehaviour 
{
	public float startHealth = 100.0f;
	
	private tk2dSprite sprite;
	private float health = 0.0f;
	private bool isAlive = true;
	public tk2dTextMesh healthText;
	
	void Start() 
	{
		sprite = GetComponent<tk2dSprite>();
		//healthText = FindChild(gameObject.name, "HealthText").GetComponent<tk2dTextMesh>();
		
		health = startHealth;
		isAlive = true;
	}
	
	void Update() 
	{		
		if (health <= 0.0f)
		{
			isAlive = false;	
			
			sprite.color = Color.red;
			
			RemoveMe();
		}
		
		if (healthText != null)
		{
			healthText.text = string.Format("{0:0.0}", health);	
			healthText.Commit();
		}
		else
		{
			//Debug.Log("StructureController:Update() - healthText == null");	
		}
	}	
	
	#region Collision
	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("StructureController:OnCollisionEnter() - Other = " + col.gameObject.name);			
	}
	#endregion
	
	#region Damage
	public void TakeDamage(float amount)
	{
		health -= amount;
		
		if (health <= 0.0f)
			health = 0.0f;
	}
	#endregion
	
	void RemoveMe()
	{
		Destroy (gameObject);	
	}
	
	private GameObject FindChild(string pRoot, string pName)
	{
	    Transform pTransform = GameObject.Find(pRoot).GetComponent<Transform>();
	
	    foreach (Transform trs in pTransform) 
		{
	        if (trs.gameObject.name == pName)
	
	            return trs.gameObject;
	    }
	
	   return null;
	}
}
