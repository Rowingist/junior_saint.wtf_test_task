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

    public GameObject CreateWarningContainer(GameObject under)
    {
      WindowConfig config = _staticData.ForWindow(WindowId.WarningContainer);
      GameObject warningsContainer = _assets.Instantiate(config.Template.gameObject, under);

      return warningsContainer;
    }

    public GameObject CreateWarningItem(GameObject under)
    {
      WindowConfig config = _staticData.ForWindow(WindowId.WarningItem);
      GameObject warningItem = _assets.Instantiate(config.Template.gameObject, under);

      return warningItem;
    }
  }
}