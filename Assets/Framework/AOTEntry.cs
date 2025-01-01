using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Framework
{
    public class AOTEntry : MonoBehaviour
    {
        public AOTEntryConfig config;

        private void Awake()
        {
            HotUpdate();
            Addressables.LoadSceneAsync(config.gameEntryReference);
        }

        private void HotUpdate()
        {
            
        }
    }
    
}