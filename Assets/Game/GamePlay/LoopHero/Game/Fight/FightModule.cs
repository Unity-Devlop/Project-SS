using UnityToolkit;

namespace Game.LoopHero
{
    public class FightModule : IState<GameMgr>
    {
        private FightModuleData _data;
        private bool _isSelfWin;
        private bool _isFightEnd;

        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _data = stateMachine.GetParam<FightModuleData>(nameof(FightModuleData));
            stateMachine.RemoveParam(nameof(FightModuleData));
            FightMgr.Singleton.StartFight(_data, OnFightEnd); // TODO
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            if(_isFightEnd)
            {
                // TODO 传递战斗结果
                stateMachine.Change<FightSettlementModule>();
            }
        }

        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        private void OnFightEnd(in FightMgr.FightResult result)
        {
            // TODO 结算数据 恢复一些Buff状态
            _isFightEnd = true;
            _isSelfWin = result.isSelfWin;
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            FightMgr.Singleton.DisableLogic();
        }
    }
}