using System;
using System.IO;
using Game;
using Sirenix.OdinInspector;

namespace Framework.Editor
{
    [Serializable]
    internal class DataEditor
    {
        [Button("删除数据库")]
        private void ClearDatabase()
        {
            // TODO 清空数据库
            if (File.Exists(DataSystem.dbPath))
            {
                File.Delete(DataSystem.dbPath);
            }
        }
    }
}