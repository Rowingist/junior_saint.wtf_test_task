using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codebase.Logic;
using Codebase.MovingResource;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Codebase.Areas
{
  public class ReceiveArea : MonoBehaviour
  {
    [SerializeField] private Transform _centralPoint;
    [SerializeField] private Cell[] _cells;

    [SerializeField] private Storage _storage;
    
    private List<Resource> _resources = new List<Resource>();
    
    public bool IsGreen;
    
    private float smoothTime = 0.1f;

    public Vector3 CentralPoint => _centralPoint.position;

    private Cell _firstEmptyCell => _cells.FirstOrDefault(c => c.IsEmpty);
    private Cell _lastFilledCell => _cells.LastOrDefault(c => !c.IsEmpty);

    private void Start()
    {
      for (int i = 0; i < _cells.Length; i++)
      {
        Object newResource = new Object();
        if(!IsGreen)
          newResource = Resources.Load("Buildings/Prefabs/Resource Red Variant 1");
        else
          newResource = Resources.Load("Buildings/Prefabs/Resource Green Variant 2");

        var resourceGO = Instantiate(newResource, transform.position, Quaternion.identity) as GameObject;
        Resource resource = resourceGO.GetComponent<Resource>();
        Receive(resource);
      }
    }

    public void Receive(Resource resource)
    {
      if (!_firstEmptyCell) return;

      StartCoroutine(Transmit(resource.transform, _firstEmptyCell.transform));
      _firstEmptyCell.Fill(resource.Type);
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
    
    private IEnumerator Transmit(Transform resource, Transform target)
    {
      Vector3 currentVelocity = Vector3.zero;
      while ((resource.position - target.position).sqrMagnitude > Constants.StopTransitDistance)
      {
        resource.position = Vector3.SmoothDamp(resource.position, target.position, ref currentVelocity, smoothTime);
        yield return null;
      }

      SetNewParent(resource, target);
    }
  }
}