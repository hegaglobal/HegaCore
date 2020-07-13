﻿using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HegaCore
{
    [Serializable]
    public sealed class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid) { }
    }
}