using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enum;
using System.Collections.Generic;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private StatsDB statsDB;
    [SerializeField] private GameObject statItemPrefab;  // Prefab con los textos
    public Transform contentParent;    // El objeto con Vertical Layout Group


    void Start()
    {
        ShowStats(statsDB.statsDictionary);
    }

    public void ShowStats(Dictionary<Stat, float> stats)
    {
        // Limpiar si hay elementos previos
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Crear una fila por cada stat
        foreach (var stat in stats)
        {
            GameObject item = Instantiate(statItemPrefab, contentParent);
            TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();

            if (texts.Length >= 2)
            {
                texts[0].text = stat.Key.ToString();       // Nombre de la stat
                texts[1].text = stat.Value.ToString(); // Valor de la stat
            }
        }
    }
}
