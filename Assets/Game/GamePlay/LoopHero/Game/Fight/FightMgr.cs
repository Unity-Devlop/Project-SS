using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Game.LoopHero
{
    public sealed partial class FightMgr : MonoSingleton<FightMgr>
    {
        #region Define

        public delegate void OnFightEnd(in FightResult result);

        public struct FightResult
        {
            public bool isSelfWin;
            public bool isFightEnd;

            public FightResult(bool isSelfWin, bool isFightEnd)
            {
                this.isSelfWin = isSelfWin;
                this.isFightEnd = isFightEnd;
            }
        }

        #endregion
        
        private FightModuleData _data;

        private CinemachineCamera _camera;

        [SerializeField]
        private Fighter self;
        [SerializeField]
        private Fighter enemy;


        protected override void OnInit()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        protected override void OnDispose()
        {
        }

        
        
        public async void /*UniTask*/ StartFight(FightModuleData data, OnFightEnd onFightEnd)
        {
            Assert.IsNull(_data);
            _data = data;
            await StartFight();
            var result = new FightResult(false, false);
            while (result.isFightEnd == false)
            {
                await UniTask.DelayFrame(1);
                await RoundStart();
                await UniTask.DelayFrame(1);
                await Rounding();
                await UniTask.DelayFrame(1);
                await RoundEnd();
                result = await GetFightResult();
            }

            await EndFight();
            onFightEnd(in result);
            _data = null;
        }


        private async UniTask RoundStart()
        {
            var t1 = self.RoundStart();
            var t2 =  enemy.RoundStart();
            await UniTask.WhenAll(t1, t2);
        }

        private async UniTask Rounding()
        {
            await UniTask.CompletedTask;
        }

        private async UniTask RoundEnd()
        {
            await UniTask.CompletedTask;
        }


        private async UniTask StartFight()
        {
            _camera.enabled = true;
            await self.Bind(_data.self);
            await enemy.Bind(_data.enemy);
            await UniTask.CompletedTask;
        }

        private async UniTask<FightResult> GetFightResult()
        {
            await UniTask.CompletedTask;
            return new FightResult()
            {
                isFightEnd = true,
                isSelfWin = true
            };
        }

        private async UniTask EndFight()
        {
            _camera.enabled = false;
            await UniTask.CompletedTask;
        }

        public void DisableLogic()
        {
        }
    }
#if UNITY_EDITOR
    public partial class FightMgr
    {
        [SerializeField,Sirenix.OdinInspector.LabelText("DEBUG")]
        private FightModuleData fightModuleData;

        [Obsolete]
        [Sirenix.OdinInspector.Button]
        internal void DebugFight()
        {
            GameMgr.Singleton.ToFight(fightModuleData);
        }
    }
#endif
}