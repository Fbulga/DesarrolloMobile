using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Enum;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    [SerializeField] private StatsDB statsDB;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadStatsFromWrapper(StatExportWrapper wrapper)
    {
        if (!StatExportService.ValidateChecksum(wrapper))
        {
            Debug.LogError("Invalid checksum. Stats not loaded.");
            return;
        }

        StatExport importData = JsonUtility.FromJson<StatExport>(wrapper.jsonData);

        foreach (StatEntry entry in importData.entries)
        {
            if (System.Enum.TryParse(entry.statName, out Stat parsedStat))
            {
                PlayerPrefs.SetFloat(entry.statName, entry.value);
            }
        }

        PlayerPrefs.Save();

        foreach (var statKey in statsDB.statsDictionary.Keys.ToList())
        {
            if (PlayerPrefs.HasKey(statKey.ToString()))
                statsDB.SetStatValue(statKey, PlayerPrefs.GetFloat(statKey.ToString()));
            else
                statsDB.SetStatValue(statKey, 0);
        }
    }

    public StatExportWrapper ExportStats()
    {
        var exportData = new StatExport();

        foreach (var kvp in statsDB.statsDictionary)
        {
            exportData.entries.Add(new StatEntry { statName = kvp.Key.ToString(), value = kvp.Value });
        }

        return StatExportService.ExportWithChecksum(exportData);
    }

    public void IncreaseStat(Stat type, float increment)
    {
        if (PlayerPrefs.HasKey(type.ToString()))
        {
            PlayerPrefs.SetFloat(type.ToString(), statsDB.IncreaseStat(type, increment));
        }
        else
        {
            AddStat(type, increment);
        }

        ExportStats();
    }

    public void AddStat(Stat type, float value = 0f)
    {
        if (!PlayerPrefs.HasKey(type.ToString()))
        {
            PlayerPrefs.SetFloat(type.ToString(), value);
            statsDB.ResetStat(type);
            statsDB.IncreaseStat(type, value);
        }

        ExportStats();
    }
    
    
    
    
    

}
