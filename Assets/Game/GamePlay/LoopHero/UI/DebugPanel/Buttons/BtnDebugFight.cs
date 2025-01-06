using Cysharp.Threading.Tasks;
using Game.Flow;
using Game.LoopHero;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Buttons
{
    public class BtnDebugFight : Button
    {
        public override async void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (Global.Get<GameFlow>().currentState is not GamePlayState)
            {
                Global.Get<GameFlow>().Change<GamePlayState>();
                await UniTask.WaitUntil(() => Global.Get<GameFlow>().currentState is GamePlayState,
                    cancellationToken: destroyCancellationToken);
                await UniTask.DelayFrame(60, cancellationToken: destroyCancellationToken);
                await UniTask.WaitUntil(() => FightMgr.SingletonNullable != null,
                    cancellationToken: destroyCancellationToken);
            }


            FightMgr.Singleton.DebugFight();
        }
    }
}