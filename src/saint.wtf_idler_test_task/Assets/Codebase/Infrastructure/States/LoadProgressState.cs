using Codebase.Data;
using Codebase.Services.PersistentProgress;
using Codebase.Services.SaveLoad;
using Codebase.Services.StaticData;

namespace Codebase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadService;
    private readonly IStaticDataService _staticDataService;
    
    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService,
      ISaveLoadService saveLoadService, IStaticDataService staticDataService)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadService = saveLoadService;
      _staticDataService = staticDataService;
    }
    
    public void Enter()
    {
      LoadProgressOrInitNew();
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {
    }
    
    private void LoadProgressOrInitNew() =>
      _progressService.Progress =
        _saveLoadService.LoadProgress()
        ?? NewProgress();

    private PlayerProgress NewProgress()
    {
      PlayerProgress progress = new PlayerProgress(initialLevel: Constants.DefaultSceneName);

      progress.PlayerStats.BagCapacity = Constants.InitialBagCapacity;
      progress.PlayerStats.MoveSpeed = Constants.InitialMoveSpeed;
      
      return progress;
    }
  }
}