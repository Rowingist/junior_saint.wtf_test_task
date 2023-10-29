using System.Collections.Generic;
using System.Linq;
using Codebase.StaticData.Level;
using Codebase.StaticData.Windows;
using Codebase.UI.Services.Windows;
using UnityEngine;

namespace Codebase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string LevelsDataPath = "Static Data/Levels";
    private const string WindowsStaticDataPath = "Static Data/UI/WindowStaticData";

    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;

    public void Load()
    {
      _levels = Resources
        .LoadAll<LevelStaticData>(LevelsDataPath)
        .ToDictionary(x => x.LevelKey, x => x);

      _windowConfigs = Resources
        .Load<WindowStaticData>(WindowsStaticDataPath).Configs
        .ToDictionary(x => x.WindowId, x => x);
    }


    public LevelStaticData ForLevel(string sceneKey) =>
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
        ? staticData
        : null;


    public WindowConfig ForWindow(WindowId windowId) =>
      _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
        ? windowConfig
        : null;
  }
}