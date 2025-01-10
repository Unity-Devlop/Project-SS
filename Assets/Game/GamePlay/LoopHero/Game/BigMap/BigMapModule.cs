using UnityToolkit;

namespace Game.LoopHero
{
    public class BigMapModule : IState<GameMgr>
    {
        public StateMachine<BigMapModule> machine { get; private set; }
        private BigMapMgr _mgr;

        private BigMapData _data;

        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            machine = new StateMachine<BigMapModule>(this);
            machine.Add<PauseState>();
            machine.Add<WaitForStartState>();
            machine.Add<WalkState>();
            _mgr = BigMapMgr.Singleton;
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _data = stateMachine.GetParam<BigMapData>(nameof(BigMapData));
            stateMachine.RemoveParam(nameof(BigMapData));
            _mgr.Enter(_data);
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

            _mgr.OnUpdate();
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _mgr.Exit();
            machine.Stop();
        }
    }
}