using UnityEngine;
using UnityToolkit;

namespace Game.LoopHero
{
    public static class WordsFloats
    {
        public static void Float(Vector3 worldPosition, Vector2 mainDirection, string text, float duration, Color color)
        {
            text = text.Color(color);
            GameLogger.Warning($"[{nameof(WordsFloats)}] Float: {worldPosition}, {mainDirection}, {text}, {duration}");
        }
    }
}