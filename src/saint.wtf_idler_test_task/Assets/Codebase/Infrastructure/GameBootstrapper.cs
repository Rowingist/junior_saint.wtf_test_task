using System;
using Codebase.Services.Input;
using Codebase.UI.Animations;
using UnityEngine;

namespace Codebase.Infrastructure
{
  
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    [SerializeField] private LoadingCurtain _curtain;
    
    private GameStateMachine _stateMachine;
    public static IInputService InputService;  

    private void Awake()
    {
      _stateMachine = new GameStateMachine();
      //_stateMachine.Enter<BootstapState>();

      InputService = new MobileInputService();
      
      DontDestroyOnLoad(this);
    }
  }
}
