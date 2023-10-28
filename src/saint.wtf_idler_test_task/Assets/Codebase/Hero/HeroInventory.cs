using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codebase.Areas;
using Codebase.Logic;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Hero
{
  public class HeroInventory : MonoBehaviour
  {
    [SerializeField] private Cell[] _cells;
    [SerializeField] private TriggerObserver _triggerObserver;
    public float smoothTime = 5f;


    private List<Resource> _resources = new List<Resource>();
    private readonly WaitForSecondsRealtime _transitInterval = new WaitForSecondsRealtime(Constants.TransmittingInterval);
    
    private Cell _firstEmptyCell => _cells.FirstOrDefault(c => c.IsEmpty);

    private void Start()
    {
      _triggerObserver.TriggerEnter += TriggerEnter;
    }

    private void TriggerEnter(Collider obj)
    {
      if (obj.TryGetComponent(out ReceiveArea receiveArea))
      {
        StartCoroutine(Receiving(receiveArea));
      }
      else if (obj.TryGetComponent(out DropArea dropArea))
      {
        StartCoroutine(Dropping(dropArea));
      }
    }

    private IEnumerator Dropping(DropArea dropArea)
    {
      yield return _transitInterval;
    }

    private IEnumerator Receiving(ReceiveArea receiveArea)
    {
      while (TryGetEmptyCell(out var availableCell))
      {
        Receive(receiveArea, availableCell);
        yield return _transitInterval;
      }
    }

    private void SetNewParent(Transform target, Transform parent)
    {
      target.transform.parent = parent;
      target.transform.position = parent.position;
      target.transform.rotation = parent.rotation;
    }

    private bool TryGetEmptyCell(out Cell cell)
    {
      cell = null;

      if (_firstEmptyCell)
      {
        cell = _firstEmptyCell;
        return true;
      }

      return false;
    }

    private bool AbleToTransitResource(ReceiveArea receiveArea) =>
      (transform.position - receiveArea.CentralPoint).sqrMagnitude < Constants.StartTransmittingDistance;

    private void OnDestroy()
    {
      _triggerObserver.TriggerEnter -= TriggerEnter;
    }

    private void Drop(Vector3 target)
    {
    }

    private void Receive(ReceiveArea receiveArea, Cell availableCell)
    {
      if (!AbleToTransitResource(receiveArea)) return;
      
      availableCell.Fill();

      if (!receiveArea.TryGetResource(out var resource)) return;
      
      _resources.Add(resource);

      StartCoroutine(Transmit(resource.transform, availableCell.transform));
    }

    private void Resort()
    {
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