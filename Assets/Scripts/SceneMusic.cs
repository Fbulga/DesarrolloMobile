using Enum;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
  [SerializeField] private SFXType sfxType;
  private void Start()
  {
    SetSceneMusic(sfxType);
  }

  private void SetSceneMusic(SFXType sfx)
  {
    SFXManager.Instance.PlayLoopableClip(sfx);
  }
}
