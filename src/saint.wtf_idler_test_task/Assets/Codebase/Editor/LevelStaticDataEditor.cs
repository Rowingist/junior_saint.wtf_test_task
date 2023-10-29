using System.Linq;
using System.Xml;
using Codebase.Manufacture.Spawners;
using Codebase.StaticData.Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codebase.Editor
{
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor
  {
    private const string InitialPointTag = "InitialPoint";
    
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      LevelStaticData levelData = (LevelStaticData) target;

      if (GUILayout.Button("Collect"))
      {
        levelData.ManufactureSpawners = FindObjectsOfType<ManufactureSpawnMarker>()
          .Select(x => new ManufactureSpawnerStaticData(x.ResourceType, x.transform.position))
          .ToList();

        levelData.LevelKey = SceneManager.GetActiveScene().name;
        
        levelData.InitialHeroPosition =  GameObject.FindWithTag(InitialPointTag).transform.position;
      }
      
      EditorUtility.SetDirty(target);
    }
  }
}