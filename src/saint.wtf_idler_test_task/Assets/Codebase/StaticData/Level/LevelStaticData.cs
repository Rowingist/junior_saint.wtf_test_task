using UnityEngine;

namespace Codebase.StaticData.Level
{
  [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
  public class LevelStaticData : ScriptableObject
  {
    public string LevelKey;
    public Vector3 InitialHeroPosition;
  }
}