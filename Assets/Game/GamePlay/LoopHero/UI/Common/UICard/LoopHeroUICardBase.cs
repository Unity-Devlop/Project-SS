using cfg;
using TMPro;
using UnityEngine;
using UnityToolkit;

namespace Game.LoopHero.UI.Common
{
    public abstract class LoopHeroUICardBase : UICard, ILoopHeroCard
    {
        [SerializeField] private TextMeshProUGUI nameText;

        public virtual void Bind(ItemEnum id)
        {
            var config = Core.Tables.ItemTable.Get(id);
            nameText.text = config.Name;
        }
    }
}