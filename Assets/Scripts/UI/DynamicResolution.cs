using UnityEngine;
using UnityEngine.Rendering;

namespace UI
{
    public class DynamicResolution : MonoBehaviour
    {
        private float targetFrameRate = 60.0f;
        private float maxScale = 1.0f;
        private float minScale = 0.5f;

        void Start()
        {
           // DynamicResolutionHandler.SetDynamicResScaler(CalculateScaleFactor());
        }

        private float CalculateScaleFactor()
        {
            float currentFrameRate = 1.0f/ Time.deltaTime;
            float performanceRatio = currentFrameRate / targetFrameRate;
            float scale = Mathf.Lerp(minScale, maxScale, performanceRatio);
            float scaleFactor = Mathf.Clamp(scale, minScale, maxScale);
            return scaleFactor;
        }
    }
}