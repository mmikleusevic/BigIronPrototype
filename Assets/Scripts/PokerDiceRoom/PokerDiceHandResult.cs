namespace PokerDiceRoom
{
    public class PokerDiceHandResult
    {
        public PokerDiceHandRank Rank { get; private set; }
        public int Score { get; private set; }
        public string Description { get; private set; }
        public string PlayerName { get; private set; }
        
        public PokerDiceHandResult(string playerName)
        {
            PlayerName = playerName;    
        }

        public void CreateResult(PokerDiceHandRank rank, int score, string description)
        {
            Rank = rank;
            Score = score;
            Description = description;
        }
    }
}