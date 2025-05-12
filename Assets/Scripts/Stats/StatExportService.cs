using System;
using UnityEngine;

public static class StatExportService
{
    public static StatExportWrapper ExportWithChecksum(StatExport data)
    {
        string json = JsonUtility.ToJson(data);
        string checksum = ComputeChecksum(json);
        string timestamp = DateTime.UtcNow.ToString("o"); // ISO 8601

        return new StatExportWrapper
        {
            jsonData = json,
            checksum = checksum,
            timestamp = timestamp
        };
    }

    public static bool ValidateChecksum(StatExportWrapper wrapper)
    {
        return wrapper.checksum == ComputeChecksum(wrapper.jsonData);
    }

    private static string ComputeChecksum(string input)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}