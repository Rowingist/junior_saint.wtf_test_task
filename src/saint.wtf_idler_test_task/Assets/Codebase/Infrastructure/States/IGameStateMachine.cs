using Codebase.Services;

namespace Codebase.Infrastructure.States
{
  public interface IGameStateMachine : IService
  {
    void Enter<TState>() where TState : class, IState;
    void Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad>;
  }
}