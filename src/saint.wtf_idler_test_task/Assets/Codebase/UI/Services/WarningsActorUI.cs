using System.Linq;
using Codebase.MovingResource;
using Codebase.NewResourceManufacture;
using Codebase.Services.StaticData;
using Codebase.UI.Services.Factory;
using Codebase.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codebase.Events
{
  public class WarningsActorUI : MonoBehaviour
  {
    private IUIFactory _uiFactory;
    private IStaticDataService _staticData;
    private ManufactureStopWorkEventProvider _eventProvider;
    private GameObject _warningsContainer;

    public void Construct(IUIFactory uiFactory, ManufactureStopWorkEventProvider eventProvider,
      IStaticDataService staticData, GameObject warningsContainer)
    {
      _uiFactory = uiFactory;
      _staticData = staticData;
      _eventProvider = eventProvider;
      _warningsContainer = warningsContainer;

      Subscribe();
    }

    private void OnDestroy() => 
      CleanUp();

    private void Subscribe() => 
      _eventProvider.ManufactureStoppedWork += OnManufactureStopped;

    private void CleanUp() => 
      _eventProvider.ManufactureStoppedWork -= OnManufactureStopped;

    private void OnManufactureStopped(int number, ResourceType type, string reason, Manufacture manufacture)
    {
      string sceneKey = SceneManager.GetActiveScene().name;
      GameObject warningObject = _uiFactory.CreateWarningItem(_warningsContainer);
      WarningItem warningItem = warningObject.GetComponent<WarningItem>();
      warningItem.Construct(number, type, reason, _staticData.ForLevel(sceneKey).ResourceConfigs.FirstOrDefault(r => r.Template.GetComponent<Resource>().Type == type).GUIIcon);
      warningItem.Construct(manufacture);
    }
  }
}