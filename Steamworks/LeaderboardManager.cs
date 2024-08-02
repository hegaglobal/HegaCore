// Get the Steamworks.NET plugin

using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using System;
using System.Collections;
using Sirenix.OdinInspector;

[Serializable]
public class LeaderBoardInfo
{
    public string boardName;
    public ELeaderboardSortMethod SortMethod;
    public ELeaderboardDisplayType DisplayType;

    public LeaderBoardInfo(string name)
    {
        boardName = name;
        SortMethod = ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending;
        DisplayType = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    [TableList] public List<LeaderBoardInfo> definedLeaderBoards;

    protected static LeaderboardManager s_instance;

    public static LeaderboardManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                Debug.LogError("LEADER BOARD MUST has");
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
        if (s_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        s_instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    [Button]
    void TryUploadTest(int score, string board)
    {
        TryUploadToLeaderboard(board, score);
    }
    
    private static bool s_initialized = false;
    public static bool Initialized => s_initialized;

    private CallResult<LeaderboardFindResult_t> findResult;
    private CallResult<LeaderboardScoreUploaded_t> uploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> downloadResult;
    private int entryCount = 10;

    private Dictionary<string, List<LeaderBoardEntryData>> LeaderBoardEntryDataDict =
        new Dictionary<string, List<LeaderBoardEntryData>>();

    public Dictionary<string, LeaderBoardEntryData> userRankDict = new Dictionary<string, LeaderBoardEntryData>();

    private Dictionary<string, List<Action<string>>> onLeaderBoardUpdated =
        new Dictionary<string, List<Action<string>>>();

    public void Init(Dictionary<string, int> scoreDict)
    {
        Debug.Log("Leader Board Manager Init");
        StartCoroutine(InitCO(scoreDict));
    }

    private IEnumerator InitCO(Dictionary<string, int> scoreDict)
    {
        yield return new WaitForSeconds(1f);
        // Wait Steam Manager
        int timeout = 0;
        while (timeout < 1000 && SteamManager.Initializing)
        {
            timeout++;
            yield return null;
        }
        
        s_initialized = SteamManager.Initialized;
        gameObject.SetActive(s_initialized);

        if (!s_initialized)
            yield break;

        if (definedLeaderBoards == null)
            yield break;
        
        Debug.Log("Leader Board Initialized ==========");
        
        for (int i = 0; i < definedLeaderBoards.Count; i++)
        {
            TryUploadToLeaderboard(definedLeaderBoards[i].boardName, scoreDict[definedLeaderBoards[i].boardName]); // to download all leaderboard.
            yield return new WaitForSeconds(2f); 
        }
    }


    #region Register

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

    #endregion


    #region Get Data

    public LeaderBoardInfo GetLeaderBoardInfo(string boardName)
    {
        if (definedLeaderBoards == null)
        {
            definedLeaderBoards = new List<LeaderBoardInfo>();
        }

        if (definedLeaderBoards.Count == 0)
        {
            var info = new LeaderBoardInfo(boardName);
            definedLeaderBoards.Add(info);
            return info;
        }
        
        foreach (var info in definedLeaderBoards)
        {
            if (string.Equals(info.boardName, boardName))
            {
                return info;
            }
        }
        
        var newInfo = new LeaderBoardInfo(boardName);
        definedLeaderBoards.Add(newInfo);
        return newInfo;
    }
    
    public LeaderBoardEntryData GetUserRank(string board)
    {
        if (!userRankDict.ContainsKey(board))
        {
            Log("Get User Rank --- ");
            userRankDict.Add(board, new LeaderBoardEntryData());
        }

        return userRankDict[board];
    }
    
    private void FindOrCreateLeaderboard(string leaderboardName,
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
    
    public List<LeaderBoardEntryData> GetLeaderBoardEntryData(string board)
    {
        return LeaderBoardEntryDataDict.TryGetValue(board, out var data) ? data : null;
    }

    #endregion
    
    #region Upload Score To Leaderboard
    public void TryUploadToLeaderboard(string leaderboardName, int valueToUpload)
    {
        if (!Initialized)
        {
            Debug.LogError("Steam SDK not Initialized");
            InvokeCallBack(leaderboardName);
            return;
        }

        var info = GetLeaderBoardInfo(leaderboardName);

        FindOrCreateLeaderboard(leaderboardName, info.SortMethod, info.DisplayType, (
                (n,t) => { UploadToLeaderboard(n, t, valueToUpload); }),
            () => { Log("Failed to upload score to Leaderboard: " + leaderboardName); });
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
        Log($"UPLOAD COMPLETED: {leaderBoardName.AddColor("green")} -- {result.m_nScore} -- {result.m_nGlobalRankNew} -- {result.m_nGlobalRankPrevious}");
        
        if (!userRankDict.ContainsKey(leaderBoardName))
        {
            userRankDict.Add(leaderBoardName, new LeaderBoardEntryData());
            userRankDict[leaderBoardName].userName = SteamFriends.GetPersonaName();
            userRankDict[leaderBoardName].m_nGlobalRank = result.m_nGlobalRankNew;
            
            if (result.m_nGlobalRankNew > 10)
                DownloadUserRank(leaderBoardName, result.m_hSteamLeaderboard); 
        }
        
        if (result.m_bScoreChanged == 1 || !LeaderBoardEntryDataDict.ContainsKey(leaderBoardName))
        {
            var userRank = GetUserRank(leaderBoardName); // to create if null
            
            userRank.m_nScore = result.m_nScore;
            userRank.m_nGlobalRank = result.m_nGlobalRankNew;
            userRank.m_oGlobalRank = result.m_nGlobalRankPrevious;
            userRank.m_isMine = true;
            
            DownloadGlobalRank(leaderBoardName, result.m_hSteamLeaderboard);
        }
#if UNITY_EDITOR
        else
        {
            Debug.Log("Upload Completed: not changed");
        }
#endif
    }
    #endregion

    #region Download LeaderBoard
    public void TryGetGlobalRank(string leaderboardName, bool forceDownload = false)
    {
        if (!Initialized)
        {
            Debug.Log("Steam SDK not Initialized");
            InvokeCallBack(leaderboardName); 
            return;
        }

        if (!forceDownload)
        {
            if (LeaderBoardEntryDataDict.ContainsKey(leaderboardName))
            {
                InvokeCallBack(leaderboardName);
                return;
            }
        }
        
        var info = GetLeaderBoardInfo(leaderboardName);
        
        FindOrCreateLeaderboard(leaderboardName, info.SortMethod, info.DisplayType, DownloadGlobalRank,
            () => { Log("Failed to download leaderboard: " + leaderboardName); });
    }

    private void DownloadGlobalRank(string leaderboardName, SteamLeaderboard_t leaderboardHandle)
    {
        Log("DownloadGlobalRank");
        
        SteamAPICall_t downloadCall = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, entryCount);
        downloadResult = CallResult<LeaderboardScoresDownloaded_t>.Create( ((t, failure) =>
            {
                OnGlobalRankDownloaded(leaderboardName, t, failure);
            }));
        downloadResult.Set(downloadCall);
    }
    
    private void DownloadUserRank(string leaderboardName, SteamLeaderboard_t leaderboardHandle)
    {
        Log("Download User Rank");
        
        SteamAPICall_t downloadCall = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -1, 1);
        downloadResult = CallResult<LeaderboardScoresDownloaded_t>.Create( ((t, failure) =>
        {
            OnUserRankDownloaded(leaderboardName, t, failure);
        }));
        downloadResult.Set(downloadCall);
    }

    private void OnUserRankDownloaded(string leaderboardName, LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogError("Failed to download USER scores: ---" );
            return;
        }
        
        var leaderboardEntries = new LeaderboardEntry_t[result.m_cEntryCount];
        for (int i = 0; i < result.m_cEntryCount; i++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i,
                out leaderboardEntries[i], null, 0);
        }
        
