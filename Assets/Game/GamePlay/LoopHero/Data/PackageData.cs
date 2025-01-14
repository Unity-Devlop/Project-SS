using System;
using System.Collections.Generic;
using cfg;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.LoopHero
{
    [Serializable]
    public class PackageData
    {
        [Serializable]
        public struct PackageItem
        {
            [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
            [Sirenix.OdinInspector.HorizontalGroup("Item")]
            [JsonRequired]
            public readonly int idx;

            [Sirenix.OdinInspector.HorizontalGroup("Item")]
            public ItemEnum id;

            [Sirenix.OdinInspector.HorizontalGroup("Item")]
            public ushort count;

            [JsonIgnore] public ItemConfig config => Core.Tables.ItemTable.Get(id);

            public PackageItem(int idx, ItemEnum id, ushort count)
            {
                this.id = id;
                this.count = count;
                this.idx = idx;
            }
        }

        [JsonRequired] [SerializeField] private List<PackageItem> _items;
        [JsonIgnore] public IReadOnlyList<PackageItem> items => _items;

        public void Get(int idx,out PackageItem item)
        {
            item = _items[idx];
        }

        public void Add(ItemEnum id, ushort count)
        {
            var config = Core.Tables.ItemTable.Get(id);
            short maxStack = config.MaxStack;
            Assert.IsTrue(maxStack > 0, $"物品{id}不可堆叠!!! 检查配置表");
            // 先填充已有的物品
            for (int i = 0; i < _items.Count; i++)
            {
                var pair = _items[i];
                if (pair.id == id)
                {
                    ushort addCount = (ushort)Math.Min(count, maxStack - pair.count);
                    _items[i] = new PackageItem(i, id, (ushort)(pair.count + addCount));
                    count -= addCount;
                }
            }

            // 剩余的物品新建一个
            if (count > 0)
            {
                _items.Add(new PackageItem(_items.Count, id, count));
            }
        }

        /// <summary>
        /// 移除指定数量的物品
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="count"></param>
        public void Remove(int idx, ushort count)
        {
            var pair = _items[idx];
            Assert.IsTrue(pair.count >= count, $"物品{pair.id}数量不足!!!");
            pair.count -= count;
            if (pair.count == 0)
            {
                pair.id = ItemEnum.None;
            }

            _items[idx] = pair;
        }

        /// <summary>
        /// 移除指定数量的物品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Remove(ItemEnum id, ushort count)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (count == 0) break;
                var pair = _items[i];
                if (pair.id == id)
                {
                    ushort removeCount = Math.Min(count, pair.count);
                    count -= removeCount;
                }
            }

            if (count > 0) throw new ArgumentException($"物品{id}数量不足!!!");
        }

        public int Count(ItemEnum id)
        {
            int count = 0;
            foreach (var pair in _items)
            {
                if (pair.id == id)
                {
                    count += pair.count;
                }
            }

            return count;
        }

        public IEnumerable<PackageItem> Query(ItemTypeEnum typeEnum)
        {
            foreach (var pair in _items)
            {
                if (pair.config.Type == typeEnum)
                {
                    yield return pair;
                }
            }
        }

        public IEnumerable<ItemEnum> FindAllCard(CardTypeEnum cardTypeEnum)
        {
            foreach (var pair in _items)
            {
                if (pair.config.Type != ItemTypeEnum.卡牌) continue;
                var cardIndexConfig = Core.Tables.CardIndexTable.Get(pair.id);
                if (cardIndexConfig.Type == cardTypeEnum)
                {
                    yield return pair.id;
                }
            }
        }


#if UNITY_EDITOR
        [Sirenix.OdinInspector.HorizontalGroup("Editor")]
        [Sirenix.OdinInspector.Button]
#endif
        internal void Validate()
        {
            foreach (var pair in _items)
            {
                if (pair.id == ItemEnum.None) continue;
                Assert.IsTrue(pair.config != null, $"物品{pair.id}不存在!!!");
                Assert.IsTrue(pair.config is { MaxStack: > 0 }, $"物品{pair.id}不可堆叠!!! 检查配置表");
            }

            for (int i = 0; i < _items.Count; i++)
            {
                var pair = _items[i];
                Assert.IsTrue(pair.idx == i, $"物品{pair.id}的索引不正确!!!");
            }
        }

#if UNITY_EDITOR
        [Sirenix.OdinInspector.HorizontalGroup("Editor")]
        [Sirenix.OdinInspector.Button]
#endif
        internal void ReBuildIndex()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                var pair = _items[i];
                var newPair = new PackageItem(i, pair.id, pair.count);
                _items[i] = newPair;
            }
        }
    }
}