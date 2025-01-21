using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

#if UNITY_EDITOR

namespace Game.LoopHero
{
    public class PokemonDebugger : MonoBehaviour
    {
        [SerializeField] private TextMeshPro healthText;
        private Pokemon _pokemon;

        private void Awake()
        {
            _pokemon = transform.parent.GetComponent<Pokemon>();
            _pokemon.OnEnterBattle += OnEnterBattle;
            _pokemon.OnExitBattle += OnExitBattle;
            // 文字必须要保持scale.x =1
            float lossyScaleX = transform.lossyScale.x;
            if (lossyScaleX < 0)
            {
                var localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;
            }
        }

        private void OnExitBattle(PokemonData obj)
        {
            obj.Unregister(OnChange);
        }

        private void OnChange(PokemonData obj)
        {
            healthText.text = $"{obj.currentHealth}/{obj.baseHealth}";
        }

        private void OnEnterBattle(PokemonData obj)
        {
            obj.Register(OnChange);
            OnChange(obj);
        }

        private void OnDestroy()
        {
            _pokemon.OnEnterBattle -= OnEnterBattle;
            _pokemon.OnExitBattle -= OnExitBattle;
        }
    }
}
#endif