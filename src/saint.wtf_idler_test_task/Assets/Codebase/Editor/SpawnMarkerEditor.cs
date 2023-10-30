using Codebase.NewResourceManufacture.Spawners;
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
      Gizmos.color = new Color32(181,102,24, 162);
      Gizmos.DrawCube(spawner.transform.position, spawner.Size);
    }
  }
}