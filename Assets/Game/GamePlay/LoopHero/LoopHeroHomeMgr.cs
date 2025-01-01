
using UnityToolkit;

namespace Game.LoopHero
{
    public class LoopHeroHomeMgr : MonoSingleton<LoopHeroHomeMgr>
    {
        // protected override bool DontDestroyOnLoad() => true;
        protected override void OnInit()
        {
            GameLogger.Log("LoopHeroHomeMgr OnInit");
            UIRoot.Singleton.OpenPanel<GameHomePanel>();
        }

        protected override void OnDispose()
        {
            GameLogger.Log("LoopHeroHomeMgr OnDispose");
            // Global.Get<AudioSystem>().DisposeBGM(FMODName.Event.BGM_game_home);
            if (UIRoot.SingletonNullable != null)
            {
                UIRoot.Singleton.Dispose<GameHomePanel>();
            }
        }
    }
}