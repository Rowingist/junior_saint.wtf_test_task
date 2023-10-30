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
    [SerializeField] private List<Cell> _cells;
    [SerializeField] private TriggerObserver _triggerObserver;
    public float smoothTime = 5f;


    private List<Resource> _resources = new List<Resource>();

    private Cell _firstEmptyCell => _cells.FirstOrDefault(c => c.IsEmpty);

    private readonly WaitForSecondsRealtime _transitInterval =
      new WaitForSecondsRealtime(Constants.TransmittingInterval);


    private Cell LastFilledCellWithType(ResourceType resourceType) =>
      _cells.LastOrDefault(c => c.FilledResourceType == resourceType);

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

    private float _elapsed;
    private IEnumerator Dropping(DropArea dropArea)
    {
      ResourceType[] resourceToDrop = dropArea.GetStorageResourceTypes();

      foreach (var type in resourceToDrop)
      {
        while (TryGetFilledCellByResourceType(type, out Cell availableCell))
        {
          if (AbleToTransitResource(dropArea))
          {
            int targetCell = _cells.IndexOf(availableCell);

            Storage storage = dropArea.GetStorageByResourceType(type);

            Cell cellInDropStorage = storage.FirstEmptyCell;

            if (cellInDropStorage && targetCell < _resources.Count)
            {
              StartCoroutine(Transmit(_resources[targetCell],
                cellInDropStorage.transform));

              dropArea.Receive(_resources[targetCell]);
              cellInDropStorage.Fill(type);
              
              _resources[targetCell] = null;
              _cells[targetCell].Empty();
              Resort();
            }
            else
            {
              break;
            }

            Resort();
          }

          yield return _transitInterval;
        }
      }
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

    private bool TryGetFilledCellByResourceType(ResourceType resourceType, out Cell cell)
    {
      cell = null;

      Cell filledCellWithType = LastFilledCellWithType(resourceType);

      if (filledCellWithType)
      {
        cell = filledCellWithType;
        return true;
      }

      return false;
    }

    private bool AbleToTransitResource(ReceiveArea receiveArea) =>
      (transform.position - receiveArea.CentralPoint).sqrMagnitude < Constants.StartTransmittingDistance;

    private bool AbleToTransitResource(DropArea receiveArea) =>
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

      if (!receiveArea.TryGetResource(out var resource)) return;
      
      availableCell.Fill(resource.Type);

      _resources.Add(resource);

      StartCoroutine(Transmit(resource, availableCell.transform));
    }

    private void Resort()
    {
      _resources.RemoveAll(r => !r);

      for (int i = 0; i < _cells.Count; i++)
      {
        if (_cells[i].IsEmpty)
        {
          for (int j = i + 1; j < _cells.Count; j++)
          {
            if (!_cells[j].IsEmpty)
            {
              _cells[i].Fill(_cells[j].FilledResourceType);
              _cells[j].Empty();
              break;
            }
          }
        }
      }

      for (int i = 0; i < _resources.Count; i++)
      {
        if (_resources[i].Type == _cells[i].FilledResourceType)
          _resources[i].transform.position = _cells[i].transform.position;
      }
    }

    private IEnumerator Transmit(Resource resource, Transform target)
    {
      if(!resource.IsPickable)
        yield return new WaitUntil(() => resource.IsPickable);
      
      resource.transform.parent = null;
      yield return StartCoroutine(RoutineUtils.TransitToTarget(resource.transform, target.position, Constants.ResourceFromToPlayerMoveDuration));

      SetNewParent(resource.transform, target);
    }
  }
}