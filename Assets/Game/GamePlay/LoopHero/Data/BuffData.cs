using System;
using cfg;

namespace Game.LoopHero
{
    [Serializable]
    public sealed class BuffData
    {
        public BuffEnum id;
        public BuffConfig config => Core.Tables.BuffTable.Get(id);
        public Guid[] targetGuids; // 目标guid
        public float leftTime; // 剩余时间
    }
}