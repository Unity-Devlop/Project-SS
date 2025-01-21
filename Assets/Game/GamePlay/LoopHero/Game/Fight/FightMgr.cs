using System;
using System.Buffers;
using System.Collections.Generic;
using cfg;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityToolkit;

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

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public FightModuleData data { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public FightState fightState { get; private set; }

        private CinemachineCamera _camera;

        [SerializeField] private Fighter local;
        [SerializeField] private Fighter enemy;

        // private BuffTicker _buffTicker;

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
                // 4️死的都要退场

                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);

                GameLogger.Log.Information("[{this}] RoundStart", this);
                await RoundStart();
                result = await GetFightResult();
                if (result.isFightEnd) break;
                fightState = FightState.RoundStart;

                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
                // await CandidateReplaceDead();
                GameLogger.Log.Information("[{this}] Rounding", this);
                await Rounding();
                result = await GetFightResult();
                if (result.isFightEnd) break;
                fightState = FightState.Rounding;

                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
                // await CandidateReplaceDead();
                GameLogger.Log.Information("[{this}] RoundEnd", this);
                await RoundEnd();

                // 死了的都应该退场
                Assert.IsTrue(local.CheckDeadPokemon() && enemy.CheckDeadPokemon());

                // 如果玩家操作了候补区域 则应该都替换到战斗区域
                var t1 = TryEnterCandidate(local);
                var t2 = TryEnterCandidate(enemy);
                await UniTask.WhenAll(t1, t2);

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

        private async UniTask TryEnterCandidate(Fighter fighter)
        {
            var teamData = fighter.data.teamData;
            Assert.IsTrue(teamData.battlePokemonList.Count <= TeamData.MaxBattlePokemonCount,
                $"[{nameof(FightMgr)}] 战斗队伍数量超过上限");
            Assert.IsTrue(teamData.candidatePokemonQueue.Count <= TeamData.MaxBattlePokemonCount,
                $"[{nameof(FightMgr)}] 候补队伍数量超过上限");

            if (teamData.candidatePokemonQueue.Count == 0) return;

            List<int> emptyIndex = ListPool<int>.Get();
            List<int> deadIndex = ListPool<int>.Get();
            for (int i = 0; i < TeamData.MaxBattlePokemonCount; i++) // 找到可以候补的位置
            {
                if (i >= teamData.battlePokemonList.Count) // 一个没有使用的位置
                {
                    Assert.IsTrue(fighter.CheckPositionEmpty(i), $"[{fighter}] 位置{i}不为空");
                    emptyIndex.Add(i);
                    continue;
                }

                if (!teamData.battlePokemonList[i].alive) // 一个被死亡宝可梦占据的位置
                {
                    deadIndex.Add(i);
                    continue;
                }
            }

            // 有候补的位置&有候补的宝可梦
            GameLogger.Log.Debug(
                "[{fighter}] 候补队伍数量:{teamData.candidatePokemonQueue.Count} 空闲位置:{candidateIndex} 死亡位置:{deadIndex}",
                fighter,
                teamData.candidatePokemonQueue.Count,
                JsonConvert.SerializeObject(emptyIndex),
                JsonConvert.SerializeObject(deadIndex));
            // 先替换死的
            while (deadIndex.Count > 0 && teamData.candidatePokemonQueue.Count > 0)
            {
                int index = deadIndex[deadIndex.Count - 1];
                deadIndex.RemoveAt(deadIndex.Count - 1);
                var candidate = teamData.candidatePokemonQueue.Dequeue();

                Assert.IsNotNull(candidate);
                Assert.IsFalse(teamData.battlePokemonList[index].alive);
                GameLogger.Log.Debug("[{fighter}] 候补宝可梦:{candidate} 替换到位置:{index} {selfTeam.battlePokemonList[index]}",
                    fighter.gameObject.name,
                    candidate.id.ToString(), index, teamData.battlePokemonList[index].id.ToString());
                teamData.battlePokemonList[index] = candidate;
                await fighter.EnterBattle(candidate, index);
            }

            // 再替换空的
            while (emptyIndex.Count > 0 && teamData.candidatePokemonQueue.Count > 0)
            {
                int index = emptyIndex[emptyIndex.Count - 1];
                emptyIndex.RemoveAt(emptyIndex.Count - 1);
                var candidate = teamData.candidatePokemonQueue.Dequeue();

                Assert.IsNotNull(candidate);
                GameLogger.Log.Debug("[{fighter}] 候补宝可梦 添加到位置:{index} 剩余空闲位置宝可梦:{emptyIndex}", fighter.gameObject.name,
                    index, JsonConvert.SerializeObject(emptyIndex));


                teamData.battlePokemonList.Add(candidate);
                await fighter.EnterBattle(candidate, index);
            }

            // while (candidateIndex.Count > 0 && teamData.candidatePokemonQueue.Count > 0)
            // {
            //     int index = candidateIndex[candidateIndex.Count - 1];
            //     candidateIndex.RemoveAt(candidateIndex.Count - 1);
            //     var candidate = teamData.candidatePokemonQueue.Dequeue();
            //
            //     Assert.IsNotNull(candidate);
            //     if (index < teamData.battlePokemonList.Count)
            //     {
            //         // 这里是替换死掉的宝可梦
            //         Assert.IsFalse(teamData.battlePokemonList[index].alive);
            //         GameLogger.Log.Debug("[{fighter}] 候补宝可梦:{candidate} 替换到位置:{index} {selfTeam.battlePokemonList[index]}",
            //             fighter.gameObject.name,
            //             candidate, index, teamData.battlePokemonList[index]);
            //         teamData.battlePokemonList[index] = candidate;
            //         await fighter.EnterBattle(candidate, index);
            //         continue;
            //     }
            //
            //     // 这里是单纯的新出场
            //     GameLogger.Log.Debug("[{fighter}] 候补宝可梦 添加到位置:{index}", fighter.gameObject.name, index);
            //     teamData.battlePokemonList.Add(candidate);
            //     await fighter.EnterBattle(candidate, index);
            // }


            ListPool<int>.Release(emptyIndex);
            ListPool<int>.Release(deadIndex);
            await UniTask.CompletedTask;
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
            // TODO 回合结束 准备下一回合
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

            // _buffTicker = new BuffTicker(data.CreateAllPokemonEnumerator(), data.CreateBuffEnumerator());
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
            data = null;
            var t1 = local.EndFight();
            var t2 = enemy.EndFight();
            await UniTask.WhenAll(t1, t2);
            _camera.enabled = false;
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
                // _buffTicker.OnUpdate(Time.deltaTime);
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
            so.data.enemy.teamData.battlePokemonList.RemoveAll(pokemonData => pokemonData.id == PokemonEnum.烈火领主);
            so.data.self.teamData.RecoverHealth();
            so.data.self.teamData.battlePokemonList.RemoveAll(pokemonData => pokemonData.id == PokemonEnum.烈火领主);
            GameMgr.Singleton.ToFight(so.data);
        }

        // private void OnGUI()
        // {
        //     if (_data == null) return;
        //     // show fightState
        //     GUI.Label(new Rect(10, 10, 100, 20), fightState.ToString());
        // }
    }
#endif
}