namespace Game.LoopHero
{
    /// <summary>
    /// 一把游戏的数据
    /// </summary>
    public class GamePlayData
    {
        public bool newGame { get; private set; }

        public GamePlayData()
        {
            newGame = true;
        }
    }
}