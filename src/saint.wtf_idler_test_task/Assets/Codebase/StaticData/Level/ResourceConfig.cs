using System;
using Codebase.MovingResource;
using UnityEngine;

namespace Codebase.StaticData.Level
{
  [Serializable]
  public class ResourceConfig
  {
    public Sprite GUIIcon;
    public ResourceType ResourceType;
    public GameObject Template;
  }
}