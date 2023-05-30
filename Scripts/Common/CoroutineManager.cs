using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : SingletonBehaviour<CoroutineManager>
{
    protected override void OnAwake()
    {
        base.OnAwake();
        Coroutines = new Dictionary<string, Coroutine>();
    }

    private Dictionary<string, Coroutine> Coroutines;

    public void RunAction(float delay, Action action)
    {
        RunAction(string.Empty, delay, action);
    }
    
    public void RunAction(string key, float delay, Action action)
    {
        var co = StartCoroutine(RunActionCO(key, delay, action));
        if (!string.IsNullOrEmpty(key))
        {
            if (Coroutines.ContainsKey(key))
            {
                Debug.LogError("THERE IS A COROUTINE WITH SAME NAME: " + key);
            }
            else
            {
                Coroutines.Add(key, co);
            }
        }
    }

    IEnumerator RunActionCO(string key, float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
        
        if (!string.IsNullOrEmpty(key))
        {
            Coroutines.Remove(key);
        }
    }

    public void StopAction(string key)
    {
        if (Coroutines.ContainsKey(key))
        {
            StopCoroutine(Coroutines[key]);
            Coroutines.Remove(key);
        }
        else
        {
            Debug.LogError("NO COROUTINE WITH KEY: " + key);
        }
    }
}
