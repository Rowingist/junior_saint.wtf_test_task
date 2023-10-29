using System;
using Codebase.UI.Services.Windows;
using Codebase.UI.Windows;

namespace Codebase.StaticData.Windows
{
  [Serializable]
  public class WindowConfig
  {
    public WindowId WindowId;
    public WindowBase Template;
  }
}