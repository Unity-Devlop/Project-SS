using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityToolkit;
namespace Game.LoopHero
{
    public class DebugPanel : UIPanel
    {
        public Button dragButton;
        
        [SerializeField] private CanvasGroup _contentCanvasGroup;
        public override void OnLoaded()
        {
            base.OnLoaded();
            _contentCanvasGroup.alpha = 0;
            _contentCanvasGroup.blocksRaycasts = false;
            _contentCanvasGroup.interactable = false;
            dragButton.onClick.AddListener(() =>
            {
                _contentCanvasGroup.blocksRaycasts = !_contentCanvasGroup.blocksRaycasts;
                _contentCanvasGroup.interactable = !_contentCanvasGroup.interactable;
                _contentCanvasGroup.alpha = _contentCanvasGroup.alpha > 0 ? 0 : 1;
            });
        }
    }
}