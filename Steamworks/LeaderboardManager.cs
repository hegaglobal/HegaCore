// Get the Steamworks.NET plugin

using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;

public class LeaderboardManager : SerializedMonoBehaviour
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

    private void Awake()
    {
        s_instance = this;
    }

    private static bool s_initialized = false;
    public static bool Initialized => s_initialized;

    private CallResult<LeaderboardFindResult_t> findResult;
    private CallResult<LeaderboardScoreUploaded_t> uploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> downloadResult;
    
    private int entryCount = 10;
    private Dictionary<string,List<LeaderBoardEntryData>> LeaderBoardEntryDataDict = new Dictionary<string, List<LeaderBoardEntryData>>();
    private Dictionary<string, List<Action<List<LeaderBoardEntryData>>>> onDownloadedLeaderBoardCallbackDict = new Dictionary<string, List<Action<List<LeaderBoardEntryData>>>>();
    public Dictionary<string, LeaderBoardEntryData> userRankDict = new Dictionary<string, LeaderBoardEntryData>();
    public void Init()
    {
        s_initialized = SteamManager.Initialized;
        gameObject.SetActive(s_initialized);
        userRankDict.Add("classic", new LeaderBoardEntryData(){ m_nGlobalRank = 0, m_oGlobalRank = 0, m_nScore = 0, userName = string.Empty});
        userRankDict.Add("speed", new LeaderBoardEntryData(){ m_nGlobalRank = 0, m_oGlobalRank = 0, m_nScore = 0, userName = string.Empty});
        userRankDict.Add("challenge", new LeaderBoardEntryData(){ m_nGlobalRank = 0, m_oGlobalRank = 0, m_nScore = 0, userName = string.Empty});
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

        if (!userRankDict.ContainsKey(leaderBoardName))
        {
            userRankDict.Add(leaderBoardName, new LeaderBoardEntryData());
        }

        userRankDict[leaderBoardName].userName = SteamFriends.GetPersonaName();
        userRankDict[leaderBoardName].m_nScore = result.m_nScore;
        userRankDict[leaderBoardName].m_nGlobalRank = result.m_nGlobalRankNew;
        userRankDict[leaderBoardName].m_oGlobalRank = result.m_nGlobalRankPrevious;
        userRankDict[leaderBoardName].m_isMine = true;
        
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
        var myID = SteamUser.GetSteamID(); 
        var leaderboardEntriesData = new List<LeaderBoardEntryData>();
        for (int i = 0; i < leaderboardEntries.Length; i++)
        {
            var entry = leaderboardEntries[i];
            
            // My score is in top 10
            if (myID.Equals(entry.m_steamIDUser))
            {
                if (!userRankDict.ContainsKey(leaderboardName))
                {
                    userRankDict.Add(leaderboardName, new LeaderBoardEntryData());
                }

                userRankDict[leaderboardName].userName = SteamFriends.GetPersonaName();
                userRankDict[leaderboardName].m_nScore = entry.m_nScore;
                userRankDict[leaderboardName].m_nGlobalRank = entry.m_nGlobalRank;
                userRankDict[leaderboardName].m_oGlobalRank = 0;
                userRankDict[leaderboardName].m_isMine = true;
            }
            
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

    
