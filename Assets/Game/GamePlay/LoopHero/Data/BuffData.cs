using cfg;

namespace Game.LoopHero
{
    public sealed class BuffData
    {
        public BuffEnum id;
        public BuffConfig config => Core.Tables.BuffTable.Get(id);
        public int[] targetsForSelf; // 自己目标
        public int[] targetsForEnemy; // 敌人目标
        public float leftTime; // 剩余时间
    }
}