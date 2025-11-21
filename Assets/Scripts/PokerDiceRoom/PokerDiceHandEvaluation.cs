using System.Collections.Generic;
using System.Linq;

namespace PokerDiceRoom
{
    public static class PokerDiceHandEvaluation 
    {
        public static PokerDiceHandResult EvaluateHand(PokerPlayer player, List<int> rolls)
        {
            Dictionary<int, int> counts = rolls.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
            List<int> sorted = rolls.OrderBy(v => v).ToList();
            List<int> frequencies = counts.Values.OrderByDescending(v => v).ToList();
            int sum = sorted.Sum();

            PokerDiceHandResult result = new PokerDiceHandResult(player.PlayerName);

            if (frequencies[0] == 5)
                return result.CreateResult(PokerDiceHandRank.FiveOfAKind, 100, "Five of a Kind!");

            if (frequencies[0] == 4)
                return result.CreateResult(PokerDiceHandRank.FourOfAKind, sum + 20, "Four of a Kind!");

            if (frequencies[0] == 3 && frequencies[1] == 2)
                return result.CreateResult(PokerDiceHandRank.FullHouse, sum + 50, "Full House!");

            if (IsStraight(sorted))
                return result.CreateResult(PokerDiceHandRank.Straight, 95, "Straight");

            if (frequencies[0] == 3)
            {
                int value = counts.First(kvp => kvp.Value == 3).Key;
                return result.CreateResult(PokerDiceHandRank.ThreeOfAKind, value * 3 + 10, "Three of a Kind");
            }

            if (frequencies.Count(f => f == 2) == 2)
            {
                int pairSum = counts.Where(kvp => kvp.Value == 2).Sum(kvp => kvp.Key * 2);
                return result.CreateResult(PokerDiceHandRank.TwoPair, pairSum, "Two Pair");
            }

            if (frequencies[0] == 2)
            {
                int value = counts.First(kvp => kvp.Value == 2).Key;
                return result.CreateResult(PokerDiceHandRank.OnePair, value * 2, "One Pair");
            }

            int highCard = sorted.Max();
            return result.CreateResult(PokerDiceHandRank.HighCard, highCard, $"High Card ({highCard})");
        }
    
        private static bool IsStraight(List<int> sortedValues)
        {
            if (sortedValues.Count != 5) return false;
        
            for (int i = 0; i < sortedValues.Count - 1; i++)
            {
                if (sortedValues[i + 1] != sortedValues[i] + 1) return false;
            }
        
            return true;
        }
    }
}