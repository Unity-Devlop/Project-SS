using cfg;

namespace Game.LoopHero.UI
{
    public interface ILoopHeroItem
    {
        public ItemEnum id { get; }
    }

    public interface ILoopHeroCard
    {
        public int idx { get; }
        void Bind(ItemEnum id, int idx);
    }
}