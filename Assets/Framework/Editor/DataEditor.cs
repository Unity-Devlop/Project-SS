using System;
using System.IO;
using Game;
using Game.LoopHero;
using LiteDB;
using Newtonsoft.Json;
using Sirenix.OdinInspector;

namespace Framework.Editor
{
    [Serializable]
    internal class DataEditor
    {
        internal void OnEnable()
        {
            LoadData();
        }


        [Button("清空数据库"), HorizontalGroup("1")]
        private void ClearDatabase()
        {
            DataSystem.Shared.Dispose();
            // TODO 清空数据库
            if (File.Exists(DataSystem.dbPath))
            {
                File.Delete(DataSystem.dbPath);
            }

            LoadData();
        }


        [Button("加载数据库"), HorizontalGroup("1")]
        private void LoadData()
        {
            DataSystem.Shared.Dispose();
            DataSystem.Shared.OnInit();
            gamePlayData = DataSystem.Shared.GetOrDefault<GamePlayData>(Core.GameDataID);
        }

        [Button("保存游戏数据"), HorizontalGroup("1")]
        private void SaveData()
        {
            DataSystem.Shared.Save(Core.GameDataID, gamePlayData);
        }


        public GamePlayData gamePlayData;


        public void OnDestroy()
        {
            DataSystem.Shared.Dispose();
        }
    }
}