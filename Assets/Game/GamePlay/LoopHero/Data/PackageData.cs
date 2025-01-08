using System;
using System.Collections.Generic;
using cfg;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.LoopHero
{
    [Serializable]
    public class PackageData
    {
        [Serializable]
        public class Pair
        {
            public ItemEnum id;
            public uint count;
        }

        [SerializeField] private List<Pair> _items;
        [JsonIgnore] public IReadOnlyList<Pair> items => _items;


        public IEnumerable<ItemEnum> Query(ItemTypeEnum typeEnum)
        {
            foreach (var pair in _items)
            {
                var cfg = Core.Tables.ItemTable.Get(pair.id);
                if (cfg.Type == typeEnum)
                {
                    yield return pair.id;
                }
            }
        }
    }
}