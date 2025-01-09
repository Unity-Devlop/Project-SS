using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.LoopHero
{
    public class Pokemon : MonoBehaviour
    {
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public PokemonData data { get; private set; }

        public event Action<PokemonData> OnEnterBattle;
        public event Action OnAction;

        public event Action<PokemonData> OnExitBattle;

        public virtual async UniTask EnterBattle(PokemonData data)
        {
            Assert.IsNull(this.data);
            this.data = data;
            OnEnterBattle?.Invoke(data);
            await UniTask.CompletedTask;
        }

        public virtual async UniTask Action(Pokemon target)
        {
            Assert.IsNotNull(data);
            float localY = transform.localPosition.y;
            // TODO 占位攻击动作
            await transform.DOLocalMoveY(localY + 0.5f, 0.2f);
            await transform.DOLocalMoveY(localY, 0.2f);
            // 计算伤害
            int damage = FightMath.CalDamage(data, target.data);
            // 造成伤害
            await target.TakeDamage(damage);
            OnAction?.Invoke();
            await UniTask.CompletedTask;
        }

        private async UniTask TakeDamage(int damage)
        {
            await UniTask.CompletedTask;
            Assert.IsNotNull(data);
            data.currentHealth -= damage;
            data.Trigger();
            // TODO 实现伤害飘字
            WordsFloats.Float(transform.position, Vector2.up, damage.ToString(), 0.2f, Color.red);
        }

        public async UniTask ExitBattle()
        {
            Assert.IsNotNull(data);
            OnExitBattle?.Invoke(data);
            data = null;
            await UniTask.CompletedTask;
        }
    }
}