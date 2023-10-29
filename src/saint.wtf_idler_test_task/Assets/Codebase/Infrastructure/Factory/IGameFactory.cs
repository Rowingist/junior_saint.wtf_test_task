using System.Collections.Generic;
using Codebase.MovingResource;
using Codebase.Services;
using Codebase.Services.PersistentProgress;
using UnityEngine;

namespace Codebase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    
    GameObject CreateHud();
    GameObject CreateJoystick(Transform under);
    GameObject CreateHero(Vector3 at);
    void CreateManufactureSpawner(Vector3 at, ResourceType resourceType);
    GameObject CreateManufacture(GameObject template, Vector3 at);
    GameObject CreateResource(GameObject template, GameObject defaultParent);
    
    void Cleanup();
  }
}