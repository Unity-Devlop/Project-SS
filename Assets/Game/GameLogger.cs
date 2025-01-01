using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Game
{
    public static class GameLogger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void EditorLog(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void EditorLog(string msg, Color color)
        {
            UnityEngine.Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void EditorWarning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void EditorWarning(string msg, Color color)
        {
            UnityEngine.Debug.LogWarning($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>");
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void EditorError(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void EditorError(string msg, Color color)
        {
            UnityEngine.Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log(msg); // TODO Write TO File
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(string msg, Color color)
        {
            UnityEngine.Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>"); // TODO Write TO File
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg); // TODO Write TO File
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warning(string msg, Color color)
        {
            UnityEngine.Debug.LogWarning(
                $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>"); // TODO Write TO File
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string msg)
        {
            UnityEngine.Debug.LogError(msg); // TODO Write TO File
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string msg, Color color)
        {
            UnityEngine.Debug.LogError(
                $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>"); // TODO Write TO File
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Exception(System.Exception e)
        {
            UnityEngine.Debug.LogException(e); // TODO Write TO File
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("UNITY_EDITOR")]
        public static void EditorAssert(bool condition, string msg = "")
        {
            Debug.Assert(condition, msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assert(bool condition, string msg = "")
        {
            Debug.Assert(condition, msg);
        }
    }
}