using Codebase.Infrastructure.AssetManagement;
using Codebase.Services.StaticData;
using Codebase.StaticData.Windows;
using Codebase.UI.Services.Windows;
using Codebase.UI.Windows;
using UnityEngine;

namespace Codebase.UI.Services.Factory
{
  public class UIFactory : IUIFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    
    public UIFactory(IAssetProvider assets, IStaticDataService staticData)
    {
      _assets =  assets;
      _staticData = staticData;
    }
    
    public GameObject CreateUIRoot() => 
      _assets.Instantiate(AssetPaths.UIRootPath);

    public GameObject CreateWarningWindow(Transform under)
    {
      WindowConfig config = _staticData.ForWindow(WindowId.WarningContainer);
      WarningWindow warningWindow = Object.Instantiate(config.Template, under) as WarningWindow;
      
      return warningWindow.gameObject;
    }

    public GameObject CreateWarningItem(Transform under)
    {
      WindowConfig config = _staticData.ForWindow(WindowId.WarningItem);
      WindowBase warningItem = Object.Instantiate(config.Template, under);

      return warningItem.gameObject;
    }
  }
}