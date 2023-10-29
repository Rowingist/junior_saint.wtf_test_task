using Codebase.Data;

namespace Codebase.Services.PersistentProgress
{
  public class PersistentProgressService : IPersistentProgressService
  {
    public PlayerProgress Progress { get; set; }
  }
}