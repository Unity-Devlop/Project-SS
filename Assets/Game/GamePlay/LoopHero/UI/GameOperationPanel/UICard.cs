using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityToolkit;

namespace Game.LoopHero.UI
{
    public class UICard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        // 需要锚定到的位置
        private RectTransform _pivotPoint;
        private RectTransform _transform;

        // Config
        [SerializeField] private Vector3 offset;
        [SerializeField] private float moveSpeedLimit = 20f;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public bool dragging { get; private set; }

        private bool _autoReset = true;

        private void Awake()
        {
            _pivotPoint = transform.parent.GetComponent<RectTransform>();
            Assert.IsNotNull(_pivotPoint);
            _transform = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _transform.anchoredPosition += eventData.delta;
        }

        private void Update()
        {
            if (dragging)
            {
                //     Vector3 mousePosition = Input.mousePosition;
                //     Vector2 targetPosition = UIRoot.Singleton.UICamera.ScreenToWorldPoint(mousePosition) - offset;
                //     Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                //     Vector2 velocity = direction * Mathf.Min(moveSpeedLimit,
                //         Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
                //     transform.Translate(velocity * Time.deltaTime);
                //     ClampPosition(); // 限制位置 不能超出屏幕
                return;
            }

            if (_autoReset)
            {
                _transform.anchoredPosition = Vector2.zero;
            }
        }
        //
        // private void ClampPosition()
        // {
        //     Vector2 screenBounds =
        //         UIRoot.Singleton.UICamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        //     Vector3 clampedPosition = transform.position;
        //     clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
        //     clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
        //     float z = transform.position.z;
        //     transform.position = new Vector3(clampedPosition.x, clampedPosition.y, z);
        // }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            dragging = false;
        }
    }
}