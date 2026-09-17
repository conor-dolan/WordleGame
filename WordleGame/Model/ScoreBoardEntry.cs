namespace WordleGame.Model
{
    public class ScoreBoardEntry
    {
        public string PlayerName { get; set; } = string.Empty;
        public string Word { get; set; } = string.Empty;
        public int Attempts { get; set; }
    }
}
