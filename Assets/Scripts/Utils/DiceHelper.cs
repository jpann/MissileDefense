using UnityEngine;
using System.Collections;

public enum DieType
{
    D4 = 4,
    D6 = 6,
    D8 = 8,
    D10 = 10,
    D12 = 12,
    D20 = 20,
    D100 = 100
}

public static class DiceHelper
{
    public static int RollDie(DieType die)
    {
		
        return Random.Range(0, (int)die) + 1;
    }

    public static int RollDie(int min, int max)
    {
        return Random.Range(min, max);
    }

    public static int RollDie(DieType die, int numberOfDie)
    {
        int value = 0;

        for (int i = 0; i < numberOfDie; i++)
        {
            value += RollDie(die);
        }

        return value;
    }
}