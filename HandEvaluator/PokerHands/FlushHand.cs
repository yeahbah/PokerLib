﻿using System;
using System.Linq;

namespace Yeahbah.Poker.HandEvaluator.PokerHands
{
    public class FlushHand : IPokerHand
    {
        public HandEvaluationResult? Evaluate(Card[] hand)
        {
            var cards = hand.ToArray();
            Array.Sort(cards);
            var sameSuit = 1;
            for(var i = 0; i < cards.Length; i++)
            {
                var card = cards[i];
                if (i < cards.Length - 1)
                {
                    if (card.Suit == cards[i + 1].Suit)
                    {
                        sameSuit++;
                    }
                }
            }

            // should we be worried a straight flushes or just leave it to the evaluator?            
            if (sameSuit == 5 && (new StraightHand().Evaluate(hand) == null)) 
            {
                var handWeight = cards.Sum(card => card.DefaultCardWeight);
                return new HandEvaluationResult(handWeight, HandType.Flush, hand, cards, $"Flush, {cards[0].CardValue} High.");
            }

            return null;
        }
    }
}
