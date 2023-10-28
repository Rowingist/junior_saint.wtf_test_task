using System;
using System.Linq;
using Codebase.Areas;
using UnityEngine;

namespace Codebase.Manufacture
{
  public class SupplyManufacture : Manufacture
  {
    [SerializeField] private Storage[] _inputStorages;


    public override void Produce()
    {
    }

    private bool AnyOfInputStoragesIsEmpty()
    {
      if (_inputStorages == Array.Empty<Storage>()) return false;

      return _inputStorages.Any(s => s.IsEmpty);
    }
  }
}