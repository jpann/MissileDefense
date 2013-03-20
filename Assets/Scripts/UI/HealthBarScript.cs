using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour 
{
	public GUIStyle progress_empty;
	public GUIStyle progress_full;
	
	public float barDisplay;
	
	Vector2 pos = new Vector2(10, 50);
	Vector2 size = new Vector2(28, 5);
	
	public Texture2D emptyTexture;
	public Texture2D fullTexture;
	
	void Start()
	{
		size.x = emptyTexture.width;
		size.y = emptyTexture.height;
	}
	
	void OnGUI()
	{
		//draw the background:
	   	GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y), emptyTexture, progress_empty);
	    GUI.Box(new Rect(pos.x, pos.y, size.x, size.y), fullTexture, progress_full);
	 
	    //draw the filled-in part:
	    GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
	    GUI.Box(new Rect(0, 0, size.x, size.y), fullTexture, progress_full);
	 
	    GUI.EndGroup();
	    GUI.EndGroup();
	}
	
	void Update() 
	{
		pos = transform.position;
		
		//the player's health
		barDisplay = 0.50f;
    	//barDisplay = PlayerMoveScript.playerHealth / PlayerMoveScript.playerHealthTotal;
	}
}
