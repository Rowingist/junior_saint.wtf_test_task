using System;
using System.Collections.Generic;
using Codebase.Infrastructure.Factory;
using Codebase.Services;
using Codebase.Services.Input;
using Codebase.Services.PersistentProgress;
using Codebase.Services.SaveLoad;
using Codebase.Services.StaticData;
using Codebase.UI.Animations;
using Codebase.UI.Services.Factory;

namespace Codebase.Infrastructure.States
{
  public class GameStateMachine : IGameStateMachine
  {
    private Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
    {
      _states = new Dictionary<Type, IExitableState>
      {
        [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),

        [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistentProgressService>(),
          services.Single<ISaveLoadService>(), services.Single<IStaticDataService>()),

        [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain,
          services.Single<IPersistentProgressService>(), services.Single<IGameFactory>(),
          services.Single<IStaticDataService>(), services.Single<IInputService>(), services.Single<IUIFactory>()),

        [typeof(GameLoopState)] = new GameLoopState(this, services.Single<IStaticDataService>(),
          services.Single<IPersistentProgressService>())
      };
    }
    
    public void Enter<TState>() where TState : class, IState
    {
      IState state = ChangeState<TState>();
      state.Enter();
    }

    public void Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad>
    {
      TState state = ChangeState<TState>();
      state.Enter(payLoad);
    }

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
      _activeState?.Exit();

      TState state = GetState<TState>();
      _activeState = state;

      return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState => 
      _states[typeof(TState)] as TState;
  }
}