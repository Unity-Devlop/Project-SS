using Game.LoopHero;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Buttons
{
    public class BtnDebugFight: Button
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            FightMgr.Singleton.DebugFight();
        }
    }
}