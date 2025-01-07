using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Game.LoopHero
{
    public enum FightState
    {
        FightStart,
        RoundStart,
        Rounding,
        RoundEnd,
        EndFight
    }

    public sealed partial class FightMgr : LoopHeroModuleMgr<FightMgr>
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

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public FightState fightState { get; private set; }

        private CinemachineCamera _camera;

        [SerializeField] private Fighter self;
        [SerializeField] private Fighter enemy;


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
            fightState = FightState.FightStart;
            var result = new FightResult(false, false);
            while (result.isFightEnd == false)
            {
                await UniTask.DelayFrame(1);
                await RoundStart();
                result = await GetFightResult();
                if (result.isFightEnd) break;
                fightState = FightState.RoundStart;

                await UniTask.DelayFrame(1);
                await Rounding();
                result = await GetFightResult();
                if (result.isFightEnd) break;
                fightState = FightState.Rounding;


                await UniTask.DelayFrame(1);
                await RoundEnd();
                result = await GetFightResult();
                if (result.isFightEnd) break;
                fightState = FightState.RoundEnd;
            }

            await EndFight();
            fightState = FightState.EndFight;
            onFightEnd(in result);
            _data = null;
        }


        private async UniTask RoundStart()
        {
            var t1 = self.RoundStart();
            var t2 = enemy.RoundStart();

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

            var t1 = self.FightStart();
            var t2 = enemy.FightStart();
            await UniTask.WhenAll(t1, t2);
        }

        private async UniTask<FightResult> GetFightResult()
        {
            await UniTask.CompletedTask;

            bool selfAlive = _data.self.canFight;
            bool enemyAlive = _data.enemy.canFight;

            if (!selfAlive && !enemyAlive)
            {
                throw new NotImplementedException($@"[{nameof(FightMgr)}]:双方都死了");
            }

            if (selfAlive && !enemyAlive)
            {
                return new FightResult(true, true);
            }

            if (!selfAlive && enemyAlive)
            {
                return new FightResult(false, true);
            }

            // 双方都活着
            return new FightResult(false, false);
        }

        private async UniTask EndFight()
        {
            _camera.enabled = false;
            await UniTask.CompletedTask;
        }

        public void DisableLogic()
        {
        }

        public override void OnUpdate()
        {
            
        }
    }
#if UNITY_EDITOR
    public partial class FightMgr
    {
        [SerializeField, Sirenix.OdinInspector.LabelText("DEBUG")]
        private FightModuleData fightModuleData;

        [Obsolete]
        [Sirenix.OdinInspector.Button]
        internal void DebugFight()
        {
            GameMgr.Singleton.ToFight(fightModuleData);
        }
        //
        // private void OnGUI()
        // {
        //     if (_data == null) return;
        //     // show fightState
        //     GUI.Label(new Rect(10, 10, 100, 20), fightState.ToString());
        // }
    }
#endif
}