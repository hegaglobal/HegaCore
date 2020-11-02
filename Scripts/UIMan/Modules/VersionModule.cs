using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    public sealed class VersionModule : UIManModule<VersionViewModel>
    {
        private void Start()
        {
            this.DataInstance.Version = Application.version;
        }
    }
}