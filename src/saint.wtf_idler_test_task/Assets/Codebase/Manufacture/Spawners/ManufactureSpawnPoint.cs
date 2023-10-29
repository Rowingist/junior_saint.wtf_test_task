using System.Collections.Generic;
using System.Linq;
using Codebase.Data;
using Codebase.Infrastructure.Factory;
using Codebase.MovingResource;
using Codebase.Services.PersistentProgress;
using Codebase.Services.StaticData;
using Codebase.StaticData.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codebase.Manufacture.Spawners
{
  public class ManufactureSpawnPoint : MonoBehaviour, ISavedProgress
  {
    public ResourceType ResourceType;

    private IGameFactory _gameFactory;
    private IStaticDataService _staticData;
    private IPersistentProgressService _progressService;

    public void Construct(IGameFactory gameFactory, IStaticDataService staticData,
      IPersistentProgressService progressService)
    {
      _gameFactory = gameFactory;
      _staticData = staticData;
      _progressService = progressService;
    }

    private void Spawn()
    {
      List<ManufactureConfig> manufactureConfigs = _staticData.ForLevel(_progressService.Progress.LastLevel).Configs;

      ManufactureConfig manufacture =
        manufactureConfigs.FirstOrDefault(m => m.Template.GetComponent<Manufacture>().TypeOutput == ResourceType);

      GameObject newManufacture = _gameFactory.CreateManufacture(manufacture.Template, transform.position);

      CreateResources(newManufacture);
      newManufacture.GetComponent<Manufacture>().Produce();
    }

    private void CreateResources(GameObject newManufacture)
    {
      string sceneKey = SceneManager.GetActiveScene().name;
      int resourcePoolsCapacity = _staticData.ForLevel(sceneKey).ResourcePoolsCapacity;
      Manufacture manufacture = newManufacture.GetComponent<Manufacture>();

      List<Resource> resources = new List<Resource>();
      
      for (int i = 0; i < resourcePoolsCapacity; i++)
      {
        GameObject resource = _gameFactory.CreateResource(
          _staticData.ForLevel(sceneKey).ResourceConfigs
            .FirstOrDefault(c => c.Template.GetComponent<Resource>().Type == manufacture.TypeOutput).Template, 
          manufacture.ResourcesPool);


        Resource newResource = resource.GetComponent<Resource>();
        newResource.Construct(manufacture.ResourcesPool.transform);
        resources.Add(newResource);
        
        resource.SetActive(false);
      }
      
      manufacture.Construct(resources);
    }

    public void LoadProgress(PlayerProgress progress)
    {
      Spawn();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
    }
  }
}