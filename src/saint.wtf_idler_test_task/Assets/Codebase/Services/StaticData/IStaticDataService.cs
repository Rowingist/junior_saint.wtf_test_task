using Codebase.StaticData.Level;
using Codebase.StaticData.Windows;
using Codebase.UI.Services.Windows;

namespace Codebase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    LevelStaticData ForLevel(string sceneKey);
    WindowConfig ForWindow(WindowId windowId);
  }
}