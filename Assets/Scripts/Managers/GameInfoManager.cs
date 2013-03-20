using UnityEngine;
using System.Collections;

//
// Usage:
// GameInfoManager.Instance.Score += 1;
// GameInfoManager.Instance.Reset();
//
public class GameInfoManager
{
	private static GameInfoManager instance;
	
	public GameInfoManager()
	{
		if (instance != null)
		{
			Debug.LogError("Cannot have two instances of GameManager.");
			return;
		}
		
		instance = this;
	}
	
	public static GameInfoManager Instance
	{
		get
		{
			if (instance == null)
			{
				new GameInfoManager();	
			}
			
			return instance;
		}
	}
	
	// Variables
	private int currency = 0;
	private int score = 0;
	private int missilesFired = 0;
	private int enemyMissilesHit = 0;
	private float damageDealt = 0.0f;
	private float damageReceived = 0.0f;
	private int turretsDestroyed = 0;
	private float timeAlive = 0.0f;
	private int enemyMissilesSpawned = 0;
	private int missilesSpawned = 0;
	private float amountHealed = 0.0f;
	
	#region Properties
	public int Currency
	{
		set { currency = value; }
		get { return currency; }
	}
	
	public int Score
	{
		set { score = value; }
		get { return score; }
	}
	
	public int MissilesFired
	{
		set { missilesFired = value; }
		get { return missilesFired; }
	}
	
	public int EnemyMissilesHit
	{
		set { enemyMissilesHit = value; }
		get { return enemyMissilesHit; }
	}
	
	public float DamageDealt 
	{
		set { damageDealt = value; }
		get { return damageDealt; }
	}
	
	public float DamageReceived
	{
		set { damageReceived = value; }
		get { return damageReceived; }
	}
	
	public int TurretsDestroyed
	{
		set { turretsDestroyed = value; }
		get { return turretsDestroyed; }
	}
	
	public float TimeAlive
	{
		set { timeAlive = value; }
		get { return timeAlive; }
	}
	#endregion
	
	public void Reset()
	{
		currency = 0;
		score = 0;
		missilesFired = 0;
		enemyMissilesHit = 0;
		damageDealt = 0.0f;
		damageReceived = 0.0f;
		turretsDestroyed = 0;
		timeAlive = 0.0f;
	}
	
	public void IncrementCurrency()
	{
		currency += 1;	
	}
	
	public void IncrementScore()
	{
		score += (int)(1 + (1 * timeAlive));
	}
	
	public void IncrementMissilesFired()
	{
		missilesFired++;	
	}
	
	public void IncrementEnemyMissilesHit()
	{
		enemyMissilesHit++;	
	}
	
	public void IncrementEnemyMissilesHit(int amount)
	{
		enemyMissilesHit += amount;	
	}
	
	public void IncrementDamageDealt(float amount)
	{
		damageDealt += amount;
	}
	
	public void IncrementDamageReceived(float amount)
	{
		damageReceived += amount;
	}
	
	public void IncrementTurretsDestroyed()
	{
		turretsDestroyed++;
	}
}
