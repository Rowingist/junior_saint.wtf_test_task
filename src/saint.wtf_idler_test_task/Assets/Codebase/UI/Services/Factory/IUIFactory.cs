using Codebase.Services;
using UnityEngine;

namespace Codebase.UI.Services.Factory
{
  public interface IUIFactory : IService
  {
    GameObject CreateUIRoot();
    GameObject CreateWarningWindow(Transform under);
    GameObject CreateWarningItem(Transform under);
  }
}