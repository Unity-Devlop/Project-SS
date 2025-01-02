using System;
using UnityEngine;
using UnityToolkit;

namespace Game.LoopHero
{
    [Serializable]
    public class LoopHeroEntry : MonoBehaviour, IGameEntry
    {
        [field: SerializeField] public bool initialized { get; private set; }

        public void OnInit()
        {
            UIRoot.Singleton.OpenPanel<VersionPanel>();
            UIRoot.Singleton.OpenPanel<DebugPanel>();
            GameLogger.Log("LoopHeroEntry OnInit");
            initialized = true;
            var core = new GameObject(nameof(LoopHeroCore));
            core.AddComponent<LoopHeroCore>();
        }

        private void OnDestroy()
        {
            initialized = false;
            GameLogger.Log("LoopHeroEntry OnDestroy");
        }
    }
}