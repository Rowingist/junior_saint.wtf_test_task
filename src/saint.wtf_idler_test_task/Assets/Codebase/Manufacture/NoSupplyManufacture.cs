using System.Collections;
using Codebase.Logic;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Manufacture
{
  public class NoSupplyManufacture : Manufacture
  {
    public override void Produce() => 
      StartCoroutine(Producing());

    public override IEnumerator Producing()
    {
      while (Application.isPlaying)
      {
        yield return new WaitUntil(() => !IsReceiveAreaFull);
        yield return CullDown;
        yield return StartCoroutine(MakeResource(ConveyorSpeed));
      }
    }

    public override IEnumerator MakeResource(float duration)
    {
      if (TryGetPooledResource(out Resource resource))
      {
        EnableResource(resource);
        
        yield return StartCoroutine(RoutineUtils.TransitFromToTarget(resource.transform, SpawnPoint.position,
          OutputPoint.position, duration));

        yield return CullDown;

        FinishTransition(resource);
      }
    }
  }
}