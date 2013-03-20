using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
	void Start () 
	{
		Screen.showCursor = true;
	}
	
	void Update () 
	{
	
	}
	
	void Play()
	{
		Application.LoadLevel("TestScene_0");
	}
	
	void TestBed()
	{
		Application.LoadLevel("ProjectileTestScene");	
	}
	
	void MainMenu()
	{
		Application.LoadLevel("MainMenu");	
	}
}
