using Codebase.Infrastructure.AssetManagement;
using Codebase.Infrastructure.Factory;
using Codebase.Services;
using Codebase.Services.Input;
using Codebase.Services.PersistentProgress;
using Codebase.Services.SaveLoad;
using Codebase.Services.StaticData;
using Codebase.UI.Services.Factory;

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
      RegisterStaticDataService();
      RegisterAssetProvider();

      _services.RegisterSingle(InputService());
      _services.RegisterSingle<IGameStateMachine>(_stateMachine);
      _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
      _services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAssetProvider>(),
        _services.Single<IStaticDataService>()));
      _services.RegisterSingle<IGameFactory>(new GameFactory());
      _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(),
        _services.Single<IGameFactory>()));
    }

    private void RegisterStaticDataService()
    {
      IStaticDataService staticData = new StaticDataService();
      staticData.Load();
      _services.RegisterSingle(staticData);
    }

    private void RegisterAssetProvider()
    {
      IAssetProvider assetProvider = new AssetProvider();
      _services.RegisterSingle(assetProvider);
    }

    private static IInputService InputService() =>
      new MobileInputService();
  }
}