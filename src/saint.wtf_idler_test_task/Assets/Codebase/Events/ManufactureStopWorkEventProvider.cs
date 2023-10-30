using System;
using System.Collections.Generic;
using Codebase.MovingResource;
using Codebase.NewResourceManufacture;
using UnityEngine;

namespace Codebase.Events
{
  public class ManufactureStopWorkEventProvider : MonoBehaviour
  {
    private readonly List<Manufacture> _manufactures = new List<Manufacture>();

    public event Action<int, ResourceType, string, Manufacture> ManufactureStoppedWork;

    public void Construct(List<Manufacture> manufactures)
    {
      FillManufactures(manufactures);

      Subscribe();
    }

    private void OnDestroy() => CleanUnp();

    private void Subscribe()
    {
      foreach (Manufacture manufacture in _manufactures)
      {
        manufacture.StoppedMaking += OnStopManufactureMaking;
      
        if (manufacture is SupplyManufacture supplyManufacture)
          supplyManufacture.StoppedDigesting += OnStopManufactureDigest;
      }
    }
    
    private void CleanUnp()
    {
      foreach (Manufacture manufacture in _manufactures)
      {
        manufacture.StoppedMaking -= OnStopManufactureMaking;
      
        if (manufacture is SupplyManufacture supplyManufacture)
          supplyManufacture.StoppedDigesting -= OnStopManufactureDigest;
      }
    }

    private void OnStopManufactureDigest(int manufactureNumber, ResourceType resourceType, Manufacture manufacture)
    {
      ManufactureStoppedWork?.Invoke(manufactureNumber, resourceType, Constants.NeedResource, manufacture);
    }

    private void OnStopManufactureMaking(int manufactureNumber, ResourceType resourceType, Manufacture manufacture)
    {
      ManufactureStoppedWork?.Invoke(manufactureNumber, resourceType, Constants.FilledUp, manufacture);
    }

    private void FillManufactures(List<Manufacture> manufactures)
    {
      foreach (Manufacture manufacture in manufactures)
        _manufactures.Add(manufacture);
    }
  }
}