using System.Collections;
using UnityEngine;

namespace Codebase.UI.Animations
{
  [RequireComponent(typeof(CanvasGroup))]
  class LoadingCurtain : MonoBehaviour
  {
    private const float HidingStep = 0.03f;
    
    public CanvasGroup Curtain;

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    public void Show()
    {
      gameObject.SetActive(true);
      Curtain.alpha = 1;
    }

    public void Hide()
    {
      StartCoroutine(DoFadeIn());
    }

    private IEnumerator DoFadeIn()
    {
      WaitForSeconds fadeInSeconds = new WaitForSeconds(HidingStep);
      
      while(Curtain.alpha > 0f)
      {
        Curtain.alpha -= HidingStep;
        yield return fadeInSeconds;
      }
    }
  }
}