using Cysharp.Threading.Tasks;
using UnityToolkit;

namespace Game.LoopHero
{
    public class FightModule : IState<GameMgr>
    {
        private FightModuleData _data;
        private bool _isLocalPlayerWin;
        private bool _isFightEnd;
        private FightMgr _mgr;

        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _mgr = FightMgr.Singleton;
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _isFightEnd = false;
            _isLocalPlayerWin = false;
            
            GameLogger.Log.Information("FightModule OnEnter");
            _data = stateMachine.GetParam<FightModuleData>(nameof(FightModuleData));
            stateMachine.RemoveParam(nameof(FightModuleData));
            _mgr.StartFight(_data, OnFightEnd).Forget(); // TODO
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            if (_isFightEnd)
            {
                // TODO 传递战斗结果
                stateMachine.SetParam(nameof(FightSettlementModuleData), new FightSettlementModuleData
                {
                    isLocalPlayerWin = _isLocalPlayerWin
                    
                });
                stateMachine.Change<FightSettlementModule>();
            }
        }

        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            _mgr.OnUpdate();
        }


        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            GameLogger.Log.Information("FightModule OnExit");
            _mgr.ExitFight();
        }

        private void OnFightEnd(in FightMgr.FightResult result)
        {
            // TODO 结算数据 恢复一些Buff状态
            _isFightEnd = true;
            _isLocalPlayerWin = result.isSelfWin;
        }
    }
}