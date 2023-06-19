using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class HegaCommon
{
    public static float ParseFloatUS(this string input)
    {
        return float.Parse(input, CultureInfo.CreateSpecificCulture("en-US"));
    }
    public static string AddColor(this string str, string color)
    {
        return $"<color={color}>{str}</color>";
    }
    
    public static string Stringify(this string v)
    {
        v = v.Replace("\r", "").
            Replace("\n", "");
        return v.Replace("{", "{\"").
            Replace("}", "\"}").
            Replace(":", "\":\"").
            Replace(",", "\",\"").
            Replace("}\",\"{", "},{");
    }

    public static void TakeCapture(string filePath, string fileName)
    {
#if UNITY_EDITOR || DEV
            var Now = System.DateTime.Now;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = $"{filePath}/{fileName}_{Now.Day}_{Now.Hour}h{Now.Minute}m{Now.Millisecond}ms.png";
            ScreenCapture.CaptureScreenshot(fullPath);
            UnuLogger.Log(fullPath);
#endif
    }
    
    /// <summary>
    /// Read File in streamingAssetsPath
    /// </summary>
    /// <param name="fileName"></param> without file Extenstion
    /// <param name="dataString"></param>
    /// <returns></returns>
    public static bool ReadDevFile(string fileName, out string dataString)
    {
        string fileCheatPath = $"{Application.streamingAssetsPath}/{fileName}.txt";
        if (File.Exists(fileCheatPath))
        {
            var data = File.ReadAllText(fileCheatPath);
            if (!string.IsNullOrEmpty(data))
            {
                dataString = data;
                return true;
            }
        }
        dataString = string.Empty;
        return false;
    }
    
    /// <summary>
    /// Read File in streamingAssetsPath
    /// </summary>
    /// <param name="fileName"></param> without file Extenstion
    /// <param name="dataLines"></param>
    /// <returns></returns>
    public static bool ReadDevFile(string fileName, out string[] dataLines)
    {
        string fileCheatPath = $"{Application.streamingAssetsPath}/{fileName}.txt";
        if (File.Exists(fileCheatPath))
        {
            var data = File.ReadAllLines(fileCheatPath);
            if (data.Length > 0)
            {
                dataLines = data;
                return true;
            }
        }
        dataLines = null;
        return false;
    }


    #region Google form submit

    public static void SubmitDataToGoogleForm(string url, Dictionary<string,string> dataDict)
    {
        var form = new WWWForm();

        foreach (var pair in dataDict)
        {
            form.AddField(pair.Key, pair.Value);
        }
        
        var request = UnityWebRequest.Post(url, form);

        // handle the response
        CoroutineManager.Instance.StartCoroutine(HandleResponse(request));
    }

    private static IEnumerator HandleResponse(UnityWebRequest request)
    {
        yield return request.SendWebRequest();

        if (request.isDone)
        {
            Debug.Log("Submitted data to Google Form");
        }
        else
        {
            Debug.LogError("Failed to submit data to Google Form: " + request.error);
        }
    }
    
    #endregion
    
}
