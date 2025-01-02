using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.LoopHero
{
    [CreateAssetMenu(fileName = "GameMgrConfig", menuName = "Game/GameMgrConfig")]
    public class GameMgrConfig : ScriptableObject
    {
        
        [SerializeField] public AssetReferenceT<GameObject> playerPrefab;
        [SerializeField] public AssetReference bigMapScene;
        [SerializeField] public AssetReference campScene;
        [SerializeField] public AssetReference fightScene;

    }
}