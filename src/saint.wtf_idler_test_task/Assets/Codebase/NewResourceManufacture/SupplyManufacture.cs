using System;
using System.Collections;
using System.Linq;
using Codebase.Areas;
using Codebase.Logic;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.NewResourceManufacture
{
  public class SupplyManufacture : Manufacture
  {
    [SerializeField] private DropArea _dropArea;

    public event Action<int, ResourceType, Manufacture> StoppedDigesting;
    public event Action<Manufacture> StartedDigesting;

    public void Start()
    {
      _dropArea.OutOfResource += OnAreaEmpty;
      _dropArea.Filled += OnAreaFilled;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      _dropArea.OutOfResource -= OnAreaEmpty;
      _dropArea.Filled -= OnAreaFilled;
    }

    private void OnAreaEmpty(ResourceType type)
    {
      StoppedDigesting?.Invoke(Number, type, this);
    }

    private void OnAreaFilled() => 
      StartedDigesting?.Invoke(this);

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
        EnableResource(resource);

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