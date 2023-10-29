using System;

namespace Codebase.Data
{
  [Serializable]
  public class WorldData
  {
    public string LastLevelName;
    public PositionOnLevel PositionOnLevel;

    public WorldData(string initialLevel)
    {
      PositionOnLevel = new PositionOnLevel(initialLevel);
    }
  }
}