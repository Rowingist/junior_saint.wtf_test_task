using Codebase.Data;
using Codebase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.UI.Windows
{
  public abstract class WindowBase : MonoBehaviour
  {
    protected IPersistentProgressService ProgressService;
    protected PlayerProgress Progress => ProgressService.Progress;

    public void Construct(IPersistentProgressService progressService)
    {
      ProgressService = progressService;
    }

    // private void Awake() => 
    //   OnAwake();

    private void Start()
    {
      Initialize();
      SubscribeUpdates();
    }

    public virtual void OnDestroy() => 
      CleanUp();

    // protected virtual void OnAwake()
    // {
    //   if(_closeButton)
    //     _closeButton.onClick.AddListener(() => Destroy(gameObject));
    // }

    protected virtual void Initialize() {}
    protected virtual void SubscribeUpdates() {}
    protected virtual void CleanUp() {}
  }
}