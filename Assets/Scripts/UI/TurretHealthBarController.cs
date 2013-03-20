using UnityEngine;
using System.Collections;

public class TurretHealthBarController : MonoBehaviour 
{
	public float barDisplay;

	private TurretBarrelController controller;
	private tk2dClippedSpriteSample sprite;
	
	void Start()
	{
		controller = transform.parent.gameObject.GetComponent<TurretBarrelController>();

		sprite = gameObject.GetComponentInChildren<tk2dClippedSpriteSample>();
		
	}
	
	void OnGUI()
	{
		
	}
	
	void Update() 
	{
		barDisplay = controller.health / controller.maxHealth;
		
		sprite.clipTopRight = new Vector2(barDisplay, 1);
		sprite.color = TurretStateManager.GetStateColor(controller.State);
		
	}
}
