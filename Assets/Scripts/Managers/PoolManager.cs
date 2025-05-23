using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    public Action OnClearPool;
    
    private Dictionary<PrefabsType, Queue<GameObject>> unActivePool = new Dictionary<PrefabsType, Queue<GameObject>>();
    private Dictionary<PrefabsType, List<GameObject>> activePool = new Dictionary<PrefabsType, List<GameObject>>();

    public Dictionary<PrefabsType, List<GameObject>> ActivePool => activePool;
    
    
    [SerializeField] private PrefabsDB prefabsDB;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        OnClearPool += HandleClearPool;
    }
    
    private GameObject GetObject(PrefabsType prefabType, Vector3 position)
    {
        if (!unActivePool.ContainsKey(prefabType))
            unActivePool[prefabType] = new Queue<GameObject>();

        if (!activePool.ContainsKey(prefabType))
            activePool[prefabType] = new List<GameObject>();

        GameObject obj;

        if (unActivePool[prefabType].Count > 0)
        {
            obj = unActivePool[prefabType].Dequeue();
            Debug.Log("Se reutilizo");
        }
        else
        {
            obj = Instantiate(prefabsDB.PrefabDictionary[prefabType], position, Quaternion.identity);
            Debug.Log("Se instancio uno nuevo");
        }

        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        activePool[prefabType].Add(obj);

        return obj;
    }

    private void ReturnObject(GameObject obj, PrefabsType prefabType)
    {
        if (!unActivePool.ContainsKey(prefabType))
            unActivePool[prefabType] = new Queue<GameObject>();

        if (activePool.ContainsKey(prefabType))
            activePool[prefabType].Remove(obj);

        obj.SetActive(false);
        unActivePool[prefabType].Enqueue(obj);
        Debug.Log("Volvio");
    }

    private void HandleClearPool()
    {
        unActivePool.Clear();
        activePool.Clear();
    }

    public GameObject GetPowerUp(PrefabsType prefabType, Vector3 position) => GetObject(prefabType, position);
    public void ReturnPowerUp(GameObject obj, PrefabsType prefabType) => ReturnObject(obj, prefabType);
    
    public GameObject GetBall(PrefabsType prefabType, Vector3 position) => GetObject(prefabType, position);
    
    public void ReturnBall(GameObject obj, PrefabsType prefabType) => ReturnObject(obj, prefabType);
    
}
