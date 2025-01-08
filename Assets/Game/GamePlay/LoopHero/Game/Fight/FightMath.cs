using System;
using System.Collections;
using System.Collections.Generic;
using cfg;
using NUnit.Framework;
using UnityToolkit;

namespace Game.LoopHero
{
    internal static class FightMath
    {
        /// <summary>
        /// power - defense 至少造成5点伤害
        /// </summary>
        private const int MinPowerDamage = 5;

        private const int MaxPowerDamage = 9999;

        public static int CalDamage(PokemonData attacker, PokemonData defenser)
        {
            Assert.IsFalse(defenser.config.Type == PokemonTypeEnum.玩家,
                $"{nameof(FightMath)}.{nameof(CalDamage)} {defenser}不可被攻击");
            if (attacker.config.Type == PokemonTypeEnum.玩家)
            {
                return Math.Clamp(attacker.finalPower - defenser.finalDefense, MinPowerDamage, MaxPowerDamage) +
                       attacker.finalSpeed;
            }

            return (int)(Math.Clamp(attacker.finalPower - defenser.finalDefense, MinPowerDamage, MaxPowerDamage) *
                         attacker.level / (float)defenser.finalDefense +
                         attacker.finalSpeed);
        }


        public static PokemonData SearchTarget(PokemonData actor, IEnumerable<PokemonData> targets)
        {
            List<PokemonData> list = UnityEngine.Pool.ListPool<PokemonData>.Get();
            foreach (var candidate in targets)
            {
                UnityEngine.Assertions.Assert.IsTrue(candidate.alive);
                if (candidate.config.Type == PokemonTypeEnum.玩家) continue;
                list.Add(candidate);
            }

            Assert.IsTrue(list.Count > 0, $"{nameof(FightMath)}.{nameof(SearchTarget)} 没有合适的目标, actor:{actor}");
            var target = list.RandomTakeWithoutRemove();
            UnityEngine.Pool.ListPool<PokemonData>.Release(list);
            return target;
        }
    }
}