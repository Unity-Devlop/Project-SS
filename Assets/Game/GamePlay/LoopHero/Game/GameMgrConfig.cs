using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.LoopHero
{
    [CreateAssetMenu(fileName = "GameMgrConfig", menuName = "Game/GameMgrConfig")]
    public class GameMgrConfig : ScriptableObject
    {
        
        [SerializeField] private AssetReferenceT<GameObject> playerPrefab;
        [SerializeField] private AssetReference bigMapScene;
        [SerializeField] private AssetReference campScene;
        [SerializeField] private AssetReference fightScene;

    }
}