using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour 
{
	public GameObject player;
	public tk2dTextMesh scoreText;
	public tk2dTextMesh timeText;
	private GameInformation objGameInfo;
	
	void Start () 
	{
		objGameInfo = GameObject.FindGameObjectWithTag("GameInformation").GetComponent<GameInformation>();
	}
	
	void Update () 
	{
		UpdateScore();
		UpdateTime();
	}
	
	void UpdateScore()
	{
		//Debug.Log("HUDController:UpdateScore() score = " + objGameInfo.Score);
		
		scoreText.text = string.Format("Score: {0}", GameInfoManager.Instance.Score);
		scoreText.Commit();
	}
	
	void UpdateTime()
	{
		float timeAlive = Time.timeSinceLevelLoad;
		
		timeText.text = string.Format("Time: {0:00.00}", timeAlive);
		timeText.Commit();
		
		GameInfoManager.Instance.TimeAlive = timeAlive;
	}
}
