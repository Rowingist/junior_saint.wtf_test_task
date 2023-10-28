using System.Collections.Generic;
using System.Linq;
using Codebase.Logic;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.Areas
{
  public class Storage : MonoBehaviour
  {
    [SerializeField] private ResourceType _resourceType;

    [SerializeField] private List<Cell> _cells;

    public ResourceType ResourceType => _resourceType;
    
    public Cell FirstEmptyCell => _cells.FirstOrDefault(c => c.IsEmpty);
  }
}