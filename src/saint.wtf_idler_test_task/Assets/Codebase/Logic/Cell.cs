using UnityEngine;

namespace Codebase.Logic
{
  public class Cell : MonoBehaviour
  {
    public bool IsEmpty { get; private set; }

    private void Awake() => 
      IsEmpty = true;

    public void Fill() => 
      IsEmpty = false;
    
    public void Empty() => 
      IsEmpty = true;
  }
}