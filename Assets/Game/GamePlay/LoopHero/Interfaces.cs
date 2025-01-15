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

    public interface ILoopHeroEntity
    {
        public EntityTypeEnum entityType { get; }
    }
    
    public interface ILoopHeroGroupEntity : ILoopHeroEntity
    {
        public GroupEnum groupEnum { get; }
    }

    public interface ILoopHeroPokemon : ILoopHeroEntity
    {
    }

    public interface ILoopHeroBuilding : ILoopHeroEntity
    {
    }

    public interface ILoopHeroTerrain : ILoopHeroEntity
    {
    }
}