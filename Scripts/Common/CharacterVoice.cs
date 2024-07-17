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

public class CharacterVoice : MonoBehaviour
{
    [InfoBox("Random Talk")]
    [TableList]
    public List<VoiceData> VoiceDatas;

    [SerializeField]
    private Animator _animator;

    private List<int> indexes;
    void Awake()
    {
        PrepareVoice().Forget();
    }

    async UniTaskVoid PrepareVoice()
    {
        string[] voices = new string[VoiceDatas.Count];
        for (int i = 0; i < VoiceDatas.Count; i++)
        {
            voices[i] = VoiceDatas[i].voiceKey;
        }
        
        await AudioManager.Instance.PrepareVoiceAsync(true, voices);
    }
    
    void OnEnable()
    {
        StartCoroutine(PlayVoiceCO());
    }
    
    void OnDisable()
    {
        StopAllCoroutines();
        AudioManager.Instance.Player.StopVoice();
    }

    IEnumerator PlayVoiceCO()
    {
        RenewPool();
        
        yield return new WaitForSeconds(3);
        while (this.gameObject.activeSelf)
        {
            var data = VoiceDatas[GetRandomVoice()];
            if (AudioManager.Instance.TryGetVoice(data.voiceKey, out var voiceClip))
            {
                AudioManager.Instance.Player.PlayVoice(data.voiceKey);
                if (data.animInt > 0)
                {
                    _animator.SetInteger("ID", data.animInt);
                }

                if (!string.IsNullOrEmpty(data.animtrigger))
                {
                    _animator.SetTrigger(data.animtrigger);
                }
                
                yield return new WaitForSeconds(voiceClip.length + 15f);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
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

    int GetRandomVoice()
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
        return result;
    }
}

