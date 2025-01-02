using UnityEngine;

namespace Game.LoopHero
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerData data { get; private set; }
    }
}