using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    public sealed class VersionModule : SingletonModule<VersionModule, VersionViewModel>
    {
        private void Start()
        {
            var ver = Application.version;
            this.DataInstance.Version = ver;
            Debug.Log($"Version: {ver}");
        }

        public void SetGameMode(GameMode mode)
            => this.DataInstance.Version = $"{Application.version} ({mode.ToSimpleName()})";
    }
}