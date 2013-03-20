using UnityEngine;
using System.Collections;

public enum TurretState
{
	FULL = 100,
	SLIGHTLY_DAMAGED = 75,
	DAMAGED = 50,
	MORE_DAMAGED = 25,
	HEAVILY_DAMAGED = 1,
	DESTROYED = 0
};

public static class TurretStateManager
{
	public static Color GetStateColor(TurretState state)
	{
		Color color = Color.white;
		
		switch (state)
		{
			case TurretState.FULL:
				color = Color.white;
				break;
			case TurretState.SLIGHTLY_DAMAGED:
				color = new Color(253, 255, 184);
				break;
			case TurretState.DAMAGED:
				color = Color.yellow;
				break;
			case TurretState.MORE_DAMAGED:
				color = new Color(255, 136, 0);
				break;
			case TurretState.HEAVILY_DAMAGED:
				color = Color.red;
				break;
			case TurretState.DESTROYED:
				color = Color.grey;
				break;
		}
		
		return color;
	}
	
	public static TurretState GetState(float health)
	{
		TurretState state = TurretState.FULL;
		
		if (health >= (int)TurretState.FULL)
		{
			state = TurretState.FULL;	
		}
		else if (health >= (int)TurretState.SLIGHTLY_DAMAGED)
		{
			state = TurretState.SLIGHTLY_DAMAGED;	
		}
		else if (health >= (int)TurretState.DAMAGED)
		{
			state = TurretState.DAMAGED;	
		}
		else if (health >= (int)TurretState.MORE_DAMAGED)
		{
			state = TurretState.MORE_DAMAGED;	
		}
		else if (health >= (int)TurretState.HEAVILY_DAMAGED)
		{
			state = TurretState.HEAVILY_DAMAGED;	
		}
		else 
		{
			state = TurretState.DESTROYED;	
		}
		
		return state;
	}
}