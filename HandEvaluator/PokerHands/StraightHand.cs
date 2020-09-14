﻿using System;
using System.Linq;

namespace Yeahbah.Poker.HandEvaluator.PokerHands
{
    public class StraightHand : IPokerHand
    {
        public HandEvaluationResult? Evaluate(Card[] hand)
        {
            var cards = hand.ToArray();
            Array.Sort(cards);
            var ok = false;
            var sameSuit = 1;
            var startIndex = 0;
            if (cards[0].CardValue == CardValue.Ace && cards[4].CardValue == CardValue.Deuce)
            {
                startIndex = 1;
                if (cards[0].Suit == cards[1].Suit)
                {
                    sameSuit++;
                }
            }

            for(var i = startIndex; i < cards.Length; i++)
            {                
                if (i < cards.Length - 1)
                {
                    var diff = cards[i].CardValue - cards[i + 1].CardValue;
                    ok = (diff == 1); 
                    if (!ok)
                    {
                        break;
                    }
                    if (cards[i].Suit == cards[i + 1].Suit)
                    {
                        sameSuit++;
                    }
                }                
            };

            if (ok)
            {
                var handWeight = cards.Sum(card => card.DefaultCardWeight);
                var firstCardIsAnAce = cards[0].CardValue == CardValue.Ace;                
                if (sameSuit == 5 && firstCardIsAnAce && cards[4].CardValue == CardValue.Ten)
                {                    
                    return new HandEvaluationResult(handWeight, HandType.RoyalFlush, hand, cards, $"Royal Straight Flush.");
                }

                if (firstCardIsAnAce)
                {
                    handWeight -= 64; // ace weight = 71, on wheel ace, ace weight should be lower than deuce
                }

                if (sameSuit == 5)
                {
                    return new HandEvaluationResult(handWeight, HandType.StraightFlush, hand, cards, $"Straight Flush to {cards[0].CardValue}");
                }
                return new HandEvaluationResult(handWeight, HandType.Straight, hand, cards, $"Straight to {cards[0].CardValue}.");
            }

            return null;
        }
    }
}
