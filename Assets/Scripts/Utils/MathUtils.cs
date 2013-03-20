using UnityEngine;
using System.Collections;

public static class MathUtils
{
	public static float GetPercentOf(float percent, float value)
	{
		float number = value * percent / 100;
	
		return number;	
	}
}
