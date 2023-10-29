using System.Collections.Generic;
using Codebase.Services.PersistentProgress;
using UnityEngine;

namespace Codebase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    
    public GameObject CreateHud()
    {
      throw new System.NotImplementedException();
    }

    public GameObject CreateJoystick(Transform under)
    {
      throw new System.NotImplementedException();
    }

    public GameObject CreateHero(Vector3 at)
    {
      throw new System.NotImplementedException();
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
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