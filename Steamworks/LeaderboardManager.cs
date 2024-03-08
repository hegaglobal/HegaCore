// Get the Steamworks.NET plugin

using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

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
    public Dictionary<string, LeaderBoardEntryData> userRankDict = new Dictionary<string, LeaderBoardEntryData>();
    private Dictionary<string, List<Action<string>>> onLeaderBoardUpdated = new Dictionary<string, List<Action<string>>>();
    
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

    [Button]
    public void TestUploadScoreToLeaderBoard(int score)
    {
        Debug.Log("try upload score: " + score);
        TryUploadToLeaderboard("classic",
            ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending,
            ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric, score);
    }

    #region Get Data

    public List<LeaderBoardEntryData> GetLeaderBoardEntryData(string board)
    {
        return LeaderBoardEntryDataDict.TryGetValue(board, out var data) ? data : null;
    }

    #endregion
    
    
    #region Upload Score To Leaderboard
    public void TryUploadToLeaderboard(string leaderboardName, ELeaderboardSortMethod sort,
        ELeaderboardDisplayType type, int valueToUpload)
    {
        if (!Initialized)
        {
            Debug.Log("Steam SDK not Initialized");
            InvokeCallBack(leaderboardName);
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
            OnUploadToLeaderBoardCompleted(leaderboardName, t, failure);
        });
            
        uploadResult.Set(uploadCall);
    }

    private void OnUploadToLeaderBoardCompleted(string leaderBoardName, LeaderboardScoreUploaded_t result, bool failure)
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

        if (result.m_bScoreChanged == 1 || !LeaderBoardEntryDataDict.ContainsKey(leaderBoardName))
        {
            DownloadGlobalRank(leaderBoardName, result.m_hSteamLeaderboard);
        }
    }
    #endregion

    #region Download LeaderBoard
    public void TryGetGlobalRank(string leaderboardName, 
        ELeaderboardSortMethod sort = ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, 
        ELeaderboardDisplayType type = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric) 
        //Action<List<LeaderBoardEntryData>> callback = null)
    {
        if (!Initialized)
        {
            Debug.Log("Steam SDK not Initialized");
            InvokeCallBack(leaderboardName); 
            return;
        }

        if (LeaderBoardEntryDataDict.ContainsKey(leaderboardName))
        {
            InvokeCallBack(leaderboardName); 
            return;
        }
        
        FindOrCreateLeaderboard(leaderboardName, sort, type, DownloadGlobalRank,
            () => { Debug.Log("Failed to download leaderboard: " + leaderboardName); });
    }

    private void DownloadGlobalRank(string leaderboardName, SteamLeaderboard_t leaderboardHandle)
    {
        Debug.Log("DownloadGlobalRank");
        
        SteamAPICall_t downloadCall = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, entryCount);
        downloadResult = CallResult<LeaderboardScoresDownloaded_t>.Create( ((t, failure) =>
            {
                OnGlobalRankDownloaded(leaderboardName, t, failure);
            }));
        downloadResult.Set(downloadCall);
    }

    private void OnGlobalRankDownloaded(string leaderboardName, LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogError("Failed to download scores: " );
            InvokeCallBack(leaderboardName);
            return;
        }

        Debug.Log("Download Global Rank Completed");
        // Store the leaderboard entries
        var leaderboardEntries = new LeaderboardEntry_t[result.m_cEntryCount];
        for (int i = 0; i < result.m_cEntryCount; i++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i,
                out leaderboardEntries[i], null, 0);
        }
        var myID = SteamUser.GetSteamID(); 
        var leaderboardEntriesData = new List<LeaderBoardEntryData>();
        foreach (var entry in leaderboardEntries)
        {
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
        
        InvokeCallBack(leaderboardName);
    }
    
    public void SubcribeLeaderBoardUpdated(string board, Action<string> callback)
    {
        if (!onLeaderBoardUpdated.ContainsKey(board))
        {
            onLeaderBoardUpdated.Add(board, new List<Action<string>>());
        }
        onLeaderBoardUpdated[board].Add(callback);
    }

    public void UnsubcribeLeaderBoardUpdated(string board, Action<string> callback)
    {
        if (onLeaderBoardUpdated.ContainsKey(board))
        {
            onLeaderBoardUpdated[board].Remove(callback);
        }
    }

    private void InvokeCallBack(string board)
    {
        if (onLeaderBoardUpdated.TryGetValue(board, out var actions))
        {
            foreach (var action in actions)
            {
                action?.Invoke(board);
            }
        }
    }

    #endregion
}

    
