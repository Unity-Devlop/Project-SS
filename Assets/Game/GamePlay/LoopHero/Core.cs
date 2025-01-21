using System.Runtime.CompilerServices;
using cfg;
using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Game.LoopHero
{
    public class Core : MonoSingleton<Core>
    {
        #region Fast Access

        private static TypeEventSystem _event;

        /// <summary>
        /// 事件系统
        /// </summary>
        public static TypeEventSystem Event
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_event == null)
                {
                    _event = new TypeEventSystem();
                }

                return _event;
            }
        }

        /// <summary>
        /// 数据表
        /// </summary>
        public static Tables Tables
        {
            get
            {
                if (_tables == null)
                {
                    _tables = new Tables(TableLoad);
                }

                return _tables;
            }
        }

        private static Tables _tables;

        private static JSONNode TableLoad(string name)
        {
            var path = $"DataTable/{name}.json";
            TextAsset asset = Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion();
            return JSON.Parse(asset.text);
        }

        #endregion


        protected override bool DontDestroyOnLoad() => true;
        internal const int GameDataID = 1;
        public GamePlayData playData { get; private set; }

        protected override void OnInit()
        {
            _event = new TypeEventSystem();
            _tables = new Tables(TableLoad);
            ItemCardEffects.GenerateAll();
            playData = Global.Get<DataSystem>().GetOrDefault<GamePlayData>(GameDataID);
            if (playData.newGame)
            {
                Global.Get<DataSystem>().Save(GameDataID, playData);
            }
        }

        protected override void OnDispose()
        {
            _tables = null;
        }
    }
}