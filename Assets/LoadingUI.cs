using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.loadingScreen = this.gameObject;
        GameManager.Instance.slider = this.slider;
        this.gameObject.SetActive(false);
    }

}
