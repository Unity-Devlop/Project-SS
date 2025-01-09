using System;
using System.Buffers;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using UnityEngine.Serialization;

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

        public FightModuleData data { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public FightState fightState { get; private set; }

        private CinemachineCamera _camera;

        [SerializeField] private Fighter local;
        [SerializeField] private Fighter enemy;

        private BuffTicker _buffTicker;

        protected override void OnInit()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        protected override void OnDispose()
        {
        }

        public async UniTask StartFight(FightModuleData inputData, OnFightEnd onFightEnd)
        {
            Assert.IsFalse(inputData.self.teamData.trainerId == inputData.enemy.teamData.trainerId,
                $"[{nameof(FightMgr)}] 输入数据 双方trainerId不能相同");
            Assert.IsTrue(inputData.self.canFight, $"[{nameof(FightMgr)}] 输入数据 自己不能战斗");
            Assert.IsTrue(inputData.enemy.canFight, $"[{nameof(FightMgr)}] 输入数据 敌方不能战斗");

            Assert.IsTrue(inputData.enemy.teamData.Validate(), $"[{nameof(FightMgr)}] 输入数据 敌方数据不合法");
            Assert.IsTrue(inputData.self.teamData.Validate(), $"[{nameof(FightMgr)}] 输入数据 自己数据不合法");


            fightState = FightState.None;
            Assert.IsNull(this.data);
            this.data = inputData;
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
            this.data = null;
            fightState = FightState.None;
            onFightEnd(in result);
        }


        private async UniTask RoundStart()
        {
            var t1 = local.RoundStart();
            var t2 = enemy.RoundStart();

            await UniTask.WhenAll(t1, t2);
        }

        private async UniTask Rounding()
        {
            await UniTask.CompletedTask;

            using var enumerator = data.CreateBattlePokemonEnumerator();
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
                if (local.data.canFight == false || enemy.data.canFight == false)
                {
                    break;
                }

                list.Sort((a, b) => b.baseSpeed.CompareTo(a.baseSpeed));
                PokemonData actor = null;
                // 本质上是找到速度最快的没有行动过的宝可梦
                foreach (var pokemon in list)
                {
                    if (!set.Add(pokemon)) continue;
                    actor = pokemon;
                    count--;
                    break;
                }

                Assert.IsNotNull(actor);
                if (!actor.alive) continue; // 轮到我行动的时候我被打死了
                // 找到这个宝可梦对应的View 执行行动
                if (actor.trainerId == data.self.teamData.trainerId) // TODO 写一个函数把这个逻辑提取出来
                {
                    await RoundingPokemonAction(actor, local, enemy);
                }
                else if (actor.trainerId == data.enemy.teamData.trainerId)
                {
                    await RoundingPokemonAction(actor, enemy, local);
                }
                else
                {
                    throw new NotImplementedException($"[{nameof(FightMgr)}]:未知的trainerId:{actor.trainerId}");
                }
            }

            HashSetPool<PokemonData>.Release(set);
            ListPool<PokemonData>.Release(list);
        }

        private static async UniTask RoundingPokemonAction(PokemonData actor, Fighter attackFighter,
            Fighter defenseFighter)
        {
            Assert.IsFalse(actor.trainerId == defenseFighter.data.teamData.trainerId);
            var view = attackFighter.Query(actor);
            var targetData = FightMath.SearchTarget(actor, defenseFighter.data.teamData.GetBattlePokemonList());
            var defenseView = defenseFighter.Query(targetData);
            await view.Action(defenseView);
            if (!defenseView.data.alive) // actor的行动让target pokemon GG了
            {
                await defenseFighter.ExitTarget(defenseView.data);
            }
        }

        private async UniTask RoundEnd()
        {
            await UniTask.CompletedTask;
        }


        private async UniTask StartFight()
        {
            _camera.enabled = true;
            await local.Bind(data.self);
            await enemy.Bind(data.enemy);

            var t1 = local.FightStart();
            var t2 = enemy.FightStart();
            await UniTask.WhenAll(t1, t2);

            _buffTicker = new BuffTicker(data.CreateAllPokemonEnumerator(), data.CreateBuffEnumerator());
        }

        private async UniTask<FightResult> GetFightResult()
        {
            await UniTask.CompletedTask;

            bool selfAlive = data.self.canFight;
            bool enemyAlive = data.enemy.canFight;

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
            GameLogger.Log.Debug("[{this}] EndFight", this);
            _camera.enabled = false;
            var t1 = local.EndFight();
            var t2 = enemy.EndFight();
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
        // public Pokemon Query(PokemonData id)
        // {
        //     Fighter fighter;
        //     if (id.trainerId == data.self.teamData.trainerId)
        //     {
        //         fighter = self;
        //     }
        //     else if (id.trainerId == data.enemy.teamData.trainerId)
        //     {
        //         fighter = enemy;
        //     }
        //     else
        //     {
        //         throw new NotImplementedException($"[{nameof(FightMgr)}]:未知的trainerId:{id.trainerId}");
        //     }
        //
        //     return fighter.Query(id);
        // }
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
            so.data.enemy.teamData.RecoverHealth();
            so.data.self.teamData.RecoverHealth();
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