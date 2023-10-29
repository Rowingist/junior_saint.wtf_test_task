using System;

namespace Codebase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public string LastLevel;
    public WorldData WorldData;
    public Stats PlayerStats;

    public PlayerProgress(string initialLevel)
    {
      LastLevel = initialLevel;
      WorldData = new WorldData(initialLevel);
      PlayerStats = new Stats();
    }
  }
}