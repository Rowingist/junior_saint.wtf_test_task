using System;
using System.Collections;
using System.Linq;
using Codebase.Areas;
using Codebase.MovingResource;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Codebase.Manufacture
{
  public class SupplyManufacture : Manufacture
  {
    [SerializeField] private DropArea _dropArea;

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
        yield return new WaitUntil(() => !IsReceiveAreaFull && !AnyOfInputStoragesIsEmpty());
        yield return CullDown;
        yield return StartCoroutine(MakeResource(ConveyorSpeed));
      }
    }

    private IEnumerator MakeResource(float duration)
    {
      Object resource = Resources.Load("Buildings/Prefabs/Resource Green Variant 2");
      GameObject resGO = Instantiate(resource, SpawnPoint) as GameObject;
      
      _dropArea.Drop();
      
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

    private bool AnyOfInputStoragesIsEmpty()
    {
      if (_dropArea.Storages == Array.Empty<Storage>()) return false;

      return _dropArea.Storages.Any(s => s.IsEmpty);
    }
  }
}