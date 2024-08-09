using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HegaCore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AudioType = HegaCore.AudioType;

public class BGMPool : MonoBehaviour
{
    public List<string> BGMs;

    [ShowInInspector]
    private ProbabilityPool bgmPool;
    [ShowInInspector]
    private ProbabilityPool bgmPoolPlaylist;

    private List<string> bgmPlaylist = new List<string>();

#if UNITY_EDITOR
    public bool debug = false;
#endif

    /// <summary>
    /// Initializes a ProbabilityPool with given BGM items.
    /// </summary>
    private ProbabilityPool InitializePool(List<string> bgms)
    {
        var items = new List<ProbabilityItem>();
        foreach (var bgm in bgms)
        {
            items.Add(new ProbabilityItem { name = bgm, chance = 1 });
        }

        return new ProbabilityPool(items);
    }

    /// <summary>
    /// Plays a random BGM from the general BGM list.
    /// </summary>
    public void PlayRandomBattleBGM()
    {
        StopAllCoroutines();
        if (bgmPool == null || bgmPool.ItemCount == 0)
        {
            bgmPool = InitializePool(BGMs);
        }

        PlayRandomBGM(bgmPool, false);
    }

    /// <summary>
    /// Plays BGM from a specified playlist.
    /// </summary>
    public void PlayBGMPlaylist(List<string> keys)
    {
        StopAllCoroutines();
        bgmPlaylist = keys;

        if (bgmPoolPlaylist == null || bgmPoolPlaylist.ItemCount == 0)
        {
            bgmPoolPlaylist = InitializePool(bgmPlaylist);
        }

        PlayRandomBGM(bgmPoolPlaylist, true);
    }

    /// <summary>
    /// Selects and plays a random BGM from the given pool, scheduling the next play.
    /// </summary>
    private void PlayRandomBGM(ProbabilityPool pool, bool isPlaylist)
    {
        var bgm = pool.SelectItem(true);

        AddressablesManager.LoadAsset<AudioClip>(bgm.name, (s, asset) =>
        {
            AudioManager.Instance.Player.Play(s, AudioType.Music);

#if UNITY_EDITOR
            Debug.Log($"<color=yellow>Current play Track {(BGMs.IndexOf(bgm.name) + 1)}</color>");
#endif

            StartCoroutine(ScheduleNextBGM(asset.length - 1f, isPlaylist));
        });
    }

    /// <summary>
    /// Schedules the next BGM playback, either from the playlist or randomly from the general pool.
    /// </summary>
    private IEnumerator ScheduleNextBGM(float time, bool isPlaylist)
    {
#if UNITY_EDITOR
        if(debug)
            Debug.Log(time + " wait BGMMMMMMMMMMM");
        yield return new WaitForSeconds(debug ? 10f : time);
#else
        yield return new WaitForSeconds(time);
#endif

        if (isPlaylist)
        {
            if (bgmPlaylist == null || bgmPlaylist.Count <= 0)
            {
                PlayRandomBattleBGM();
                yield break;
            }

            if (bgmPoolPlaylist == null || bgmPoolPlaylist.ItemCount == 0)
            {
                bgmPoolPlaylist = InitializePool(bgmPlaylist);
            }

            PlayRandomBGM(bgmPoolPlaylist, true);
        }
        else
        {
            PlayRandomBattleBGM();
        }
    }

    /// <summary>
    /// Stops all ongoing coroutines.
    /// </summary>
    public void Stop()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Preloads all BGM audio clips asynchronously.
    /// </summary>
    public void LoadBGM()
    {
        AudioManager.Instance.PrepareMusicAsync(false, BGMs.ToArray()).Forget();
    }
}