        var userRank = GetUserRank(leaderboardName);
        var myID = SteamUser.GetSteamID();
        
        foreach (var entry in leaderboardEntries)
        {
            Log($"Download USER Rank Completed: Rank: {entry.m_nGlobalRank} -- {entry.m_nScore}");
            if (myID.Equals(entry.m_steamIDUser))
            {
                userRank.m_nScore = entry.m_nScore;
                userRank.m_nGlobalRank = entry.m_nGlobalRank;
                userRank.m_oGlobalRank = 0;
                userRank.m_isMine = true;
            }
        }
    }
    
    private void OnGlobalRankDownloaded(string leaderboardName, LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogError("Failed to download scores: " );
            InvokeCallBack(leaderboardName);
            return;
        }

        Log("Download Global Rank Completed -- " + leaderboardName.AddColor("red"));

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
                var userRank = GetUserRank(leaderboardName);
                
                userRank.m_nScore = entry.m_nScore;
                userRank.m_nGlobalRank = entry.m_nGlobalRank;
                userRank.m_oGlobalRank = 0;
                userRank.m_isMine = true;
            }
            
            LeaderBoardEntryData data = new LeaderBoardEntryData
            {
                userName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser),
                m_nGlobalRank = entry.m_nGlobalRank,
                m_oGlobalRank = entry.m_nGlobalRank,
                m_nScore = entry.m_nScore
            };
            Log($"{leaderboardName} --{data.m_nGlobalRank} {data.userName} -- {data.m_nScore}");
            leaderboardEntriesData.Add(data);
        }
        
        if (LeaderBoardEntryDataDict.ContainsKey(leaderboardName))
        {
            Log("Update Existed Cache -- " + leaderboardName.AddColor("red"));
            LeaderBoardEntryDataDict[leaderboardName] = leaderboardEntriesData;
        }
        else
        {
            Log("Init New cache --- "+ leaderboardName.AddColor("red"));
            LeaderBoardEntryDataDict.Add(leaderboardName, leaderboardEntriesData);
        }
        
        InvokeCallBack(leaderboardName);
    }
    
    private void InvokeCallBack(string board)
    {
        if (!onLeaderBoardUpdated.TryGetValue(board, out var actions)) return;
        foreach (var action in actions)
        {
            action?.Invoke(board);
        }
    }

    #endregion


    public bool allowLog = false;
    void Log(string log)
    {
        if (allowLog)
        {
            Debug.Log(log);
        }
    }


}

    
