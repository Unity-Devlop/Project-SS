
using UnityToolkit;

namespace Game.LoopHero
{
    public class HomeMgr : MonoSingleton<HomeMgr>
    {
        // protected override bool DontDestroyOnLoad() => true;
        protected override void OnInit()
        {
            GameLogger.Log.Debug("LoopHeroHomeMgr OnInit");
            UIRoot.Singleton.OpenPanel<GameHomePanel>();
        }

        protected override void OnDispose()
        {
            GameLogger.Log.Debug("LoopHeroHomeMgr OnDispose");
            // Global.Get<AudioSystem>().DisposeBGM(FMODName.Event.BGM_game_home);
            if (UIRoot.SingletonNullable != null)
            {
                UIRoot.Singleton.Dispose<GameHomePanel>();
            }
        }
    }
}