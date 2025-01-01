using UnityEngine;

namespace Game.LoopHero
{
    public class LoopHeroEntry : MonoBehaviour,IGameEntry
    {
        public bool initialized { get; private set; }
        public void OnInit()
        {
            initialized = true;
        }
    }
}