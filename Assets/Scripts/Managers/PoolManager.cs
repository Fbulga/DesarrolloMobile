using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    public Action OnClearPool;
    
    private Dictionary<GameObject, Queue<GameObject>> unActivePool = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, List<GameObject>> activePool = new Dictionary<GameObject, List<GameObject>>();

    public Dictionary<GameObject, List<GameObject>> ActivePool => activePool;

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
    
    private GameObject GetObject(GameObject prefab, Vector3 position)
    {
        if (!unActivePool.ContainsKey(prefab))
            unActivePool[prefab] = new Queue<GameObject>();

        if (!activePool.ContainsKey(prefab))
            activePool[prefab] = new List<GameObject>();

        GameObject obj;

        if (unActivePool[prefab].Count > 0)
        {
            obj = unActivePool[prefab].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        activePool[prefab].Add(obj);

        return obj;
    }

    private void ReturnObject(GameObject obj, GameObject prefabKey)
    {
        if (!unActivePool.ContainsKey(prefabKey))
            unActivePool[prefabKey] = new Queue<GameObject>();

        if (activePool.ContainsKey(prefabKey))
            activePool[prefabKey].Remove(obj);

        obj.SetActive(false);
        unActivePool[prefabKey].Enqueue(obj);
    }

    private void HandleClearPool()
    {
        unActivePool.Clear();
        activePool.Clear();
    }

    public GameObject GetPowerUp(GameObject prefab, Vector3 position) => GetObject(prefab, position);
    public void ReturnPowerUp(GameObject obj, GameObject prefabKey) => ReturnObject(obj, prefabKey);
    
    public GameObject GetBall(GameObject prefab, Vector3 position)
    {
        ArkanoidManager.Instance.OnNewBall?.Invoke();
        return GetObject(prefab, position);
    }
    public void ReturnBall(GameObject obj, GameObject prefabKey)
    {
        ReturnObject(obj, prefabKey);
        ArkanoidManager.Instance.OnRemoveBall?.Invoke();
    }
}
