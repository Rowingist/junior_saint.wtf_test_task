using System.Collections.Generic;
using Codebase.Hero;
using Codebase.Infrastructure.AssetManagement;
using Codebase.Manufacture.Spawners;
using Codebase.MovingResource;
using Codebase.Services.Input;
using Codebase.Services.PersistentProgress;
using Codebase.Services.StaticData;
using UnityEngine;

namespace Codebase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IPersistentProgressService _progressService;
    private readonly IInputService _inputService;
    private readonly IStaticDataService _staticData;

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    public GameFactory(IAssetProvider assets, IPersistentProgressService progressService, IInputService inputService,
      IStaticDataService staticData)
    {
      _assets = assets;
      _progressService = progressService;
      _inputService = inputService;
      _staticData = staticData;
    }
    
    public GameObject CreateHud()
    {
      GameObject hud = InstantiateRegistered(AssetPaths.HudPath);

      return hud;
    }

    public GameObject CreateJoystick(Transform under)
    {
      GameObject joystick = InstantiateRegistered(AssetPaths.JoystickPath, under);
      
      return joystick;
    }

    public GameObject CreateHero(Vector3 at)
    {
      GameObject Hero = InstantiateRegistered(AssetPaths.HeroPath, at);
      Hero.GetComponent<HeroMove>().Construct(_inputService);
      return Hero;    
    }

    public void CreateManufactureSpawner(Vector3 at, ResourceType resourceType)
    {
      ManufactureSpawnPoint spawner = InstantiateRegistered(AssetPaths.SpawnPoint, at).GetComponent<ManufactureSpawnPoint>();
      spawner.Construct(this, _staticData, _progressService);
      spawner.ResourceType = resourceType;
    }

    public GameObject CreateManufacture(GameObject template, Vector3 at)
    {
      GameObject manufactureObject = InstantiateRegistered(template, at);
      return manufactureObject;
    }

    public GameObject CreateResource(GameObject template, GameObject defaultParent)
    {
      GameObject resource = InstantiateRegistered(template, defaultParent.transform);
      return resource;
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
    }
    
    private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
    {
      GameObject gameObject = _assets.Instantiate(prefab, at);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }
    private GameObject InstantiateRegistered(GameObject prefab, Transform under)
    {
      GameObject gameObject = _assets.Instantiate(prefab, under);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
      GameObject gameObject = _assets.Instantiate(prefabPath);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }
    
    private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
    {
      GameObject gameObject = _assets.Instantiate(prefabPath, at);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath, Transform under)
    {
      GameObject gameObject = _assets.Instantiate(prefabPath, under);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }
    
    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        Register(progressReader);
    }

    private void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }
  }
}