using UnityToolkit;

namespace Game.LoopHero
{
    public class BigMapModule : IState<GameMgr>
    {
        public StateMachine<BigMapModule> machine { get; private set; }

        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            machine.Add<PauseState>();
            machine.Add<WaitForStartState>();
            machine.Add<WalkState>();
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            machine.Run<WaitForStartState>();
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            if (machine.running)
            {
                machine.OnUpdate();
            }
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            machine.Stop();
        }
    }
}