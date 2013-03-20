using UnityEngine;
using System.Collections;

public static class RandomHelper
{
    public static Random Rand;

    public static void Initialize()
    {
        Rand = new Random();
    }
}
