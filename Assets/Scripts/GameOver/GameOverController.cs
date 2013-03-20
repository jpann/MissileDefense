using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverController : MonoBehaviour
{
	public GameObject objScoreBoardText;
	public tk2dTextMesh objFlashText;
	public tk2dTextMesh objInputText;
	
	private GameInformation objGameInfo;

	private string mName = "anonymous";
	private bool hasPosted = false;

	void Start() 
	{
		Screen.showCursor = true;
		
		GameObject oTemp = GameObject.FindGameObjectWithTag("GameInformation");

		objGameInfo = new GameInformation();
		
		if (oTemp != null)
		{
			objGameInfo = oTemp.GetComponent<GameInformation>();
		}
		else
		{
			Debug.Log("GameOverController:Start1() - oTemp == null");	
			
			objGameInfo = new GameInformation();
		}
		
		StartCoroutine(LoadScoreBoard());
		
		objFlashText.text = string.Format("You got {0} points in {1:0.000} seconds!", 
			GameInfoManager.Instance.Score,
			GameInfoManager.Instance.TimeAlive);
		objFlashText.Commit();
	}

	void Update()
	{
		mName = objInputText.text;
	}
	
	void OnGUI()
	{
//		GUI.BeginGroup(new Rect(
//			(Screen.width / 2), 
//			(Screen.height / 2) - 44,
//			800,
//			300));
//
//		mName = GUI.TextField(new Rect(0, 0, 100, 20), mName, 25);
//		
//		GUI.EndGroup();
	}
	
	void PostPlayerScores()
	{
		if (!hasPosted)
		{
			if (!string.IsNullOrEmpty(mName))
			{
				objGameInfo.PostPlayerScore(mName, GameInfoManager.Instance.Score, GameInfoManager.Instance.TimeAlive);
			
			hasPosted = true;
			}
		}
	}
	
	IEnumerator LoadScoreBoard()
	{
		yield return StartCoroutine(objGameInfo.highScoreController.GetScores());
			
		List<HighScore> scores = objGameInfo.highScoreController.scores;
		Debug.Log("GameOverController:LoadScoreBoard() - scores.Count = " + scores.Count);
		
		Vector3 pos = new Vector3(417.5394f, 381.1733f, -2.0f);
		
		foreach (HighScore score in scores)
		{
			GameObject objScore = (GameObject)Instantiate(objScoreBoardText,
				pos,
				Quaternion.identity);
			
			tk2dTextMesh txt = objScore.GetComponent<tk2dTextMesh>();
			
			if (txt != null)
			{
				txt.text = string.Format("{0} - {1} - {2}\n\r", score.Score, score.Name, score.TimeAlive);
				Debug.Log(txt.text);
				txt.Commit();
			}
			
			pos = new Vector3(pos.x, pos.y - 20.0f, pos.z);
		}
	}
	
	void CreateGameInformationObject()
	{
		Debug.Log("GameOverController:CreateGameInformationObject()");
		
		objGameInfo = new GameInformation();
	}
}
