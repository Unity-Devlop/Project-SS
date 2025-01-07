using Unity.Cinemachine;
using UnityToolkit;

namespace Game.LoopHero
{
    public class CampMgr : LoopHeroModuleMgr<CampMgr>
    {
        private void OnEnable()
        {
            GetComponent<CinemachineCamera>().enabled = true;
        }

        private void OnDisable()
        {
            GetComponent<CinemachineCamera>().enabled = false;
        }

        private void Update()
        {
        }

        public override void OnUpdate()
        {
            
        }
    }
}