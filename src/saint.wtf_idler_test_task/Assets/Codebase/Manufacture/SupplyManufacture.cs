using System;
using System.Collections;
using System.Linq;
using Codebase.Areas;
using Codebase.Logic;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Manufacture
{
  public class SupplyManufacture : Manufacture
  {
    [SerializeField] private DropArea _dropArea;

    public override void Produce() =>
      StartCoroutine(Producing());

    public override IEnumerator Producing()
    {
      while (Application.isPlaying)
      {
        yield return new WaitUntil(() => !IsReceiveAreaFull && !AnyOfInputStoragesIsEmpty());
        yield return CullDown;
        yield return StartCoroutine(MakeResource(ConveyorSpeed));
      }
    }

    public override IEnumerator MakeResource(float duration)
    {
      if (TryGetPooledResource(out Resource resource))
      {
        _dropArea.Drop();

        yield return StartCoroutine(RoutineUtils.TransitFromToTarget(resource.transform, SpawnPoint.position,
          OutputPoint.position, duration));

        yield return CullDown;

        FinishTransition(resource);
      }
    }

    private bool AnyOfInputStoragesIsEmpty()
    {
      if (_dropArea.Storages == Array.Empty<Storage>()) return false;

      return _dropArea.Storages.Any(s => s.IsEmpty);
    }
  }
}