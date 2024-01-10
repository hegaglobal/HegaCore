using System.Collections;
using System.Collections.Generic;
using HegaCore;
using UnityEngine;

public class GameConfigManager : SingletonBehaviour<GameConfigManager>
{
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

    public void SetConfig(bool dl, bool ol, bool dm, GameSettings setting)
    {
        DarkLord = dl;
        OverLord = ol;
        Daemon = dm;
        GameSettings = setting;
    }
    
    public GameSettings GameSettings { get; private set; }
}
