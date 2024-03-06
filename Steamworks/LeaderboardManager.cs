// Get the Steamworks.NET plugin

using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class LeaderboardManager : MonoBehaviour
{
    protected static LeaderboardManager s_instance;

    public static LeaderboardManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                return new GameObject("SteamLeaderBoard ==============").AddComponent<LeaderboardManager>();
            }
            else
            {
                return s_instance;
            }
        }
    }

    private static bool s_initialized = false;
    public static bool Initialized => s_initialized;

    private CallResult<LeaderboardFindResult_t> findResult;
    private CallResult<LeaderboardScoreUploaded_t> uploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> downloadResult;
    
    private int entryCount = 10;
    private Dictionary<string,List<LeaderBoardEntryData>> LeaderBoardEntryDataDict = new Dictionary<string, List<LeaderBoardEntryData>>();
    private Dictionary<string, List<Action<List<LeaderBoardEntryData>>>> onDownloadedLeaderBoardCallbackDict = new Dictionary<string, List<Action<List<LeaderBoardEntryData>>>>();
    
    public void Init()
    {
        s_initialized = SteamManager.Initialized;
        gameObject.SetActive(s_initialized);
    }

    public void FindOrCreateLeaderboard(string leaderboardName,
        ELeaderboardSortMethod sort = ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending,
        ELeaderboardDisplayType type = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric,
        Action<string,SteamLeaderboard_t> onCompleted = null, Action onFailed = null)
    {
        if (!Initialized)
        {
            Debug.Log("Steam SDK not Initialized");
            return;
        }
        
        // Find or create the leaderboard
        SteamAPICall_t findCall = SteamUserStats.FindOrCreateLeaderboard(leaderboardName, sort, type);
        findResult = CallResult<LeaderboardFindResult_t>.Create(((t, failure) =>
        {
            if (failure || t.m_bLeaderboardFound == 0)
            {
                onFailed?.Invoke();
            }
            else
            {
                Debug.Log("New ------- " + leaderboardName);
                onCompleted?.Invoke(leaderboardName , t.m_hSteamLeaderboard);
            }
        }));

        findResult.Set(findCall);
    }

    [Sirenix.OdinInspector.Button]
    public void TestUploadScoreToLeaderBoard(int score)
    {
        Debug.Log("try upload score: " + score);
        UploadToLeaderboard("classic",
            ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending,
            ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric, score);
    }

    public void UploadToLeaderboard(string leaderboardName, ELeaderboardSortMethod sort,
        ELeaderboardDisplayType type, int valueToUpload)
    {
        if (!Initialized)
        {
            Debug.Log("Steam SDK not Initialized");
            return;
        }

        FindOrCreateLeaderboard(leaderboardName, sort, type, (
                (n,t) => { UploadToLeaderboard(n, t, valueToUpload); }),
            () => { Debug.Log("Failed to upload score to Leaderboard: " + leaderboardName); });
    }

    private void UploadToLeaderboard(string leaderboardName, SteamLeaderboard_t steamLeaderboardT, int score)
    {
        SteamAPICall_t uploadCall = SteamUserStats.UploadLeaderboardScore(steamLeaderboardT,
            ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);
        uploadResult = CallResult<LeaderboardScoreUploaded_t>.Create((t, failure) =>
            {
                OnDownloadLeaderboardAfterUpdateScore(leaderboardName, t, failure);
            });
            
        uploadResult.Set(uploadCall);
    }

    private void OnDownloadLeaderboardAfterUpdateScore(string leaderBoardName, LeaderboardScoreUploaded_t result, bool failure)
    {
        if (failure || result.m_bSuccess != 1)
        {
            Debug.LogError("Failed to upload score");
            return;
        }

        DownLoadGlobalRank(leaderBoardName, result.m_hSteamLeaderboard);
    }
    
    public void DownLoadGlobalRank(string leaderboardName, 
        ELeaderboardSortMethod sort = ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, 
        ELeaderboardDisplayType type = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric, 
        Action<List<LeaderBoardEntryData>> callback = null)
    {
        if (!Initialized)
        {
            Debug.Log("Steam SDK not Initialized");
            return;
        }

        if (LeaderBoardEntryDataDict.ContainsKey(leaderboardName))
        {
            callback.Invoke(LeaderBoardEntryDataDict[leaderboardName]);
            return;
        }
        
        if (!onDownloadedLeaderBoardCallbackDict.ContainsKey(leaderboardName))
        {
            onDownloadedLeaderBoardCallbackDict.Add(leaderboardName, new List<Action<List<LeaderBoardEntryData>>>());
        }
        onDownloadedLeaderBoardCallbackDict[leaderboardName].Add(callback);
        
        FindOrCreateLeaderboard(leaderboardName, sort, type, DownLoadGlobalRank,
            () => { Debug.Log("Failed to download leaderboard: " + leaderboardName); });
    }

    void DownLoadGlobalRank(string leaderboardName, SteamLeaderboard_t leaderboardHandle)
    {
        SteamAPICall_t downloadCall = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, entryCount);
        downloadResult = CallResult<LeaderboardScoresDownloaded_t>.Create( ((t, failure) =>
            {
                OnScoresDownloaded(leaderboardName, t, failure);
            }));
        downloadResult.Set(downloadCall);
    }

    private void OnScoresDownloaded(string leaderboardName, LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogError("Failed to download scores: " );
            return;
        }

        // Store the leaderboard entries
        var leaderboardEntries = new LeaderboardEntry_t[result.m_cEntryCount];
        for (int i = 0; i < result.m_cEntryCount; i++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i,
                out leaderboardEntries[i], null, 0);
        }

        var leaderboardEntriesData = new List<LeaderBoardEntryData>();
        for (int i = 0; i < leaderboardEntries.Length; i++)
        {
            var entry = leaderboardEntries[i];
            // Debug.Log("User: " + SteamFriends.GetFriendPersonaName(entry.m_steamIDUser) + ", Rank: " +
            //           entry.m_nGlobalRank + ", Score: " + entry.m_nScore);
            LeaderBoardEntryData data = new LeaderBoardEntryData
            {
                userName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser),
                m_nGlobalRank = entry.m_nGlobalRank,
                m_oGlobalRank = entry.m_nGlobalRank,
                m_nScore = entry.m_nScore
            };
            leaderboardEntriesData.Add(data);
        }
        
        
        if (LeaderBoardEntryDataDict.ContainsKey(leaderboardName))
        {
            Debug.Log("Update Existed Cache");
            LeaderBoardEntryDataDict[leaderboardName] = leaderboardEntriesData;
        }
        else
        {
            Debug.Log("Init New cache");
            LeaderBoardEntryDataDict.Add(leaderboardName, leaderboardEntriesData);
        }

        if (onDownloadedLeaderBoardCallbackDict.ContainsKey(leaderboardName))
        {
            var list = onDownloadedLeaderBoardCallbackDict[leaderboardName];
            foreach (var callback in list)
            {
                callback.Invoke(LeaderBoardEntryDataDict[leaderboardName]);
            }

            onDownloadedLeaderBoardCallbackDict.Remove(leaderboardName);
        }
    }
}
//     void Start()
//     {
// // Initialize the call result objects
//         findResult = new CallResult<LeaderboardFindResult_t>();
//         uploadResult = new CallResult<LeaderboardScoreUploaded_t>();
//
// // Find the leaderboard
//         SteamAPICall_t findCall = SteamUserStats.FindLeaderboard(leaderboardName);
//         findResult.Set(findCall, OnLeaderboardFindResult);
//     }
//
//     void OnLeaderboardFindResult(LeaderboardFindResult_t result, bool failure)
//     {
// // Check for errors
//         if (failure || result.m_bLeaderboardFound == 0)
//         {
//             Debug.LogError("Leaderboard not found");
//             return;
//         }
//
// // Store the leaderboard handle
//         leaderboardHandle = result.m_hSteamLeaderboard;
//
// // Upload the score
//         int score = 100; // Your score here
//         SteamAPICall_t uploadCall = SteamUserStats.UploadLeaderboardScore(leaderboardHandle, uploadMethod, score, null, 0);
//         uploadResult.Set(uploadCall, OnLeaderboardUploadResult);
//     }
//
//     void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t result, bool failure)
//     {
// // Check for errors
//         if (failure || result.m_bSuccess == 0)
//         {
//             Debug.LogError("Score upload failed");
//             return;
//         }
//
// // Print the result
//         Debug.Log("Score uploaded successfully");
//         Debug.Log("Leaderboard handle: " + result.m_hSteamLeaderboard);
//         Debug.Log("Score: " + result.m_nScore);
//         Debug.Log("Rank: " + result.m_nGlobalRankNew);
//         Debug.Log("Score changed: " + result.m_bScoreChanged);
//     }
//     
//     public IEnumerator OnLevelEnd(int score)
//     {
//         if (!SteamManager.Initialized)
//         {
//             leaderboardStatusText.text = "SteamManager is not initialized.";
//             yield break;
//         }
//
//         leaderboardStatusText.text = "Finding High Score leaderboard.";
//
//         bool error = false;
//
//         SteamLeaderboard_t highScoreLeaderboard = new SteamLeaderboard_t();
//         bool findLeaderboardCallCompleted = false;
//
//         var findLeaderboardCall = SteamUserStats.FindLeaderboard("High Score");
//         var findLeaderboardCallResult = CallResult<LeaderboardFindResult_t>.Create();
//         findLeaderboardCallResult.Set(findLeaderboardCall, (leaderboardFindResult, failure) =>
//         {
//             if (!failure && leaderboardFindResult.m_bLeaderboardFound == 1)
//             {
//                 highScoreLeaderboard = leaderboardFindResult.m_hSteamLeaderboard;
//             }
//             else
//             {
//                 error = true;
//             }
//
//             findLeaderboardCallCompleted = true;
//         });
//
//         while (!findLeaderboardCallCompleted) yield return null;
//
//         if (error)
//         {
//             leaderboardStatusText.text = "Error finding High Score leaderboard.";
//             yield break;
//         }
//
//         leaderboardStatusText.text = "Uploading score to High Score leaderboard.";
//
//         LeaderboardScoreUploaded_t leaderboardScore = new LeaderboardScoreUploaded_t();
//         bool uploadLeaderboardScoreCallCompleted = false;
//
//         var uploadLeaderboardScoreCall = SteamUserStats.UploadLeaderboardScore(highScoreLeaderboard,
//             ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, new int[0], 0);
//         var uploadLeaderboardScoreCallResult = CallResult<LeaderboardScoreUploaded_t>.Create();
//         uploadLeaderboardScoreCallResult.Set(uploadLeaderboardScoreCall, (scoreUploadedResult, failure) =>
//             {
//                 if (!failure && scoreUploadedResult.m_bSuccess == 1)
//                 {
//                     leaderboardScore = scoreUploadedResult;
//                 }
//                 else
//                 {
//                     error = true;
//                 }
//
//                 uploadLeaderboardScoreCallCompleted = true;
//             });
//
//         while (!uploadLeaderboardScoreCallCompleted) yield return null;
//
//         if (error)
//         {
//             leaderboardStatusText.text = "Error uploading to High Score leaderboard.";
//             yield break;
//         }
//
//         if (leaderboardScore.m_bScoreChanged == 1)
//         {
//             leaderboardStatusText.text =
//               String.Format("New high score! Global rank #{0}.", leaderboardScore.m_nGlobalRankNew);
//         }
//         else
//         {
//             leaderboardStatusText.text =
//               String.Format("A previous score was better. Global rank #{0}.", leaderboardScore.m_nGlobalRankNew);
//         }
//     }
    
