using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class HighScore
{
	public string Name { get; set; }
	public int Score { get; set; }
	public float TimeAlive { get; set; }
}

public class HighScoreController : MonoBehaviour 
{
	private string secretKey = "buttsbutts"; // Edit this value and make sure it's the same as the one stored on the server
    public string addScoreURL = "http://ikonzeh.org/highscore/addscore.php?"; //be sure to add a ? to your url
    public string highscoreURL = "http://ikonzeh.org/highscore/displayx.php";
	
	public List<HighScore> scores = new List<HighScore>();
	
    void Start()
    {
        StartCoroutine(GetScores());
    }
 
    // remember to use StartCoroutine when calling this function!
    public IEnumerator PostScores(string name, int score, float timeAlive)
    {	
		//Debug.Log("HighScoreController:PostScores() - name = " + name + "; score = " + score + "; timeAlive = " + timeAlive);
		
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Md5Sum(name + score + secretKey);
 
        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&time_alive=" + timeAlive + "&hash=" + hash;
 
        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done
 
        if (hs_post.error != null)
        {
            Debug.Log("HighScoreController:PostScores() - There was an error posting the high score: " + hs_post.error);
        }
    }

    // remember to use StartCoroutine when calling this function!
	public IEnumerator GetScores()
	{
		//Debug.Log("HighScoreController:GetScores() - Loading scores from " + highscoreURL);
		
		WWW hs_get = new WWW(highscoreURL);
		yield return hs_get;
		
		if (hs_get.error == null)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(hs_get.text);
			
			ProcessHighScores(doc.SelectNodes("highscores/score"));
		}
		else
		{
			Debug.Log("HighScoreController:GetScores() - There was an error getting the high score: " + hs_get.error);
		}
	}
	
	private void ProcessHighScores(XmlNodeList nodes)
	{
		scores = new List<HighScore>();
		
		HighScore hs;
		
		foreach (XmlNode node in nodes)
		{
			string name = node.Attributes["name"].Value;
			int score = System.Convert.ToInt32(node.Attributes["score"].Value);
			float timeAlive = System.Convert.ToSingle(node.Attributes["time_alive"].Value);
			
			hs = new HighScore();
			hs.Name = name;
			hs.Score = score;
			hs.TimeAlive = timeAlive;
			
			//Debug.Log("HighScoreController:ProcessHighScores() - name = " + hs.Name + "; score = " + hs.Score);
			
			scores.Add(hs);	
		}
	}
	
	public string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
	 
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
	 
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
	 
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
	 
		return hashString.PadLeft(32, '0');
	}
}
