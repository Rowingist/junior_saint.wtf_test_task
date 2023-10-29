using Codebase.Services;
using Codebase.Services.Input;

namespace Codebase.Infrastructure.States
{
  public class BootstrapState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _services = services;

      RegisterServices();
    }

    public void Enter()
    {
      _sceneLoader.Load(Constants.InitialSceneName, onLoaded: EnterLoadLevel);
    }

    public void Exit()
    {
    }

    private void EnterLoadLevel()
    {
      _stateMachine.Enter<LoadProgressState>();
    }

    private void RegisterServices()
    {
      RegisterAssetProvider();
      _services.RegisterSingle(InputService());
    }

    private void RegisterAssetProvider()
    {
      
    }

    private static IInputService InputService() => 
      new MobileInputService();
  }
}