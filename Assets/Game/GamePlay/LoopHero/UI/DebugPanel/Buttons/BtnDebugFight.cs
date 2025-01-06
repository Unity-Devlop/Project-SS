using Cysharp.Threading.Tasks;
using Game.Flow;
using Game.LoopHero;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Buttons
{
    public class BtnDebugFight : Button
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (Global.Get<GameFlow>().currentState is not GamePlayState)
            {
                Global.Get<GameFlow>().Change<GamePlayState>();
            }

            UniTask.WaitUntil(() => Global.Get<GameFlow>().currentState is GamePlayState).ContinueWith(() =>
            {
                FightMgr.Singleton.DebugFight();
            });
        }
    }
}