using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class DataSystem : MonoBehaviour, ISystem, IOnInit
    {
        
        public void OnInit()
        {
            GameLogger.Log("DataSystem OnInit");
        }
        

        public void Dispose()
        {
            // TODO 保存数据
        }

        public T Load<T>(string token) where T : new()
        {
            // TODO 加载数据
            return new T();
        }
    }
}