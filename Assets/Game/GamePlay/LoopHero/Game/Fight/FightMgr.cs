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

        public void Clear()
        {
           
        }

        public void StartFight(FightModuleData data)
        {
            
        }


#if UNITY_EDITOR
        
        public FightModuleData fightModuleData;

        [Obsolete]
        [Sirenix.OdinInspector.Button]
        internal void DebugFight()
        {
            GameMgr.Singleton.ToFight(fightModuleData);
        }
#endif
    }
}