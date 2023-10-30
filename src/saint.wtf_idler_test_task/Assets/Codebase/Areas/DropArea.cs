using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Areas
{
  public class DropArea : MonoBehaviour
  {
    [SerializeField] private Storage[] _storages;
    [SerializeField] private Transform _centralPoint;

    public Vector3 CentralPoint => _centralPoint.position;

    private readonly List<Resource> _resources = new List<Resource>();

    public event Action<ResourceType> OutOfResource;
    public event Action Filled;
    
    public Storage[] Storages => _storages;

    private void Start() => 
      NotifyOfEmptyStorages();

    private void NotifyOfEmptyStorages()
    {
      foreach (Storage storage in _storages)
      {
        if (storage.IsEmpty)
          OutOfResource?.Invoke(storage.ResourceType);
      }
    }    
    
    private void NotifyOfFilledStorages()
    {
      foreach (Storage storage in _storages)
      {
        if (!storage.IsEmpty)
          Filled?.Invoke();
      }
    }

    public void Receive(Resource resource)
    {
      _resources.Add(resource);
      NotifyOfFilledStorages();
    }

    public void Drop()
    {
      if(_storages == Array.Empty<Storage>())
        return;

      Resource[] resources = new Resource[_storages.Length];

      for (int i = 0; i < _storages.Length; i++)
      {
        if (!_storages[i].IsEmpty)
        {
          resources[i] = _resources.LastOrDefault(r => r.Type == _storages[i].ResourceType);
        }
      }

      foreach (Resource resource in resources)
      {
        if (resource is null)
          return;
      }

      for (int i = 0; i < resources.Length; i++)
      {
        resources[i].transform.parent = null;
        resources[i].transform.position = Vector3.zero;
        
        resources[i].gameObject.SetActive(false);
        resources[i].BackToDefaultParent();
        
        _resources.Remove(resources[i]);
        _storages[i].LastFilledCell.Empty();
      }
      
      NotifyOfEmptyStorages();
    }

    public Storage GetStorageByResourceType(ResourceType targetResource)
    {
      foreach (Storage storage in _storages)
      {
        if (storage.ResourceType == targetResource)
          return storage;
      }
      
      return null;
    }

    public ResourceType[] GetStorageResourceTypes()
    {
      ResourceType[] types = new ResourceType[_storages.Length];

      for (int i = 0; i < _storages.Length; i++)
      {
        types[i] = _storages[i].ResourceType;
      }

      return types;
    }
  }
}