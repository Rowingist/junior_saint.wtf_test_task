using System;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.StaticData.Level
{
  [Serializable]
  public class ManufactureSpawnerStaticData
  {
    public ResourceType ResourceType;
    public Vector3 Position;

    public ManufactureSpawnerStaticData(ResourceType resourceType, Vector3 position)
    {
      ResourceType = resourceType;
      Position = position;
    }
  }
}