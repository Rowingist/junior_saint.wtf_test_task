using Codebase.Services.PersistentProgress;
using Codebase.Services.StaticData;

namespace Codebase.Infrastructure.States
{
  public class GameLoopState : IState
  {
    private readonly IStaticDataService _staticDataService;
    private readonly IPersistentProgressService _progressService;

    public GameLoopState(GameStateMachine gameStateMachine, IStaticDataService staticDataService,
      IPersistentProgressService progressService)
    {
      _staticDataService = staticDataService;
      _progressService = progressService;
    }

    public void Exit()
    {

    }

    public void Enter()
    {
    }
  }
}