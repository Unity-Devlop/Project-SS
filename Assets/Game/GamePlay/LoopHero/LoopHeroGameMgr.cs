using UnityEngine;
using UnityToolkit;

namespace Game.LoopHero
{
    public class LoopHeroGameMgr  : MonoSingleton<LoopHeroGameMgr>
    {
        // protected override bool DontDestroyOnLoad() => true;

        protected override void OnInit()
        {
            GameLogger.Log("LoopHeroGameMgr OnInit");
            UIRoot.Singleton.OpenPanel<GamePlayPanel>();
        }

        protected override void OnDispose()
        {
            GameLogger.Log("LoopHeroGameMgr OnDispose");

            if(UIRoot.SingletonNullable != null)
            {
                UIRoot.Singleton.Dispose<GamePlayPanel>();
            }
        }
    }
}