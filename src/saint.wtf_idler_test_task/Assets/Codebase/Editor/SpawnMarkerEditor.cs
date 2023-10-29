using Codebase.Manufacture.Spawners;
using UnityEditor;
using UnityEngine;

namespace Codebase.Editor
{
  [CustomEditor(typeof(ManufactureSpawnMarker))]
  public class SpawnMarkerEditor : UnityEditor.Editor
  {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(ManufactureSpawnMarker spawner, GizmoType gizmo)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(spawner.transform.position, 0.5f);
    }
  }
}