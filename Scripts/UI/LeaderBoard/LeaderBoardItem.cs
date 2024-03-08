using UnuGames;
using UnuGames.MVVM;

[System.Serializable]
public class LeaderBoardEntryData
{
    public string userName;
    public int m_nGlobalRank; // newest global rank
    public int m_oGlobalRank; // previous rank, before the last update
    public int m_nScore;
    public bool m_isMine = false;
}

public class LeaderBoardItem : UIManModule<LeaderBoardEntryData>
{
    private int m_rank = default;
    [UIManAutoProperty]
    public int Rank
    {
        get { return this.m_rank; }
        set { this.m_rank = value; OnPropertyChanged(nameof(this.Rank), value); }
    }
    
    private string m_userName = default;
    [UIManAutoProperty]
    public string UserName
    {
        get { return this.m_userName; }
        set { this.m_userName = value; OnPropertyChanged(nameof(this.UserName), value); }
    }
    
    private int m_entryValue = default;
    [UIManAutoProperty]
    public int EntryValue
    {
        get { return this.m_entryValue; }
        set { this.m_entryValue = value; OnPropertyChanged(nameof(this.EntryValue), value); }
    }
    
    public void FillData(LeaderBoardEntryData entry)
    {
        DataInstance = entry;
        Refresh();
    }

    private void Refresh()
    {
        Rank = DataInstance.m_nGlobalRank;
        UserName = DataInstance.userName;
        EntryValue = DataInstance.m_nScore;
    }

    public void UpdateScore()
    {
        EntryValue = DataInstance.m_nScore;
    }
}
