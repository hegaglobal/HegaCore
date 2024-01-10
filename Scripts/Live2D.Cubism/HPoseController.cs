using UnityEngine;
using HegaCore;
using Sirenix.OdinInspector;
using UnuGames;

public class HPoseController : MonoBehaviour
{
    public CubismController CubismController;
    public bool hasBG;
    [ShowIf("hasBG"), ReadOnly] public Sprite BG;
    public bool hasFG;
    [ShowIf("hasFG"), ReadOnly] public Sprite FG;
    
    public void LoadBG(string ID)
    {
        UIManLoader.Load<Sprite>(ID, (s, o) =>
        {
            if (!(o is Sprite spr))
            {
                UnuLogger.LogError($"Asset of key={s} is not a Sprite.");
                return;
            }

            BG = spr;
        });
    }

    public void LoadFG(string ID)
    {
        UIManLoader.Load<Sprite>(ID, (s, o) =>
        {
            if (!(o is Sprite spr))
            {
                UnuLogger.LogError($"Asset of key={s} is not a Sprite.");
                return;
            }

            FG = spr;
        });
    }

}
