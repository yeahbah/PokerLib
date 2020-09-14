﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.HandEvaluator.PokerHands
{
    public class PairHand : IPokerHand
    {
        public HandEvaluationResult? Evaluate(Card[] hand)
        {
            var cards = hand.ToArray();
            Array.Sort(cards.ToArray());
            var found = new List<Card>();
            Array.ForEach(hand, card =>            
            {                
                var pair = cards.Where(c => c.CardValue == card.CardValue
                                        && !found.Any(c => c.CardValue == card.CardValue))
                    .ToArray();
                if (pair.Length == 2)
                {
                    found.AddRange(pair.ToArray());
                }
                else if (pair.Length > 2)
                {
                    found.Clear();
                }
            });

            var handWeight = cards.Sum(c => c.DefaultCardWeight);
            if (found.Count() == 2)
            {
                found.AddRange(cards.Where(c => !found.Contains(c)));
                return new HandEvaluationResult(handWeight, HandType.Pair, hand, found.ToArray(), $"Pair of {found[0].CardValue}s.");
            }

            if (found.Count() == 4)
            {
                found.AddRange(cards.Where(c => !found.Contains(c)));
                return new HandEvaluationResult(handWeight, HandType.TwoPair, hand, found.ToArray(), $"Two Pair, {found[0].CardValue}s and {found[2].CardValue}s.");
            }

            return null;
        }
    }
}
