using Codebase.CameraLogic;
using Codebase.Infrastructure.Factory;
using Codebase.Services.Input;
using Codebase.Services.PersistentProgress;
using Codebase.Services.StaticData;
using Codebase.StaticData.Level;
using Codebase.UI.Animations;
using Codebase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codebase.Infrastructure.States
{
  public class LoadLevelState : IPayLoadedState<string>
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IPersistentProgressService _progressService;
    private readonly IGameFactory _gameFactory;
    private readonly IStaticDataService _staticData;
    private readonly IInputService _inputService;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
      IPersistentProgressService progressService, IGameFactory gameFactory, IStaticDataService staticData,
      IInputService inputService, IUIFactory uiFactory)
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _progressService = progressService;
      _gameFactory = gameFactory;
      _staticData = staticData;
      _inputService = inputService;
      _uiFactory = uiFactory;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _gameFactory.Cleanup();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit()
    {
      _loadingCurtain.Hide();
    }

    private void OnLoaded()
    {
      InitUIRoot();
      InitGameWorld();

      InformProgressReaders();

      _gameStateMachine.Enter<GameLoopState>();
    }

    private void InitUIRoot() => 
      _uiFactory.CreateUIRoot();

    private void InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();
      
      GameObject hud = InitHud();
      InitJoystick(hud.transform);

      InitSpawners(levelData);
      GameObject player = InitPlayer(levelData);
      CameraFollow(player);
    }
    
    private void InitSpawners(LevelStaticData levelStaticData)
    {
      foreach (ManufactureSpawnerStaticData spawnerData in levelStaticData.ManufactureSpawners)
          _gameFactory.CreateManufactureSpawner( spawnerData.Position, spawnerData.ResourceType);
    }
    
    private LevelStaticData LevelStaticData() =>
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private GameObject InitHud() => 
      _gameFactory.CreateHud();

    private void InitJoystick(Transform under) => 
      _gameFactory.CreateJoystick(under);

    private GameObject InitPlayer(LevelStaticData levelData) => 
      _gameFactory.CreateHero(levelData.InitialHeroPosition);

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void CameraFollow(GameObject Hero)
    {
      CameraFollow cameraFollow = Camera.main.GetComponentInParent<CameraFollow>();
      cameraFollow.Follow(Hero);
    }
    
  }
}