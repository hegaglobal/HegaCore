using System;
using System.Collections.Generic;

[Serializable]
public class ProbabilityItem
{
    public string name; // not only string, any type of data
    public int chance;  // chance of getting this Item
}

public class ProbabilityPool
{
    List<ProbabilityItem> items;
    public ProbabilityPool (List<ProbabilityItem> _items)
    {
        items = _items;
    }
    public int ItemCount
    {
        get { return items.Count; }
    }
    public static Random rnd = new Random();

    // Static method for using from anywhere. You can make its overload for accepting not only List, but arrays also: 
    // public static Item SelectItem (Item[] items)...
    public ProbabilityItem SelectItem(bool bRemoveOnSelect)
    {
        if(items.Count == 0)
        {
            UnityEngine.Debug.Log("<color=\"red\">Error: ProbabilityPool is empty</color>");
            return null;
        }
        // Calculate the summa of all portions.
        int poolSize = 0;
        for (int i = 0; i < items.Count; i++)
        {
            poolSize += items[i].chance;
        }

        // Get a random integer from 0 to PoolSize.
        int randomNumber = rnd.Next(0, poolSize) + 1;

        // Detect the item, which corresponds to current random number.
        int accumulatedProbability = 0;
        for (int i = 0; i < items.Count; i++)
        {
            accumulatedProbability += items[i].chance;
            if (randomNumber <= accumulatedProbability)
            {
                var item = items[i];
                if (bRemoveOnSelect)
                {
                    items.RemoveAt(i);
                    poolSize -= item.chance;
                }
                return item;
            }
        }
        return null;    // this code will never come while you use this programm right :)
    }
    public void RemoveItem(string name)
    {
        items.RemoveAll(m => m.name == name);
    }
    public bool Contains(string name)
    {
        return items.Find(m => m.name == name) != null;
    }
}