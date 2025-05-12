using System;
using System.Collections.Generic;

[Serializable]
public class StatEntry {
    public string statName;
    public float value;
}

[Serializable]
public class StatExport {
    public List<StatEntry> entries = new List<StatEntry>();
}

[Serializable]
public class StatExportWrapper
{
    public string jsonData;     // El JSON serializado de StatExport
    public string checksum;     // El hash del jsonData
    public string timestamp;
}


