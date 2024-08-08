using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HegaCore;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AudioType = HegaCore.AudioType;

public class BGMPool : MonoBehaviour
{
    public List<string> BGMs;
    
    private ProbabilityPool BGMpool;

    private void InitListBGM()
    {
        var items = new List<ProbabilityItem>();
        foreach (var t in BGMs)
        {
            items.Add(new ProbabilityItem(){name  = t, chance = 1});
        }
        
        BGMpool = new ProbabilityPool(items);
    }
    
    public void PlayRandomBattleBGM()
    {
        if (BGMpool == null || BGMpool.ItemCount == 0)
        {
            InitListBGM();
        }

        var bgm = BGMpool.SelectItem(true);

        AddressablesManager.LoadAsset<AudioClip>(bgm.name, ((s, asset) =>
        {
            AudioManager.Instance.Player.Play(s, AudioType.Music);
            StartCoroutine(PlayNextRandom(asset.length + 3f));
        }));
    }

    int currentIndex = 0;
    List<string> BGMPlaylist = new List<string>();
    public void PlayBGMPlaylist(List<string> keys)
    {
        if (BGMpool == null || BGMpool.ItemCount == 0)
        {
            InitListBGM();
        }
        BGMPlaylist = keys;
        AddressablesManager.LoadAsset<AudioClip>(keys[currentIndex], ((s, asset) =>
        {
            AudioManager.Instance.Player.Play(s, AudioType.Music);
            StartCoroutine(PlayNext(asset.length + 3f));
        }));
    }

    IEnumerator PlayNext(float time)
    {
        yield return new WaitForSeconds(time);
        if (BGMPlaylist == null || BGMPlaylist.Count <= 0)
        {
            PlayRandomBattleBGM();
            yield break;
        }

        currentIndex++;
        if (currentIndex >= BGMPlaylist.Count) currentIndex = 0;
        AddressablesManager.LoadAsset<AudioClip>(BGMPlaylist[currentIndex], ((s, asset) =>
        {
            AudioManager.Instance.Player.Play(s, AudioType.Music);
            StartCoroutine(PlayNext(asset.length + 3f));
        }));
    }

#if UNITY_EDITOR
    public bool debug = false;
    IEnumerator PlayNextRandom(float time)
    {
        Debug.Log(time + " wait BGMMMMMMMMMMM");
        yield return new WaitForSeconds( debug ? 10f : time);
#else
    IEnumerator PlayNextRandom(float time)
    {
        yield return new WaitForSeconds(time);
#endif

        PlayRandomBattleBGM();
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public void LoadBGM()
    {
        AudioManager.Instance.PrepareMusicAsync(false, BGMs.ToArray()).Forget();
    }
}
