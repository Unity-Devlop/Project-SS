using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.LoopHero.UI
{
#if UNITY_EDITOR
    [ExecuteAlways]
#endif
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class UICarContainer : MonoBehaviour
    {
        private List<UICard> _cards;

        [SerializeField] private bool autoSizing = true;

        [SerializeField] private float standardWidth = 100;

        [SerializeField] public float standardHeight = 150;
        [SerializeField] public int standardCount = 8;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

        }

        private void Start()
        {
            _cards = new List<UICard>(GetComponentsInChildren<UICard>());
            foreach (var card in _cards)
            {
                card.pivotPoint.SetParent(transform);
            }
        }

        private void Update()
        {
            // 根据手牌的数量 动态调整自己的大小
            if (autoSizing)
            {
                float maxWidth = standardWidth * standardCount;
                float width = standardWidth * _cards.Count;
                width = Mathf.Min(width, maxWidth);
                _rectTransform.sizeDelta = new Vector2(width, standardHeight);
            }
        }

        public void Add(UICard card)
        {
            card.pivotPoint.SetParent(transform);
            _cards.Add(card);
        }
// #if UNITY_EDITOR
//
//         private void OnValidate()
//         {
//             _rectTransform = GetComponent<RectTransform>();
//             float targetStandardWidth = _rectTransform.sizeDelta.x / standardCount;
//             standardWidth = targetStandardWidth;
//             _rectTransform.sizeDelta = new Vector2(targetStandardWidth * standardCount, standardHeight);
//         }
//
// #endif
        //
        // public void Remove(UICard card)
        // {
        //     card.pivotPoint.SetParent(null);
        //     _cards.Remove(card);
        // }
#if UNITY_EDITOR
        private void OnValidate()
        {
            var rectTransform = GetComponent<RectTransform>();
            var pivot = rectTransform.pivot;
            pivot.x = 0;
            rectTransform.pivot = pivot;
        }  
#endif
    }
}