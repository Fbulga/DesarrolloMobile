using AYellowpaper.SerializedCollections;
using Enum;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDB", menuName = "SoundDB", order = 1)]
public class SoundDB : ScriptableObject
{
   [SerializedDictionary("SFX Type", "Audio Clip")]
   public SerializedDictionary<SFXType, AudioClip> SoundDictionary;

   public bool TryGetAudioClip(SFXType type, out AudioClip audioClip)
   {
      return SoundDictionary.TryGetValue(type, out audioClip);
   }

}
