using IngameDebugConsole;

namespace Game.LoopHero
{
    // TODO 加入Link.xml 避免IL2CPP编译时被优化掉
    /// <summary>
    /// 配合InGameDebugConsole使用的命令行
    /// </summary>
    public static class CommandLine
    {
        [ConsoleMethod( "to_camp", "进入主城" )]
        public static void ToCamp()
        {
            GameMgr.Singleton.ToCamp();
        }

        [ConsoleMethod( "to_fight", "进入战斗" )]
        public static void ToFight()
        {
            GameMgr.Singleton.ToFight();
        }
        
        
        [ConsoleMethod( "to_bigmap", "进入大地图" )]
        public static void ToBigMap()
        {
            GameMgr.Singleton.ToBigMap();
        }
    }
}