using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codebase.Areas;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.NewResourceManufacture
{
  public abstract class Manufacture : MonoBehaviour
  {
    public Transform SpawnPoint;
    public Transform OutputPoint;
    public float ConveyorSpeed = 1f;
    public float ProducingCullDown = 1f;
    
    [SerializeField] private ReceiveArea _receiveArea;

    protected WaitForSecondsRealtime CullDown;

    protected int Number;
    private readonly List<Resource> _resourcesStorage = new List<Resource>();
    public event Action<int, ResourceType, Manufacture> StoppedMaking;  
    public event Action<Manufacture> StartedMaking;

    [field: SerializeField] public ResourceType TypeOutput { get; private set; }
    [field: SerializeField] public GameObject ResourcesPool { get; private set; }
    
    protected bool IsReceiveAreaFull => _receiveArea.Storage.IsFull;

    public void Construct(List<Resource> pooledResources, int number)
    {
      foreach (var resource in pooledResources) 
        _resourcesStorage.Add(resource);

      Number = number;

      _receiveArea.FilledUp += OnStorageFilled;
      _receiveArea.UnFilled += OnFullStorageUnfilled;
    }

    private void OnFullStorageUnfilled() => 
      StartedMaking?.Invoke(this);

    private void OnStorageFilled(ResourceType type)
    {
      StoppedMaking?.Invoke(Number, type, this);
    }

    private void Awake() => 
      CullDown = new WaitForSecondsRealtime(ProducingCullDown);

    protected virtual void OnDestroy() => 
      _receiveArea.FilledUp -= OnStorageFilled;

    public abstract void Produce();
    public abstract IEnumerator Producing();
    public abstract IEnumerator MakeResource(float duration);
    
    protected bool TryGetPooledResource(out Resource resource)
    {
      resource = _resourcesStorage.FirstOrDefault(r => !r.isActiveAndEnabled);

      return resource is not null;
    }

    protected void EnableResource(Resource resource) => 
      resource.gameObject.SetActive(true);

    protected void FinishTransition(Resource resource)
    {
      resource.IsPickable = false;
      _receiveArea.Receive(resource);
    }
  }
}