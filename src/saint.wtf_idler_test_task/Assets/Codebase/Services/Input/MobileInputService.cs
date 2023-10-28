using UnityEngine;

namespace Codebase.Services.Input
{
  public class MobileInputService : InputService
  {
    public override Vector2 Axis => SimpleInputAxis();
  }
}