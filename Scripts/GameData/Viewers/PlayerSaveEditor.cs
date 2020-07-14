#if UNITY_EDITOR

using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore.Editor
{
    public sealed class PlayerSaveEditor : MonoBehaviour
    {
        [PropertyOrder(0)]
        [BoxGroup(GroupID = "Save Data")]
        [SerializeField]
        private string saveDataFolder = string.Empty;

        [PropertyOrder(0)]
        [BoxGroup(GroupID = "Save Data")]
        [SerializeField]
        private string saveDataFile = string.Empty;

        [PropertySpace]
        [PropertyOrder(2)]
        [InlineEditor]
        [SerializeField]
        private GameSettingsViewer settings = null;

        [PropertyOrder(2)]
        [InlineEditor]
        [SerializeField]
        private PlayerSaveViewer[] saves = null;

        private void OnValidate()
        {
            this.settings = GetComponentInChildren<GameSettingsViewer>();
            this.saves = GetComponentsInChildren<PlayerSaveViewer>();
        }

        //[PropertySpace]
        //[PropertyOrder(1)]
        //[HorizontalGroup(GroupID = "Buttons")]
        //[Button]
        //public void Load()
        //{
        //    var master = GetMaster();
        //    var saveData = master.Load();
        //    this.settings.Set(saveData.Settings);

        //    var length = Mathf.Min(this.saves.Length, saveData.PlayerSaves.Length);

        //    for (var i = 0; i < length; i++)
        //    {
        //        var data = saveData.PlayerSaves[i];
        //        var save = this.saves[i];
        //        save.Set(data);
        //    }
        //}

        //[PropertySpace]
        //[PropertyOrder(1)]
        //[HorizontalGroup(GroupID = "Buttons")]
        //[Button]
        //public void Save()
        //{
        //    var master = GetMaster();
        //    var saveData = master.Load();
        //    this.settings.CopyTo(saveData.Settings);

        //    var length = Mathf.Min(this.saves.Length, saveData.PlayerSaves.Length);

        //    for (var i = 0; i < length; i++)
        //    {
        //        this.saves[i].CopyTo(saveData.PlayerSaves[i]);
        //    }

        //    master.Save(saveData);
        //}

        //private GameMasterData GetMaster()
        //{
        //    var folderPath = Path.Combine(Application.dataPath, "..", this.saveDataFolder);
        //    var filePath = Path.Combine(folderPath, this.saveDataFile);

        //    var master = new GameMasterData();
        //    master.SetFilePath(folderPath, filePath);
        //    master.EnsureSaveData();

        //    return master;
        //}
    }
}

#endif