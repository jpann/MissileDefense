using UnityEngine;
using System.Collections;

public class CustomCursor : MonoBehaviour 
{
	public Texture cursorImage;
	
	void Start() 
	{
		Screen.showCursor = false;
	}
	
	void Update()
	{
		
	}
	
	void OnGUI()
	{
		Screen.showCursor = false; 
		
		Vector3 mousePos = Input.mousePosition;
		Rect pos = new Rect(mousePos.x - (cursorImage.width / 2), 
							Screen.height - (mousePos.y + (cursorImage.height / 2)),
							cursorImage.width, 
							cursorImage.height);
		
		GUI.Label(pos, cursorImage);	
	}
	
	void OnApplicationFocus(bool focus)
	{ 
		Screen.showCursor = false; 
	}
}
