using System.Collections.Generic;
using LiteDB;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class DataSystem : MonoBehaviour, ISystem, IOnInit
    {
        private LiteDatabase _database;

        internal static string dbPath
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath + "/Editor Default Resources/data.db";
#else
                return Application.persistentDataPath + "/data.db";
#endif
            }
        }

        public void OnInit()
        {
            GameLogger.Log("DataSystem OnInit");
            _database = new LiteDatabase(dbPath);
        }


        public void Dispose()
        {
            // TODO 保存数据
            _database.Dispose();
        }

        public T Query<T>(string collectionName, int id, T defaultValue)
        {
            var collection = _database.GetCollection<T>(collectionName);

            if (typeof(T).IsAssignableFrom(typeof(IIndexable)))
            {
                collection.EnsureIndex(x => ((IIndexable)x).index);
            }

            var result = collection.FindById(id);

            if (result == null)
            {
                collection.Insert(defaultValue);
                return defaultValue;
            }

            return result;
        }
    }
}