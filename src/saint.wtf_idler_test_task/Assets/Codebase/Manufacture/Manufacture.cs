using System;
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

    [SerializeField] private ResourceType _typeOutput;
    [SerializeField] private ReceiveArea _receiveArea;

    protected bool IsReceiveAreaFull => _receiveArea.Storage.IsFull;
    protected ReceiveArea ReceiveArea => _receiveArea;
    protected WaitForSecondsRealtime CullDown;
    protected GameObject CurrentResource;

    public abstract void Produce();

    private void Awake()
    {
      CullDown = new WaitForSecondsRealtime(ProducingCullDown);
    }
  }
}