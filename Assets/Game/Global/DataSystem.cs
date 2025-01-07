using System.Collections.Generic;
using LiteDB;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class DataSystem : MonoBehaviour, ISystem, IOnInit
    {
        public LiteDatabase database { get; private set; }

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
            database = new LiteDatabase(dbPath);
        }


        public void Dispose()
        {
            // TODO 保存数据
            database.Dispose();
        }

        public T GetOrDefault<T>(int id) where T : new()
        {
            var collection = database.GetCollection<T>();
            var result = collection.FindById(id);
            if (result == null)
            {
                result = new T();
                GameLogger.Log($"[{nameof(DataSystem)}]:Create {id} {result}");
                collection.Insert(id, result);
            }
            else
            {
                GameLogger.Log($"[{nameof(DataSystem)}]:Get {id} {result}");
            }

            return result;
        }
        
        public void Save<T>(int id, T data)
        {
            GameLogger.Log($"[{nameof(DataSystem)}]:Save {id} {data}");
            var collection = database.GetCollection<T>();
            collection.Upsert(id, data);
        }
    }
}