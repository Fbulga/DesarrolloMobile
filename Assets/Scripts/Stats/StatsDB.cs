using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Enum;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsDB", menuName = "Data/StatsDB", order = 1)]
public class StatsDB : ScriptableObject
{
    [SerializedDictionary("Stat Name", "Value")]
    public SerializedDictionary<Stat, float> statsDictionary;
    
    
    public bool TryGetStat(Stat type, out float statValue)
    {
        return statsDictionary.TryGetValue(type, out statValue);
    }
    
    public float IncreaseStat(Stat type, float increment)
    {
        if (TryGetStat(type, out float statValue))
        {
            statsDictionary[type] = statValue + increment;
            return statsDictionary[type];
        }
        return statsDictionary[type] = statValue;
    }

    public void ResetStat(Stat type)
    {
        if (TryGetStat(type, out float statValue))
        {
            statsDictionary[type] = 0f;
        }
    }

    public void SetStatValue(Stat type, float statValue)
    {
        statsDictionary[type] = statValue;
    }
}
