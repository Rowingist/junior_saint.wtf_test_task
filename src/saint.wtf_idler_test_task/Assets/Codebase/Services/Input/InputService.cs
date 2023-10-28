using UnityEngine;

namespace Codebase.Services.Input
{
  public abstract class InputService : IInputService
  {
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    
    public abstract Vector2 Axis { get; }

    protected static Vector2 SimpleInputAxis()
    {
      return new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
  }
}