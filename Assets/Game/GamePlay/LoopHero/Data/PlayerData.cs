using System;
using UnityEngine;
using UnityToolkit;

namespace Game.LoopHero
{
    [Serializable]
    public class PlayerData
    {
        [field: SerializeField] public PackageData package { get; private set; }
    }
}