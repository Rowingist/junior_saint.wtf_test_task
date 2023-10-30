using Codebase.MovingResource;
using Codebase.NewResourceManufacture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.UI.Windows
{
  public class WarningItem : WindowBase
  {
    [SerializeField] private TMP_Text _warning;
    [SerializeField] private Image _itemImage;
    
    private Manufacture _manufacture;

    public void Construct(Manufacture manufacture)
    {
      _manufacture = manufacture;

      _manufacture.StartedMaking += CleanUp;
      if (_manufacture is SupplyManufacture supplyManufacture)
        supplyManufacture.StartedDigesting += CleanUp;
    }

    public override void OnDestroy()
    {
      _manufacture.StartedMaking -= CleanUp;
      if (_manufacture is SupplyManufacture supplyManufacture)
        supplyManufacture.StartedDigesting -= CleanUp;
    }

    private void CleanUp(Manufacture manufacture)
    {
      if(manufacture == _manufacture)
        Destroy(gameObject);
    }

    public void Construct(int manufactureNumber, ResourceType type, string reason, Sprite sprite)
    {
      _warning.text = $"Factory #{manufactureNumber} {reason} {type} brick";
      _itemImage.sprite = sprite;
    }
  }
}