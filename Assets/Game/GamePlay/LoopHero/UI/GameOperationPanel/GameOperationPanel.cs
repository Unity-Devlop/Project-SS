using UnityEngine;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Game.LoopHero
{
    public class GameOperationPanel : UIPanel
    {
        [SerializeField] private UICarContainer _carContainer;
        private TeamData _bindData;
        public void Bind(TeamData data)
        {
            Assert.IsNull(_bindData);
            this._bindData = data;
        }
    }
}