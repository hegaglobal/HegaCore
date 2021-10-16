using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HegaCore;
using Random = UnityEngine.Random;

public class HappyGamesCheat : SingletonBehaviour<HappyGamesCheat>
{
    private bool cheat = false;
    private bool showCheat = false;
    public string[] achievementKeys;
    private Action<string> onAchievementCheat;
    
    public void ActiveCheatAchievement(Action<string> callback)
    {
#if !UNITY_EDITOR
        onAchievementCheat += callback;
        StartCoroutine(SetAchievementCO());
#endif
    }
    
    IEnumerator SetAchievementCO()
    {
        cheat = true;
        showCheat = true;
        if (achievementKeys == null || achievementKeys.Length == 0 || onAchievementCheat == null)
        {
            yield break;
        }
        yield return new WaitForSeconds(100f);
        foreach (var key in achievementKeys)
        {
            onAchievementCheat(key);
            yield return new WaitForSeconds(Random.Range(60,120));
        }

        onAchievementCheat = null;
    }

    void OnGUI()
    {
        if (cheat)
        {
            GUI.Label(new Rect(10, 10, 400, 200),
                showCheat ? "..." : "HAPPY GAMES CHEAT ACTIVATED !!! [NumLock] to hide Cheat");

            if (Input.GetKeyDown(KeyCode.Numlock))
            {
                showCheat = !showCheat;
            }
        }
    }
}