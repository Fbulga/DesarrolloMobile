using UnityEngine;

public static  class VibrationManager
{
    private const int AMPLITUDE_LIGHT = 90;
    private const int AMPLITUDE_MEDIUM = 150;
    private const int AMPLITUDE_HEAVY = 255;

    private static void Vibrate(long milliseconds, int amplitude = AMPLITUDE_HEAVY)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            if (vibrator == null) return;

            if (AndroidVersion() >= 26)
            {
                AndroidJavaClass vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
                AndroidJavaObject effect = vibrationEffect.CallStatic<AndroidJavaObject>(
                    "createOneShot", milliseconds, amplitude);
                vibrator.Call("vibrate", effect);
            }
            else
            {
                vibrator.Call("vibrate", milliseconds);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Vibration error: " + e.Message);
        }
#endif
    }
    
    public static void Vibrate(VibrationPreset preset)
    {
        Debug.Log("[DEVICE] VIBRATING!!!");
#if UNITY_ANDROID && !UNITY_EDITOR
        switch (preset)
        {
            case VibrationPreset.Light:
                Vibrate(20, AMPLITUDE_LIGHT);
                break;
            case VibrationPreset.UI:
                Vibrate(40, 100);
                break;
            case VibrationPreset.Medium:
                Vibrate(60, AMPLITUDE_MEDIUM);
                break;
            case VibrationPreset.Heavy:
                Vibrate(80, AMPLITUDE_HEAVY);
                break;
        }
#endif
    }
    private static int AndroidVersion()
    {
        using AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION");
        return version.GetStatic<int>("SDK_INT");
    }

    public static void VibrateLight()
    {
#if UNITY_ANDROID || UNITY_IOS
        Vibrate(50, AMPLITUDE_LIGHT);
#endif
    }
    
    public static void VibrateMedium()
    {
        Vibrate(50, AMPLITUDE_MEDIUM);
    }

    public static void VibrateHeavy()
    {
        Vibrate(70, AMPLITUDE_HEAVY);
    }
    
    public static void VibrateUI()
    {
        Vibrate(40, 100);
    }
}
