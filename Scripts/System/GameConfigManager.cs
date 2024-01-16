using System.Collections;
using System.Collections.Generic;
using HegaCore;
using UnityEngine;

public class GameConfigManager : SingletonBehaviour<GameConfigManager>
{
    public Camera live2DCamera;
    /// <summary>
    /// R DLC
    /// </summary>
    public bool DarkLord { get; private set; }
    /// <summary>
    /// ACHIEVEMENT
    /// </summary>
    public bool OverLord { get; private set; }
    /// <summary>
    /// DEV CHEAT
    /// </summary>
    public bool Daemon { get; private set; }

    public static GameDataContainer DataContainer { get; private set; }
    public static GameSettings GameSettings { get; private set; }

    public string CurCharID => $"Char{DataContainer.Player.CurCharIndex + 1}Stand";
    public void SetConfig(bool dl, bool ol, bool dm, GameSettings setting, GameDataContainer container)
    {
        DarkLord = dl;
        OverLord = ol;
        Daemon = dm;
        GameSettings = setting;
        DataContainer = container;
    }
}
