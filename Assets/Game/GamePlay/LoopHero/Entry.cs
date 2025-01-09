using System;
using UnityEngine;
using UnityToolkit;

namespace Game.LoopHero
{
    [Serializable]
    public class Entry : MonoBehaviour, IGameEntry
    {
        [field: SerializeField] public bool initialized { get; private set; }

        public void OnInit()
        {
            UIRoot.Singleton.OpenPanel<VersionPanel>();
            UIRoot.Singleton.OpenPanel<DebugPanel>();
            GameLogger.Log.Debug("LoopHeroEntry OnInit");
            initialized = true;
            var core = new GameObject(nameof(Core));
            core.AddComponent<Core>();
        }

        private void OnDestroy()
        {
            initialized = false;
            GameLogger.Log.Debug("LoopHeroEntry OnDestroy");
        }
    }
}