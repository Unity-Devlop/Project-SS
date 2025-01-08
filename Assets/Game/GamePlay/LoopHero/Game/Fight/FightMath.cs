using System;
using cfg;
using NUnit.Framework;

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
            Assert.IsFalse(defenser.config.Type == PokemonTypeEnum.玩家, $"玩家:{defenser}不可被攻击");
            if (attacker.config.Type == PokemonTypeEnum.玩家)
            {
                return Math.Clamp(attacker.finalPower - defenser.finalDefense, MinPowerDamage, MaxPowerDamage) +
                       attacker.finalSpeed;
            }

            return (int)(Math.Clamp(attacker.finalPower - defenser.finalDefense, MinPowerDamage, MaxPowerDamage) *
                         attacker.level / (float)defenser.finalDefense +
                         attacker.finalSpeed);
        }
    }
}