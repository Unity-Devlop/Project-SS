using cfg;
using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Game.LoopHero
{
    public class LoopHeroCore : MonoSingleton<LoopHeroCore>
    {
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
        protected override bool DontDestroyOnLoad() => true;
        protected override void OnInit()
        {
            _tables = new Tables(TableLoad);
        }  

        protected override void OnDispose()
        {
            _tables = null;
        }
    }
}