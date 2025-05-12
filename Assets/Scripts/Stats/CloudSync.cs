using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;

public class CloudSync : MonoBehaviour
{
    public static CloudSync Instance;

    public Action OnUploadJsonRequested;
    public event Action OnSignInCompleted;

    private const string statsKey = "stats_json";

    private async void Awake()
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

        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Signed in to CloudSync");
                OnSignInCompleted?.Invoke();
            }

            await DownloadJsonFromCloud();
        }
        catch (Exception e)
        {
            Debug.LogError($" Erron in LogIn : {e.Message}");
        }
    }

    private void Start()
    {
        OnUploadJsonRequested += SyncNow;
    }

    private async Task DownloadJsonFromCloud()
    {
        try
        {
            var keys = new HashSet<string> { statsKey };
            var result = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            if (result.TryGetValue(statsKey, out var item))
            {
                string cloudJson = item.Value.GetAsString();
                var cloudWrapper = JsonUtility.FromJson<StatExportWrapper>(cloudJson);

                StatExportWrapper localWrapper;
                if (!StatExportService.ValidateChecksum(cloudWrapper))
                {
                    Debug.LogWarning("Cloud data corrupt. Use local data instead.");
                    localWrapper = StatManager.Instance.ExportStats();
                    StatManager.Instance.LoadStatsFromWrapper(localWrapper);
                    await UploadJsonToCloud(); // Reemplazamos en la nube
                    return;
                }

                localWrapper = StatManager.Instance.ExportStats();
                bool localValid = StatExportService.ValidateChecksum(localWrapper);
                
                if (!localValid)
                {
                    Debug.Log("Local data corrupt. Use cloud data instead.");
                    StatManager.Instance.LoadStatsFromWrapper(cloudWrapper);
                    return;
                }
                
                if (localValid && cloudWrapper.checksum == localWrapper.checksum)
                {
                    Debug.Log("Local data and cloud data are synchronized.");
                    StatManager.Instance.LoadStatsFromWrapper(localWrapper); // O cloudWrapper, da igual
                    return;
                }
                
                // Comparar timestamps
                DateTime cloudTime = DateTime.Parse(cloudWrapper.timestamp);
                DateTime localTime = DateTime.Parse(localWrapper.timestamp);

                if (cloudTime < localTime)
                {
                    Debug.Log("Using most recent local data");
                    StatManager.Instance.LoadStatsFromWrapper(localWrapper);
                    await UploadJsonToCloud();
                }
                else
                {
                    Debug.Log("Using most recent cloud data");
                    StatManager.Instance.LoadStatsFromWrapper(cloudWrapper);
                }
            }
            else
            {
                Debug.Log("No cloud data found. Using local data instead.");
                var localWrapper = StatManager.Instance.ExportStats();
                StatManager.Instance.LoadStatsFromWrapper(localWrapper);
                await UploadJsonToCloud();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error syncing data : {e.Message}");
            Debug.Log("Using local data fallback.");
            var localWrapper = StatManager.Instance.ExportStats();
            StatManager.Instance.LoadStatsFromWrapper(localWrapper);
        }
    }

    private async Task UploadJsonToCloud()
    {
        var wrapper = StatManager.Instance.ExportStats();
        string wrapperJson = JsonUtility.ToJson(wrapper);

        var data = new Dictionary<string, object> { { statsKey, wrapperJson } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);

        Debug.Log("Json saved to cloud.");
    }

    private async void SyncNow()
    {
        await UploadJsonToCloud();
    }
}
