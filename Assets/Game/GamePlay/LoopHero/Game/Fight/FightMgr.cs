using System;
using System.Buffers;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace Game.LoopHero
{
    public enum FightState
    {
        None,
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

        private BuffTicker _buffTicker;

        protected override void OnInit()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        protected override void OnDispose()
        {
        }


        public async UniTask StartFight(FightModuleData data, OnFightEnd onFightEnd)
        {
            fightState = FightState.None;
            Assert.IsNull(_data);
            _data = data;
            await StartFight();
            fightState = FightState.FightStart;
            var result = new FightResult(false, false);
            while (result.isFightEnd == false &&
                   destroyCancellationToken.IsCancellationRequested == false &&
                   fightState != FightState.None
                  )
            {
                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
                await RoundStart();
                result = await GetFightResult();
                if (result.isFightEnd) break;
                fightState = FightState.RoundStart;

                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
                await Rounding();
                result = await GetFightResult();
                if (result.isFightEnd) break;
                fightState = FightState.Rounding;

                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
                await RoundEnd();
                result = await GetFightResult();
                if (result.isFightEnd) break;
                fightState = FightState.RoundEnd;
            }

            await EndFight();
            fightState = FightState.EndFight;
            onFightEnd(in result);
            _data = null;
            fightState = FightState.None;
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

            using var enumerator = _data.CreateBattlePokemonEnumerator();
            var list = ListPool<PokemonData>.Get();
            while (enumerator.MoveNext())
            {
                var pokemon = enumerator.Current;
#if UNITY_EDITOR
                Assert.IsNotNull(pokemon);
                Assert.IsFalse(list.Contains(pokemon));
#endif
                list.Add(pokemon);
            }

            var set = HashSetPool<PokemonData>.Get();
            // TODO 考虑 A的行动影响B的速度 从而导致需要重新排序的情况
            int count = list.Count;
            while (count > 0)
            {
                list.Sort((a, b) => b.baseSpeed.CompareTo(a.baseSpeed));
                PokemonData action = null;
                // 本质上是找到速度最快的没有行动过的宝可梦
                foreach (var pokemon in list)
                {
                    if (!set.Add(pokemon)) continue;
                    action = pokemon;
                    count--;
                    break;
                }

                Assert.IsNotNull(action);
                // 找到这个宝可梦对应的View 执行行动
                if (action.trainerId == _data.self.teamData.trainerId)
                {
                    await self.Action(action);
                }
                else if (action.trainerId == _data.enemy.teamData.trainerId)
                {
                    await enemy.Action(action);
                }
                else
                {
                    throw new NotImplementedException($"[{nameof(FightMgr)}]:未知的trainerId:{action.trainerId}");
                }
            }

            HashSetPool<PokemonData>.Release(set);
            ListPool<PokemonData>.Release(list);
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

            _buffTicker = new BuffTicker(_data.CreateAllPokemonEnumerator(), _data.CreateBuffEnumerator());
        }

        private async UniTask<FightResult> GetFightResult()
        {
            await UniTask.CompletedTask;

            bool selfAlive = _data.self.canFight;
            bool enemyAlive = _data.enemy.canFight;

            if (!selfAlive && !enemyAlive)
            {
                throw new NotImplementedException($"[{nameof(FightMgr)}]:双方都死了");
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
            var t1 =  self.EndFight();
            var t2 =  enemy.EndFight();
            await UniTask.WhenAll(t1, t2);
            await UniTask.CompletedTask;
        }

        public void ExitFight()
        {
            fightState = FightState.None;
        }

        public override void OnUpdate()
        {
            if (fightState != FightState.None)
            {
                _buffTicker.OnUpdate(Time.deltaTime);
            }
        }
    }
#if UNITY_EDITOR
    public partial class FightMgr
    {
        [SerializeField, Sirenix.OdinInspector.LabelText("DEBUG")]
        private LoopHero.Debug.FightDebugSO so;

        [Obsolete]
        [Sirenix.OdinInspector.Button]
        internal void DebugFight()
        {
            GameMgr.Singleton.ToFight(so.data);
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