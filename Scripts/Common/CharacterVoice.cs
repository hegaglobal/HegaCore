using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HegaCore;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class VoiceData
{
    public string voiceKey;
    public int animInt = -1;
    public string animtrigger = string.Empty;
}

public class CharacterVoice : MonoBehaviour
{
    [TableList]
    public List<VoiceData> VoiceDatas;

    private int voiceIndex = 0;
    [SerializeField]
    private Animator _animator;
    
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
        yield return new WaitForSeconds(3);
        while (this.gameObject.activeSelf)
        {
            var data = VoiceDatas[voiceIndex];
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

            voiceIndex++;
            if (voiceIndex>= VoiceDatas.Count)
            {
                voiceIndex = 0;
            }
        }
    }
}

