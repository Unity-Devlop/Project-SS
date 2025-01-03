using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace Framework.Editor
{
    public class FrameworkEditor : OdinMenuEditorWindow
    {
        [MenuItem("Framework/Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<FrameworkEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        private DataEditor _dataEditor;
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Add("FMOD", new FMODEditor(), EditorIcons.Microphone);
            _dataEditor = new DataEditor();
            tree.Add("Data", _dataEditor, EditorIcons.SettingsCog);
            return tree;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _dataEditor.OnDestroy();
        }
    }
}