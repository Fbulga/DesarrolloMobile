using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPoolManager : MonoBehaviour
{
    public static PowerUpPoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetPowerUp(GameObject prefab, Vector3 position)
    {
        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        GameObject obj;

        if (pool[prefab].Count > 0)
        {
            obj = pool[prefab].Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;

        return obj;
    }

    public void ReturnPowerUp(GameObject obj, GameObject prefabKey)
    {
        obj.SetActive(false);
        pool[prefabKey].Enqueue(obj);
    }
}
