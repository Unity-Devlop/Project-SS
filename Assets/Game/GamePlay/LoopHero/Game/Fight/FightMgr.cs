using System;
using Unity.Cinemachine;
using UnityToolkit;

namespace Game.LoopHero
{
    public class FightMgr : MonoSingleton<FightMgr>
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