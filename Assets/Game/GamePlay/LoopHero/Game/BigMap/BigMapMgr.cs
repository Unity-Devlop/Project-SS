using System;
using Unity.Cinemachine;
using UnityToolkit;

namespace Game.LoopHero
{
    public class BigMapMgr : MonoSingleton<BigMapMgr>
    {
        private void OnEnable()
        {
            GetComponent<CinemachineCamera>().enabled = true;
        
        }

        private void OnDisable()
        {
            GetComponent<CinemachineCamera>().enabled = false;
        }
        private void Update()
        {
            
        }
    }
}