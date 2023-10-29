using System;

namespace Codebase.Data
{
  [Serializable]
  public class Vector3Data
  {
    public float x;
    public float y;
    public float z;

    public Vector3Data(float X, float Y, float Z)
    {
      x = X;
      y = Y;
      z = Z;
    }
  }
}