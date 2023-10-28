using Codebase.Logic;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Areas
{
  public class DropArea : MonoBehaviour
  {
    [SerializeField] private Storage[] _storages;
    [SerializeField] private Transform _centralPoint;

    public Vector3 CentralPoint => _centralPoint.position;
    
    public Storage GetStorageByResourceType(ResourceType targetResource)
    {
      foreach (Storage storage in _storages)
      {
        if (storage.ResourceType == targetResource)
          return storage;
      }

      Debug.LogError("No capable storage available.");
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