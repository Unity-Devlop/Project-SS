using cfg;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityToolkit;

namespace Game.LoopHero.UI.Common
{
    public abstract class LoopHeroUICardBase : UICard, ILoopHeroCard
    {
        [SerializeField] private TextMeshProUGUI nameText;


        public int idx { get; private set; }

        public virtual void Bind(ItemEnum id, int idx)
        {
            this.idx = idx;
            var config = Core.Tables.ItemTable.Get(id);
            nameText.text = config.Name;
        }
    }
}