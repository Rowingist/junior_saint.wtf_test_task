using System.Collections.Generic;
using System.Linq;
using Codebase.Logic;
using Codebase.MovingResource;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Codebase.Areas
{
  public class ReceiveArea : MonoBehaviour
  {
    [SerializeField] private Transform _centralPoint;
    [SerializeField] private Cell[] _cells;
    
    private List<Resource> _resources = new List<Resource>();

    public Vector3 CentralPoint => _centralPoint.position;

    private Cell _firstEmptyCell => _cells.FirstOrDefault(c => c.IsEmpty);
    private Cell _lastFilledCell => _cells.LastOrDefault(c => !c.IsEmpty);

    private void Start()
    {
      for (int i = 0; i < _cells.Length; i++)
      {
        Object newResource = Resources.Load("Buildings/Prefabs/Resource Red Variant 1");
        var resourceGO = Instantiate(newResource, transform.position, Quaternion.identity) as GameObject;
        Resource resource = resourceGO.GetComponent<Resource>();
        Receive(resource);
      }
    }

    public void Receive(Resource resource)
    {
      if (!_firstEmptyCell) return;
      
      resource.transform.DOMove(_firstEmptyCell.transform.position, Constants.ResourceMoveDuration);
      _firstEmptyCell.Fill();
      _resources.Add(resource);
    }

    private void SetNewParent(Transform target, Transform parent)
    {
      target.transform.parent = parent;
      target.transform.position = parent.position;
      target.transform.rotation = parent.rotation;
    }

    public bool TryGetResource(out Resource resource)
    {
      resource = null;

      if (!_lastFilledCell) return false;
      
      resource = _resources.Last();
        
      Drop(resource);
      return true;
    }

    private void Drop(Resource resource)
    {
      if (!_lastFilledCell) return;
      
      _lastFilledCell.Empty();
      _resources.Remove(resource);
    }
  }
}