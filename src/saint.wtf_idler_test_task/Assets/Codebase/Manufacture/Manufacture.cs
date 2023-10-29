using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codebase.Areas;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Manufacture
{
  public abstract class Manufacture : MonoBehaviour
  {
    public Transform SpawnPoint;
    public Transform OutputPoint;
    public float ConveyorSpeed = 1f;
    public float ProducingCullDown = 1f;
    
    [SerializeField] private ReceiveArea _receiveArea;

    protected WaitForSecondsRealtime CullDown;
    
    private readonly List<Resource> _resourcesStorage = new List<Resource>();

    [field: SerializeField] public ResourceType TypeOutput { get; private set; }
    protected bool IsReceiveAreaFull => _receiveArea.Storage.IsFull;

    public void Construct(List<Resource> pooledResources)
    {
      foreach (var resource in pooledResources) 
        _resourcesStorage.Add(resource);
    }

    private void Awake() => 
      CullDown = new WaitForSecondsRealtime(ProducingCullDown);

    public abstract void Produce();
    public abstract IEnumerator Producing();
    public abstract IEnumerator MakeResource(float duration);
    
    protected bool TryGetPooledResource(out Resource resource)
    {
      resource = _resourcesStorage.FirstOrDefault(r => r.isActiveAndEnabled);

      return resource is not null;
    }

    protected void FinishTransition(Resource resource)
    {
      resource.IsPickable = false;
      _receiveArea.Receive(resource);
    }
  }
}