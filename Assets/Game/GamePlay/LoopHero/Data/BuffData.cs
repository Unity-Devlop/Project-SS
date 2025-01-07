using System;
using cfg;
using Newtonsoft.Json;

namespace Game.LoopHero
{
    [Serializable]
    public sealed class BuffData
    {
        public BuffEnum id;
        [JsonIgnore] public BuffConfig config => Core.Tables.BuffTable.Get(id);
        public Guid[] targetGuids; // 目标guid
        public float leftTime; // 剩余时间
    }
}