#if UNITY_EDITOR

using System;
using UnityEngine;

namespace Game.LoopHero.Debug
{
    [CreateAssetMenu(menuName = "Game/FightDebugSO")]
    public class FightDebugSO : ScriptableObject
    {
        public FightModuleData data;


        [Sirenix.OdinInspector.Button]
        private void Random()
        {
            throw new NotImplementedException();
        }
    }
}
#endif