// using UnityEngine;
// using Steamworks;
// using Steamworks;
//
// public class LeaderboardManager : MonoBehaviour
// {
//     private void Start()
//     {
//         // Replace "YourLeaderboardName" with your actual leaderboard name
//         SteamLeaderboard_t leaderboardHandle = Steamworks.SteamUserStats.FindLeaderboard("YourLeaderboardName");
//
//         if (leaderboardHandle != SteamLeaderboard_t.Invalid)
//         {
//             SteamAPICall_t downloadScoresCall = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 10);
//             CallResult<LeaderboardFindResult_t> callback = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardScoresDownloaded);
//             callback.Set(downloadScoresCall);
//         }
//         else
//         {
//             Debug.LogError("Failed to find leaderboard");
//         }
//     }
//
//     private void OnLeaderboardScoresDownloaded(LeaderboardFindResult_t callback, bool failure)
//     {
//         if (!failure && callback.m_bLeaderboardFound != 0)
//         {
//             SteamLeaderboardEntries_t entries;
//             SteamUserStats.GetDownloadedLeaderboardEntries(callback.m_hSteamLeaderboard, 1, 10, out entries);
//
//             for (int i = 0; i < entries.m_cEntryCount; i++)
//             {
//                 LeaderboardEntry_t entry = entries.m_LeaderboardEntries[i];
//                 Debug.Log($"Rank: {entry.m_nGlobalRank} - Score: {entry.m_nScore}");
//             }
//         }
//         else
//         {
//             Debug.LogWarning("Failed to download leaderboard scores.");
//         }
//     }
// }