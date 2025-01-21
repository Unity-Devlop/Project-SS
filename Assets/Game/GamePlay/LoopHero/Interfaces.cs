using cfg;
using UnityEngine;

namespace Game.LoopHero.UI
{
    public interface IUICard
    {
        public Transform transform { get; }
        public int idx { get; }
        void Bind(ItemEnum id, int idx);
    }

    public interface ICardEffectExecute
    {
        bool OnEndDragCard(IUICard card);

        bool OnBeginDragCard(IUICard card);

        void OnDragCard(IUICard card);
    }
}