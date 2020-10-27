using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Yeahbah.Poker;
using Yeahbah.Poker.HandEvaluator;

namespace SpeedTest
{
    static class Program
    {
        private static BlockingCollection<Card[]> _hands = new BlockingCollection<Card[]>();
        private static int _numberOfHands = 10_000_000;
        static void Main(string[] args)
        {
            Console.WriteLine($"Setting up {_numberOfHands} hands...");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            SetupHands();
            stopWatch.Stop();
            Console.WriteLine($"Elapsed: {stopWatch.Elapsed}");
            
            Console.WriteLine($"Evaluating {_numberOfHands} hands...");
            stopWatch.Start();
            
            var handsPerTask = _numberOfHands / 10;
            var tasks = new List<Task>();
            var hands = _hands.ToList();
            for (int i = 0; i < 10; i++)
            {
                var h = hands.Take(handsPerTask);
                var task = Task.Run(() =>
                {
                    foreach (var hand in h)
                    {
                        var evaluator = new DefaultHandEvaluator();
                        var result = evaluator.Evaluate(hand);
                    }
                });
                tasks.Add(task);
                hands.RemoveRange(0, handsPerTask);
            }
            
            Task.WaitAll(tasks.ToArray());

            stopWatch.Stop();
            Console.WriteLine($"Elapsed = {stopWatch.Elapsed}");
            var handsPerSecond = _numberOfHands / stopWatch.Elapsed.Seconds;
            Console.WriteLine($"Hands per second = {handsPerSecond}");
        }

        private static void SetupHands()
        {
            var handsPerTask = _numberOfHands / 10;
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var task = Task.Run(() =>
                {
                    var deck = new Deck();
                    for (int j = 0; j < handsPerTask; j++)
                    {
                        var hand = deck.TakeCards(5);
                        _hands.Add(hand);       
                        deck.ResetDeck();
                    }
                });    
                tasks.Add(task);
            }
            
            Task.WaitAll(tasks.ToArray());
            _hands.CompleteAdding();
        }
    }
}
