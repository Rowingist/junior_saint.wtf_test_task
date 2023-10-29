using System.Collections;
using UnityEngine;

namespace Codebase.Logic
{
  public static class RoutineUtils
  {
    public static IEnumerator TransitToTarget(Transform obj, Vector3 target, float duration)
    {
      Vector3 startPosition = obj.position;
      
      float t = 0;
      while (t < 1)
      {
        obj.position = Vector3.Lerp(startPosition, target, t);
        
        t += Time.deltaTime / duration;

        yield return null;
      }
    }
    
    public static IEnumerator TransitFromToTarget(Transform obj, Vector3 start, Vector3 target, float duration)
    {
      Vector3 startPosition = obj.position;
      
      float t = 0;
      while (t < 1)
      {
        obj.position = Vector3.Lerp(startPosition, target, t);
        
        t += Time.deltaTime / duration;

        yield return null;
      }
    }
  }
}