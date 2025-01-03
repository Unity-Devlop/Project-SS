using System;
using System.IO;
using Game;
using Game.LoopHero;
using LiteDB;
using Sirenix.OdinInspector;

namespace Framework.Editor
{
    [Serializable]
    internal class DataEditor
    {
        private LiteDatabase _database;

        public DataEditor()
        {
            _database = new LiteDatabase(DataSystem.dbPath);
            gamePlayData = GetOrDefault<GamePlayData>(Core.GameDataID);
        }

        ~DataEditor()
        {
            if (_database != null) _database.Dispose();
        }

        [Sirenix.OdinInspector.ShowInInspector, HorizontalGroup("1")]
        public bool loaded => _database != null;

        [Button("清空数据库"), HorizontalGroup("1")]
        private void ClearDatabase()
        {
            if (_database != null) _database.Dispose();
            // TODO 清空数据库
            if (File.Exists(DataSystem.dbPath))
            {
                File.Delete(DataSystem.dbPath);
            }

            LoadData();
        }


        // [Button("加载数据库"), HorizontalGroup("1")]
        private void LoadData()
        {
            if (_database != null) _database.Dispose();
            _database = new LiteDatabase(DataSystem.dbPath);
            gamePlayData = GetOrDefault<GamePlayData>(Core.GameDataID);
        }

        [Button("保存"), HorizontalGroup("1")]
        private void SaveData()
        {
            if (gamePlayData != null)
            {
                var collection = _database.GetCollection<GamePlayData>();
                collection.Update(Core.GameDataID, gamePlayData);
            }
        }

        [ShowIf("loaded"), Sirenix.OdinInspector.PropertyOrder(2)]
        public GamePlayData gamePlayData;

        private T GetOrDefault<T>(int id) where T : new()
        {
            var collection = _database.GetCollection<T>();
            var result = collection.FindById(id);
            if (result == null)
            {
                result = new T();
                collection.Insert(id, result);
            }

            return result;
        }

        public void OnDestroy()
        {
            if (_database != null) _database.Dispose();
        }
    }
}