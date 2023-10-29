using System.Collections.Generic;
using UnityEngine;

namespace Codebase.StaticData.Level
{
  [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
  public class LevelStaticData : ScriptableObject
  {
    public string LevelKey;
    public Vector3 InitialHeroPosition;
    
    public int ResourcePoolsCapacity;

    public List<ManufactureConfig> Configs;
    public List<ManufactureSpawnerStaticData> ManufactureSpawners;
    
    public List<ResourceConfig> ResourceConfigs;
  }
}