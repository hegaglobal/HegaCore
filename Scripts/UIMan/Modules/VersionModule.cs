using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    public sealed class VersionModule : SingletonModule<VersionModule, VersionViewModel>
    {
        private void Start()
        {
            this.DataInstance.Version = Application.version;
        }

        public void SetGameMode(GameMode mode)
            => this.DataInstance.Version = $"{Application.version} ({mode.ToSimpleName()})";
    }
}