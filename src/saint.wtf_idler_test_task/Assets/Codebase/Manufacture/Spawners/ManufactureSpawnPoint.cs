using System.Collections.Generic;
using System.Linq;
using Codebase.Data;
using Codebase.Infrastructure.Factory;
using Codebase.MovingResource;
using Codebase.Services.PersistentProgress;
using Codebase.Services.StaticData;
using Codebase.StaticData.Level;
using UnityEngine;

namespace Codebase.Manufacture.Spawners
{
  public class ManufactureSpawnPoint : MonoBehaviour, ISavedProgress
  {
    public ResourceType ResourceType;
    
    private IGameFactory _gameFactory;
    private IStaticDataService _staticData;
    private IPersistentProgressService _progressService;

    public void Construct(IGameFactory gameFactory, IStaticDataService staticData, IPersistentProgressService progressService)
    {
      _gameFactory = gameFactory;
      _staticData = staticData;
      _progressService = progressService;
    }

    private void Spawn()
    {
      List<ManufactureConfig> manufactureConfigs = _staticData.ForLevel(_progressService.Progress.LastLevel).Configs;

      ManufactureConfig manufacture = manufactureConfigs.FirstOrDefault(m => m.Type == ResourceType);
      _gameFactory.CreateManufacture(manufacture.Template, transform.position);
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