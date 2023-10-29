using UnityEngine;

namespace Codebase.MovingResource
{
  public class Resource : MonoBehaviour
  {
    public ResourceType Type = ResourceType.Red;
    public bool IsPickable;

    private Transform _defaultParent;

    public void Construct(Transform defaultParent) => 
      _defaultParent = defaultParent;

    public void BackToDefaultParent()
    {
      transform.position = _defaultParent.position;
      transform.parent = _defaultParent;
    }
  }
}