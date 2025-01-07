using System;
using UnityToolkit;

namespace Game.LoopHero
{
    public abstract class LoopHeroModuleMgr<T> : MonoSingleton<T> where T : LoopHeroModuleMgr<T>
    {
        public abstract void OnUpdate();
    }
}