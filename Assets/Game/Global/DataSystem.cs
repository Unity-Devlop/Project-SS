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
        }
    }
}