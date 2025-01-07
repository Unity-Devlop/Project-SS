using UnityToolkit;

namespace Game.LoopHero
{
    public class BuffTicker : IOnUpdate
    {
        private TeamData _localPlayer;

        public BuffTicker(TeamData localPlayer)
        {
            _localPlayer = localPlayer;
        }

        public void OnUpdate(float deltaTime)
        {
        }
    }
}