using System;
using TMPro;
using UnityEngine;
using UnityToolkit;

namespace Game.GamePlay.GameEntry
{
    public class VersionPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI versionText;

        private void Update()
        {
            // 设备信息 - 版本号 - 时间
            versionText.text = $"{SystemInfo.deviceType} {Application.version} {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }
    }
}