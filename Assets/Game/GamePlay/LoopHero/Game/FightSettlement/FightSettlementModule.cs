using Game.LoopHero.UI;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Game.LoopHero
{
    public class FightSettlementModule : IState<GameMgr>
    {
        private bool _settlementEnd;
        public FightSettlementModuleData data { get; private set; }
        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            GameLogger.Log.Debug("[FightSettlementModule] OnEnter");
         
            _settlementEnd = false;
            Assert.IsNull(data);
            data = stateMachine.GetParam<FightSettlementModuleData>(nameof(FightSettlementModuleData));
            stateMachine.RemoveParam(nameof(FightSettlementModuleData));
            var panel = UIRoot.Singleton.OpenPanel<FightSettlementPanel>();
            // TODO 使用数据进行结算
            panel.Bind(this);
            
        }
        
        public void SettlementEnd()
        {
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
            data = null;
            UIRoot.Singleton.Dispose<FightSettlementPanel>();
            GameLogger.Log.Debug("[FightSettlementModule] OnExit");
            _settlementEnd = false;
        }
    }
}