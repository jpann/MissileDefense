using UnityEngine;
using System.Collections;

public class GameInformation : MonoBehaviour 
{
	public HighScoreController highScoreController;
	
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void Start() 
	{
		highScoreController = gameObject.GetComponent<HighScoreController>();
	}
	
	void Update() 
	{
	
	}
	
	void OnLevelWasLoaded(int level)
	{
		if (level == 1)
		{
			Reset();	
		}
	}
	
	void Reset()
	{
		Debug.Log("GameInformation:Reset()");	

		GameInfoManager.Instance.Reset();
	}
	
	public void PostPlayerScore(
		string name, 
		int score,
		float alive)
	{
		//Debug.Log("GameInformation:PostPlayerScore() - name = " + name + "; score = " + score);
		
		if (highScoreController != null)
		{
			object[] parms = new object[2] { name, score };
			
			//StartCoroutine("PostScores", parms);
			StartCoroutine(highScoreController.PostScores(name, score, alive));
		}
		else
		{
			Debug.Log("GameInformation:PostScore() - highScoreController == null");	
		}
	}
}
