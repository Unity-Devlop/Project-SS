using System;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityToolkit;

namespace Game.LoopHero
{
    public sealed partial class FightMgr : MonoSingleton<FightMgr>
    {
        private FightModuleData _data;

        public delegate void OnFightEnd(bool isSelfWin);

        public struct FightCheckResult
        {
            public bool isSelfWin;
            public bool isFightEnd;

            public FightCheckResult(bool isSelfWin, bool isFightEnd)
            {
                this.isSelfWin = isSelfWin;
                this.isFightEnd = isFightEnd;
            }
        }


        public async void /*UniTask*/ StartFight(FightModuleData data, OnFightEnd onFightEnd)
        {
            GetComponent<CinemachineCamera>().enabled = true;
            await FightStart();
            var check = new FightCheckResult(false, false);
            while (check.isFightEnd == false)
            {
                await UniTask.DelayFrame(1);
                await DoAutoFight();
                check = await CheckFightEnd();
            }

            await EndFight();
            onFightEnd(check.isSelfWin);
        }


        private async UniTask FightStart()
        {
            await UniTask.CompletedTask;
        }

        private async UniTask DoAutoFight()
        {
            await UniTask.CompletedTask;
        }

        private async UniTask<FightCheckResult> CheckFightEnd()
        {
            await UniTask.CompletedTask;
            return new FightCheckResult()
            {
                isFightEnd = true,
                isSelfWin = true
            };
        }

        private async UniTask EndFight()
        {
            await UniTask.CompletedTask;
        }

        public void DisableLogic()
        {
            GetComponent<CinemachineCamera>().enabled = false;
        }
    }
#if UNITY_EDITOR
    public partial class FightMgr
    {
        public FightModuleData fightModuleData;

        [Obsolete]
        [Sirenix.OdinInspector.Button]
        internal void DebugFight()
        {
            GameMgr.Singleton.ToFight(fightModuleData);
        }
    }
#endif
}