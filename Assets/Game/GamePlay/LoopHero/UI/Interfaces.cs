using cfg;

namespace Game.LoopHero.UI
{
    public interface ILoopHeroItem
    {
        public ItemEnum id { get; }
    }

    public interface ILoopHeroCard
    {
        void Bind(ItemEnum id);
    }
}