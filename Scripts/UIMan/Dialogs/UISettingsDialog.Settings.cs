using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore.UI
{
    public partial class UISettingsDialog
    {
        public static class Settings
        {
            public static GameSettings Data { get; private set; }

            public static ListSegment<ScreenResolution> Resolutions { get; private set; }

            public static ListSegment<string> Languages { get; private set; }

            public static AudioPlayer AudioPlayer { get; private set; }

            public static Action Save { get; private set; }

            public static void Initialize(GameSettings data, in ListSegment<ScreenResolution> resolutions,
                                          in ListSegment<string> languages, AudioPlayer audioPlayer, Action save)
            {
                Data = data;
                Resolutions = resolutions;
                Languages = languages;
                AudioPlayer = audioPlayer;
                Save = save;
            }
        }
    }
}