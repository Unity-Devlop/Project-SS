using UnityToolkit;

namespace Game.LoopHero
{
    public class FightModule : IState<GameMgr>
    {
        private FightModuleData _data;

        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            FightMgr.Singleton.enabled = true;
            _data = stateMachine.GetParam<FightModuleData>(nameof(FightModuleData));
            stateMachine.RemoveParam(nameof(FightModuleData));
            FightMgr.Singleton.StartFight(_data);
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            FightMgr.Singleton.Clear();
            FightMgr.Singleton.enabled = false;
        }
    }
}