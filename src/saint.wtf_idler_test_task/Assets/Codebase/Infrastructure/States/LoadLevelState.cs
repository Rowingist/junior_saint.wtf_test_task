using System.Collections.Generic;
using Codebase.CameraLogic;
using Codebase.Events;
using Codebase.Infrastructure.Factory;
using Codebase.NewResourceManufacture;
using Codebase.NewResourceManufacture.Spawners;
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
    private readonly ManufactureStopWorkEventProvider _stopWorkEventProvider;

    private GameObject _warningsContainer;
    
    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
      IPersistentProgressService progressService, IGameFactory gameFactory, IStaticDataService staticData,
      IInputService inputService, IUIFactory uiFactory, ManufactureStopWorkEventProvider stopWorkEventProvider)
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _progressService = progressService;
      _gameFactory = gameFactory;
      _staticData = staticData;
      _inputService = inputService;
      _uiFactory = uiFactory;
      _stopWorkEventProvider = stopWorkEventProvider;
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

    private void InitUIRoot()
    {
      GameObject uiRoot = _uiFactory.CreateUIRoot();
      _warningsContainer = InitWarningsContainer(uiRoot);
    }

    private GameObject InitWarningsContainer(GameObject under) =>
      _uiFactory.CreateWarningContainer(under);

    private void InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();

      List<Manufacture> manufactures = InitSpawners(levelData);
      InitEventProviders(manufactures);

      GameObject player = InitPlayer(levelData);
      CameraFollow(player);

      GameObject hud = InitHud(_warningsContainer);
      InitJoystick(hud.transform);
    }

    private List<Manufacture> InitSpawners(LevelStaticData levelStaticData)
    {
      List<Manufacture> manufactures = new List<Manufacture>();
      foreach (ManufactureSpawnerStaticData spawnerData in levelStaticData.ManufactureSpawners)
      {
        GameObject manufactureSpawner =
          _gameFactory.CreateManufactureSpawner(spawnerData.Position, spawnerData.ResourceType);
        Manufacture manufacture = manufactureSpawner.GetComponent<ManufactureSpawnPoint>().Spawn();
        manufactures.Add(manufacture);
      }

      return manufactures;
    }

    private void InitEventProviders(List<Manufacture> manufactures) =>
      _stopWorkEventProvider.Construct(manufactures);

    private LevelStaticData LevelStaticData() =>
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private GameObject InitHud(GameObject warningsContainer)
    {
      GameObject hudObject = _gameFactory.CreateHud();
      hudObject.GetComponentInChildren<WarningsActorUI>()
        .Construct(_uiFactory, _stopWorkEventProvider, _staticData, warningsContainer);

      return hudObject;
    }

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