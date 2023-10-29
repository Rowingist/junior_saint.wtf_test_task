using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codebase.Logic;
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

      StartCoroutine(Transmit(resource, Storage.FirstEmptyCell.transform));
      Storage.FirstEmptyCell.Fill(resource.Type);
      _resources.Add(resource);
    }

    private void SetNewParent(Resource target, Transform parent)
    {
      target.transform.parent = parent;
      target.transform.position = parent.position;
      target.transform.rotation = parent.rotation;
      target.IsPickable = true;
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

    private IEnumerator Transmit(Resource resource, Transform target)
    {
      yield return StartCoroutine(RoutineUtils.TransitToTarget(resource.transform, target.position,
        Constants.ResourceFromToPlayerMoveDuration));
      
      SetNewParent(resource, target);
    }
  }
}