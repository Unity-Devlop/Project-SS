using System;
using System.Collections.Generic;
using cfg;
using Game.LoopHero.CardEffect;
using UnityEngine;
using UnityToolkit;

namespace Game.LoopHero
{
    public static class ItemCardEffects
    {
        public delegate bool EffectExecute(ItemEnum cardId, Collider2D collider2D);

        private static readonly Dictionary<ItemEnum, EffectExecute> _effectExecuteDict =
            new Dictionary<ItemEnum, EffectExecute>();

        public static EffectCardConfig EffectConfig(this ItemEnum cardId)
        {
            return Core.Tables.EffectCardTable.GetOrDefault(cardId);
        }
        public static void GenerateAll()
        {
            // 收集所有的卡牌效果
            _effectExecuteDict.Clear();
            var types = typeof(EffectForItemCardAttribute).Assembly.GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(EffectForItemCardAttribute), false);
                    foreach (var attribute in attributes)
                    {
                        if (attribute is not EffectForItemCardAttribute cardEffectAttribute) continue;
                        var index = Core.Tables.CardIndexTable.GetOrDefault(cardEffectAttribute.cardId);
                        if (index == null)
                        {
                            GameLogger.Log.Error("道具:{0}不存在卡牌配置", cardEffectAttribute.cardId);
                            continue;
                        }


                        if (index.Type != CardTypeEnum.道具卡)
                        {
                            GameLogger.Log.Error("道具:{0}不是道具卡", index.Id);
                            continue;
                        }


                        var config = Core.Tables.EffectCardTable.GetOrDefault(cardEffectAttribute.cardId);

                        if (config == null)
                        {
                            GameLogger.Log.Error("道具:{0}不存在卡牌配置", cardEffectAttribute.cardId);
                            continue;
                        }

                        _effectExecuteDict.Add(cardEffectAttribute.cardId,
                            (EffectExecute)Delegate.CreateDelegate(typeof(EffectExecute), method));
                    }
                }
            }
        }

        public static bool Execute((ItemEnum Id, Collider2D collider) valueTuple)
        {
            if (_effectExecuteDict.TryGetValue(valueTuple.Id, out var effectExecute))
            {
              return  effectExecute(valueTuple.Id, valueTuple.collider);
            }

            GameLogger.Log.Error("道具:{0}没有对应的效果", valueTuple.Id);
            return false;
        }
    }
}