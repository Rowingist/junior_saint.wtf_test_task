using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Areas
{
  public class ReceiveArea : MonoBehaviour
  {
    [SerializeField] private Transform _centralPoint;

    [field: SerializeField] public Storage Storage { get; private set; }
    
    private readonly List<Resource> _resources = new List<Resource>();

    public Vector3 CentralPoint => _centralPoint.position;

    public void Receive(Resource resource)
    {
      if (!Storage.FirstEmptyCell) return;

      StartCoroutine(Transmit(resource.transform, Storage.FirstEmptyCell.transform));
      Storage.FirstEmptyCell.Fill(resource.Type);
      _resources.Add(resource);
    }

    private void SetNewParent(Transform target, Transform parent)
    {
      target.transform.parent = parent;
      target.transform.position = parent.position;
      target.transform.rotation = parent.rotation;
      target.GetComponent<Resource>().IsPickable = true;
    }

    public bool TryGetResource(out Resource resource)
    {
      resource = null;

      if (!Storage.LastFilledCell) return false;
      
      resource = _resources.Last();
        
      Drop(resource);
      return true;
    }

    private void Drop(Resource resource)
    {
      if (!Storage.LastFilledCell) return;
      
      Storage.LastFilledCell.Empty();
      _resources.Remove(resource);
    }
    
    private IEnumerator Transmit(Transform resource, Transform target)
    {
      Vector3 startPosition = resource.position;
      float t = 0;
      
      while (t < 1)
      {
        resource.position = Vector3.Lerp(startPosition, target.position, t);

        t += Time.deltaTime / Constants.ResourceMoveDuration;
        yield return null;
      }

      SetNewParent(resource, target);
    }
  }
}