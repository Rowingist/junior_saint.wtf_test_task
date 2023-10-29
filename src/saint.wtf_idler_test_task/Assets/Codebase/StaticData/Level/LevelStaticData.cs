using System.Collections.Generic;
using Codebase.UI.Services.Windows;
using Codebase.UI.Windows;
using UnityEngine;

namespace Codebase.StaticData.Level
{
  [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
  public class LevelStaticData : ScriptableObject
  {
    public string LevelKey;
    public Vector3 InitialHeroPosition;
    
    public int Resource1PoolCapacity;
    public int Resource2PoolCapacity;
    public int Resource3PoolCapacity;

    public List<ManufactureConfig> Configs;
    public List<ManufactureSpawnerStaticData> ManufactureSpawners;
  }
}