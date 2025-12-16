using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PokerDiceRoom
{
    public static class PokerDiceAIHelper
    {
        public static HashSet<int> SelectDiceToKeep(List<(int Index, int Value)> dice, PokerDiceHandResult myHand,
            PokerDiceHandResult opponentHand)
        {
            HashSet<int> keep = new HashSet<int>();
            List<int> values = dice.Select(d => d.Value).OrderBy(v => v).ToList();

            if (myHand.Rank >= PokerDiceHandRank.FullHouse)
            {
                foreach ((int Index, int Value) d in dice)
                {
                    keep.Add(d.Index);
                }
                
                return keep;
            }

            Dictionary<int, int> counts = dice
                .GroupBy(d => d.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (KeyValuePair<int, int> pairGroup in counts.Where(c => c.Value >= 2))
            {
                foreach ((int Index, int Value) d in dice.Where(x => x.Value == pairGroup.Key))
                {
                    keep.Add(d.Index);
                }
            }

            bool isStraight = IsStraight(values);
            if (isStraight)
            {
                foreach ((int Index, int Value) d in dice)
                {
                    keep.Add(d.Index);
                }
                
                return keep;
            }

            List<int> straightCandidates = FindStraightCandidates(values);
            if (straightCandidates.Count >= 3)
            {
                foreach ((int Index, int Value) d in dice.Where(d => straightCandidates.Contains(d.Value)))
                {
                    keep.Add(d.Index);
                }
            }

            if (opponentHand.Rank > myHand.Rank)
            {
                keep = KeepOnlyStrongestGroup(counts, dice);
            }

            return keep;
        }

        private static List<int> FindStraightCandidates(List<int> sorted)
        {
            List<int> unique = sorted.Distinct().ToList();
            List<int> result = new List<int>();

            for (int i = 0; i < unique.Count - 1; i++)
            {
                if (unique[i + 1] == unique[i] + 1) result.Add(unique[i]);
            }

            if (result.Count > 0) result.Add(result.Last() + 1);

            return result;
        }

        private static HashSet<int> KeepOnlyStrongestGroup(Dictionary<int, int> counts, List<(int Index, int Value)> dice)
        {
            int best = counts.Max(kvp => kvp.Value);
            List<int> bestValues = counts.Where(c => c.Value == best)
                .Select(c => c.Key).ToList();

            HashSet<int> newKeep = new HashSet<int>();
            foreach ((int Index, int Value) d in dice.Where(x => bestValues.Contains(x.Value)))
            {
                newKeep.Add(d.Index);
            }

            return newKeep;
        }

        private static bool IsStraight(List<int> sorted)
        {
            if (sorted.Count != 5) return false;
            
            return sorted[4] - sorted[0] == 4 && sorted.Distinct().Count() == 5;
        }
    }
}