using Codebase.Services;
using UnityEngine;

namespace Codebase.UI.Services.Factory
{
  public interface IUIFactory : IService
  {
    GameObject CreateUIRoot();
    GameObject CreateWarningContainer(GameObject under);
    GameObject CreateWarningItem(GameObject under);
  }
}