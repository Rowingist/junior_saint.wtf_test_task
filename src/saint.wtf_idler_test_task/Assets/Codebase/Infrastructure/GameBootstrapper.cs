using Codebase.Infrastructure.States;
using Codebase.Services;
using Codebase.UI.Animations;
using UnityEngine;

namespace Codebase.Infrastructure
{
  
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    [SerializeField] private LoadingCurtain _curtainTemplate;
    
    private GameStateMachine _stateMachine;

    private void Awake()
    {
      _stateMachine = new GameStateMachine(new SceneLoader(this), Instantiate(_curtainTemplate),AllServices.Container);
      _stateMachine.Enter<BootstrapState>();
      
      DontDestroyOnLoad(this);
    }
  }
}
