using UnityToolkit;

namespace Game.LoopHero
{
    public class CampModule : IState<GameMgr>
    {
        private CampMgr _mgr;

        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _mgr = CampMgr.Singleton;
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _mgr.enabled = true;
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _mgr.OnUpdate();
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _mgr.enabled = false;
        }
    }
}