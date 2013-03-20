using UnityEngine;
using System.Collections;

public class TurretTargetController : MonoBehaviour 
{
	tk2dSprite objSprite;
	
	private float timeToLive = 2.0f;
	private float rotationSpeed = 100.0f;
	private float scaleSpeed = 2.0f;
	private float maxScale = 5.0f;
	private float minScale = 0.5f;
	private float fade;
	private Color objColor;
	
	private bool isFadingOut = false;
	private bool isFadedOut = false;
	private bool isFadingIn = false;
	private bool isFadedIn = false;
	
	void Start() 
	{
		objSprite = GetComponent<tk2dSprite>();
		
		objColor = Color.white;
		objColor.a = 0.0000f;
		
		isFadingIn = true;
	}
	
	void Update() 
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, -2.0f);
		
		float fadeSpeed = Time.time * 4;
		fade = Mathf.Min(fadeSpeed + fade, 100);
		
		float time = Time.time;
		float pulsate = Mathf.Sin(time * 6) + 1;
		float scale = 1 + pulsate * 0.01f * fade;
		
		objSprite.scale = new Vector3(scale, scale, scale);

		transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
		
		// Fade in
		if (isFadingIn)
		{
			objColor.a += 0.01f;	
			
			if (objColor.a >= 0.750f)
			{
				isFadedIn = true;
				isFadingIn = false;
			}
		}
		
		if (isFadedIn)
		{
			// Fade out
			if ((Time.time >= timeToLive) && isFadingOut == false)
			{
				isFadingOut = true;
			}
			
			if (isFadingOut)
			{
				objColor.a -= 0.01f;
				
				if (objColor.a <= 0.000f)
				{
					isFadedOut = true;	
					isFadingOut = false;
				}
			}
			else
			{
				
			}
		}
		
		if (isFadedOut)
		{
			ExpireMe();
		}
		
		objSprite.color = objColor;
	}
	
	void ExpireMe()
	{
		Destroy (gameObject);
	}
}
