using UnityToolkit;

namespace Game.LoopHero
{
    public class FightSettlementModule : IState<GameMgr>
    {
        private bool _settlementEnd;

        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            GameLogger.Log.Debug("[FightSettlementModule] OnEnter");
            var data = stateMachine.GetParam<FightSettlementModuleData>(nameof(FightSettlementModuleData));
            stateMachine.RemoveParam(nameof(FightSettlementModuleData));
            // TODO 使用数据进行结算
            _settlementEnd = true;
        }


        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            if (_settlementEnd)
            {
                stateMachine.Change<BigMapModule>();
            }
        }

        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            GameLogger.Log.Debug("[FightSettlementModule] OnExit");
            _settlementEnd = false;
        }
    }
}