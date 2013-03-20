using UnityEngine;
using System.Collections;

public class LevelInformation 
{
	private static LevelInformation instance;
	
	public LevelInformation()
	{
		if (instance != null)
		{
			Debug.LogError("Cannot have two instances of LevelInformation.");
			return;
		}
		
		instance = this;
	}
	
	public static LevelInformation Instance
	{
		get
		{
			if (instance == null)
			{
				new LevelInformation();	
			}
			
			return instance;
		}
	}
	
	// Variables
	private int maxNumberOfWaves;
	
	#region Public Properties
	#endregion
	
	public void Reset()
	{
		
	}
}
