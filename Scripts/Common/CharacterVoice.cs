using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HegaCore;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class VoiceData
{
    public string voiceKey;
    public int animInt = -1;
    public string animtrigger = string.Empty;
}

[Serializable]
public class VoicePool
{
    [ListDrawerSettings(Expanded = true)]
    [TableList]
    public List<VoiceData> VoiceDatas;
    private List<int> indexes = new List<int>();

    public async UniTaskVoid PrepareVoice()
    {
        if (VoiceDatas.Count > 0)
        {
            List<string> voices = new List<string>(VoiceDatas.Count);
            for (int i = 0; i < VoiceDatas.Count; i++)
            {
                voices.Add(VoiceDatas[i].voiceKey);
            }

            await AudioManager.Instance.PrepareVoiceAsync(true, voices.ToArray());
        }
    }
    
    void RenewPool()
    {
        indexes = new List<int>(VoiceDatas.Count);
        for (int i = 0; i < VoiceDatas.Count; i++)
        {
            indexes.Add(i);
        }
    }
    
    public VoiceData GetRandomVoiceData()
    {
        if (indexes.Count == 0)
        {
            RenewPool();
        }

        int index = 0;
        if (indexes.Count > 1)
        {
            index = Random.Range(0, indexes.Count);
        }

        int result = indexes[index];
        indexes.RemoveAt(index);
        return VoiceDatas[result];
    }
    
    public VoiceData GetVoiceData(string key)
    {
        foreach (var data in VoiceDatas)
        {
            if (string.Equals(data.voiceKey, key))
            {
                return data;
            }
        }

        return null;
    }
}

public class CharacterVoice : MonoBehaviour
{
    public Animator _animator;

    public VoicePool RandomVoices;
    public VoicePool AngryVoices;
    public VoicePool HappyVoices;
    
    private float specialVoiceDelay = 0;
    
    private List<int> indexes;
    void Awake()
    {
        RandomVoices.PrepareVoice().Forget();
        AngryVoices.PrepareVoice().Forget();
        HappyVoices.PrepareVoice().Forget();
    }
    
    void OnEnable()
    {
        StartCoroutine(PlayVoiceCO());
    }
    
    void OnDisable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        AudioManager.Instance.Player.StopVoice();
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        specialVoiceDelay -= Time.fixedDeltaTime;
    }

    IEnumerator PlayVoiceCO()
    {
        yield return new WaitForSeconds(3);
        while (this.gameObject.activeSelf)
        {
            while (specialVoiceDelay > 0f)
            {
                yield return new WaitForSeconds(10f);
            }

            var data = RandomVoices.GetRandomVoiceData();
            var delay = PlayVoiceData(data) + 10;
            yield return new WaitForSeconds(delay);
        }
    }

    public void PlayAngryVoice()
    {
        AudioManager.Instance.Player.StopVoice();
        var data = AngryVoices.GetRandomVoiceData();
        specialVoiceDelay = PlayVoiceData(data);
    }

    public void PlayHappyVoice()
    {
        AudioManager.Instance.Player.StopVoice();
        var data = HappyVoices.GetRandomVoiceData();
        specialVoiceDelay = PlayVoiceData(data);
    }

    private float PlayVoiceData(VoiceData data)
    {
        if (AudioManager.Instance.TryGetVoice(data.voiceKey, out var voiceClip))
        {
            Debug.Log($"Play Voice Data: {data.voiceKey}");
            AudioManager.Instance.Player.PlayVoice(data.voiceKey);
            
            if (data.animInt > 0)
            {
                _animator.SetInteger("ID", data.animInt);
            }
            
            if (!string.IsNullOrEmpty(data.animtrigger))
            {
                _animator.SetTrigger(data.animtrigger);
            }
            
            return voiceClip.length + 5f;
        }
        else
        {
            return 5f;
        }
    }
    
    public void PlaySFX(string sfx)
    {
        AudioManager.Instance.Player.PlayAsync(sfx, HegaCore.AudioType.Sound);
    }
}

