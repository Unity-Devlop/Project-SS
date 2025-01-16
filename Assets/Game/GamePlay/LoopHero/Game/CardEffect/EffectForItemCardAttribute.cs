using System;
using cfg;

namespace Game.LoopHero.CardEffect
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class EffectForItemCardAttribute : Attribute
    {
        public readonly ItemEnum cardId;
        public EffectForItemCardAttribute(ItemEnum cardId)
        {
            this.cardId = cardId;
        }
    }
}