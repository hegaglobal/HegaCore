using UnityEngine;
using HegaCore;
using Sirenix.OdinInspector;
using UnuGames;

public class HPoseController : MonoBehaviour
{
    public CubismController CubismController;
    public bool hasBG;
    // [ShowIf("hasBG"),ReadOnly]
    // public SpriteRenderer BG;
    public bool hasFG;
    // [ShowIf("hasFG"),ReadOnly]
    // public SpriteRenderer FG;

    // public void ShowBG(bool show)
    // {
    //     if (BG != null)
    //     {
    //         BG.enabled = show;
    //     }
    // }
    //
    // public void ShowFG(bool show)
    // {
    //     if (FG != null)
    //     {
    //         FG.enabled = show;
    //     }
    // }

    // public void LoadBG(string ID)
    // {
    //     if (hasBG && BG != null)
    //     {
    //         UIManLoader.Load<Sprite>(ID, (s, o) =>
    //         {
    //             if (!(o is Sprite spr))
    //             {
    //                 UnuLogger.LogError($"Asset of key={s} is not a Sprite.");
    //                 return;
    //             }
    //             BG.sprite = spr;
    //         });
    //     }
    // }

    // public void LoadFG(string ID)
    // {
    //     if (hasFG && FG != null)
    //     {
    //         UIManLoader.Load<Sprite>(ID, (s, o) =>
    //         {
    //             if (!(o is Sprite spr))
    //             {
    //                 UnuLogger.LogError($"Asset of key={s} is not a Sprite.");
    //                 return;
    //             }
    //             FG.sprite = spr;
    //         });
    //     }
    // }
    
}
