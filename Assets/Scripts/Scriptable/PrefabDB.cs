using AYellowpaper.SerializedCollections;
using Enum;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsDB", menuName = "Data/PrefabsDB", order = 1)]
public class PrefabsDB : ScriptableObject
{
   [SerializedDictionary("Prefab Type", "Prefab")]
   public SerializedDictionary<PrefabsType, GameObject> PrefabDictionary;

   
   public bool TryGetPrefab(PrefabsType type, out GameObject prefab)
   {
      return PrefabDictionary.TryGetValue(type, out prefab);
   }

}
