// Get the Steamworks.NET plugin
using Steamworks;
using UnityEngine;

[System.Serializable]
public class LeaderBoardEntryF
{
    public string userName; 
    public int m_nGlobalRank;	
    public int m_nScore;
}


public class LeaderboardManager : MonoBehaviour
{
// The name of the leaderboard
    public string leaderboardName = "MyLeaderboard";

// The handle of the leaderboard
    private SteamLeaderboard_t leaderboardHandle;

// The call result objects for each operation
    private CallResult<LeaderboardFindResult_t> findResult;
    private CallResult<LeaderboardScoreUploaded_t> uploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> downloadResult;

// The array of leaderboard entries
    private LeaderboardEntry_t[] leaderboardEntries;

// The number of entries to download
    private int entryCount = 10;

// Start is called before the first frame update
    void Start()
    {
// Initialize the Steamworks API
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steamworks is not initialized");
            return;
        }

// Find or create the leaderboard
        SteamAPICall_t findCall = SteamUserStats.FindOrCreateLeaderboard(leaderboardName,
            ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending,
            ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
        findResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFound);
        findResult.Set(findCall);
    }

// Callback function for finding the leaderboard
    private void OnLeaderboardFound(LeaderboardFindResult_t result, bool failure)
    {
// Check for errors
        if (failure || result.m_bLeaderboardFound == 0)
        {
            Debug.LogError("Failed to find leaderboard");
            return;
        }

// Store the leaderboard handle
        leaderboardHandle = result.m_hSteamLeaderboard;

// Upload a score for testing
// Replace this with your own logic
        int score = Random.Range(0, 100);
        SteamAPICall_t uploadCall = SteamUserStats.UploadLeaderboardScore(leaderboardHandle,
            ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);
        uploadResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnScoreUploaded);
        uploadResult.Set(uploadCall);
    }

// Callback function for uploading a score
    private void OnScoreUploaded(LeaderboardScoreUploaded_t result, bool failure)
    {
// Check for errors
        if (failure || result.m_bSuccess != 1)
        {
            Debug.LogError("Failed to upload score");
            return;
        }

// Download the global top entries
        SteamAPICall_t downloadCall = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, entryCount);
        downloadResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnScoresDownloaded);
        downloadResult.Set(downloadCall);
    }

// Callback function for downloading scores
    private void OnScoresDownloaded(LeaderboardScoresDownloaded_t result, bool failure)
    {
// Check for errors
        if (failure)
        {
            Debug.LogError("Failed to download scores");
            return;
        }

// Store the leaderboard entries
        leaderboardEntries = new LeaderboardEntry_t[result.m_cEntryCount];
        for (int i = 0; i < result.m_cEntryCount; i++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i,
                out leaderboardEntries[i], null, 0);
        }

// Display the leaderboard entries
// Replace this with your own UI logic
        foreach (LeaderboardEntry_t entry in leaderboardEntries)
        {
            Debug.Log("User: " + SteamFriends.GetFriendPersonaName(entry.m_steamIDUser) + ", Rank: " +
                      entry.m_nGlobalRank + ", Score: " + entry.m_nScore);
        }
    }
}