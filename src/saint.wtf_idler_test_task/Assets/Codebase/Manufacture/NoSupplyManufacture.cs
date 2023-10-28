using System;
using System.Collections;
using Codebase.MovingResource;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Codebase.Manufacture
{
  public class NoSupplyManufacture : Manufacture
  {
    private void Start()
    {
      Produce();
    }

    public override void Produce()
    {
      StartCoroutine(Producing());
    }

    private IEnumerator Producing()
    {
      while (Application.isPlaying)
      {
        yield return new WaitUntil(() => !IsStopped);
        yield return CullDown;
        yield return StartCoroutine(MakeResource(ConveyorSpeed));
      }
    }

    private IEnumerator MakeResource(float duration)
    {
      Object resource = Resources.Load("Buildings/Prefabs/Resource Red Variant 1");
      GameObject resGO = Instantiate(resource, SpawnPoint) as GameObject;

      float t = 0;
      while (t < 1)
      {
        resGO.transform.position = Vector3.Lerp(SpawnPoint.position, OutputPoint.position, t);
        
        t += Time.deltaTime / duration;

        yield return null;
      }

      yield return CullDown;

      Resource resource1 = resGO.GetComponent<Resource>();
      resource1.IsPickable = false;
      ReceiveArea.Receive(resource1);
    }
  }
}