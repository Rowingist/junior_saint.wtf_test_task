using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Logic
{
  public class Cell : MonoBehaviour
  {
    public bool IsEmpty { get; private set; }
    
    public ResourceType FilledResourceType { get; private set; }
    
    private void Awake()
    {
      IsEmpty = true;
      FilledResourceType = ResourceType.None;
    }

    public void Fill(ResourceType resourceType)
    {
      IsEmpty = false;
      FilledResourceType = resourceType;
    }

    public void Empty()
    {
      IsEmpty = true;
      FilledResourceType = ResourceType.None;
    }
  }
}