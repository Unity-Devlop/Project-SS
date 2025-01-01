using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Framework
{
    [CreateAssetMenu(fileName = "AOTEntryConfig", menuName = "Framework/AOTEntryConfig")]
    public class AOTEntryConfig : ScriptableObject
    {
        public List<AssetReference> hybridClrDllReferences;
        public List<AssetReference> hybridClrAotMetaReferences;
        public AssetReference gameEntryReference;

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void AutoCollect()
        {
            // // 拿到BuildTarget
            // string dllPath =
            //     $"Assets/Resource/HybridCLR/HotUpdateDlls/{UnityEditor.EditorUserBuildSettings.activeBuildTarget}/";
            //
            // // 其下所有.bytes文件 都添加 一个AssetReference
            // hybridClrDllReferences = new List<AssetReference>();
            // foreach (var path in System.IO.Directory.GetFiles(dllPath, "*.bytes"))
            // {
            //     AssetReference reference = new AssetReference();
            //     reference.SetEditorAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
            //     hybridClrDllReferences.Add(reference);
            // }
            //
            //
            // string metaPath = $"Assets/Resource/HybridCLR/AOT/{UnityEditor.EditorUserBuildSettings.activeBuildTarget}/";
            // hybridClrAotMetaReferences = new List<AssetReference>();
            //
            // foreach (var path in System.IO.Directory.GetFiles(metaPath, "*.bytes"))
            // {
            //     AssetReference reference = new AssetReference();
            //     reference.SetEditorAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
            //     hybridClrAotMetaReferences.Add(reference);
            // }
            //
            // EditorUtility.SetDirty(this);
            //
            // Debug.Log("AutoCollect Done");
        }
#endif
    }
}