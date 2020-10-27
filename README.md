# PokerLib

```c#
var deck = new Deck();
var hand = deck.TakeCards(5);
var handResult = new DefaultHandEvaluator().Evaluate(hand);
```

Speed test on Intel i7-8750H 2.2GHz, 6 cores: <br />
<br /><br />
Setting up 10000000 hands...<br />
Elapsed: 00:01:12.5392532<br />
Evaluating 10000000 hands...<br />
Elapsed = 00:01:13.0291014<br />
Hands per second = 769230<br />
<br /><br />
Slow. Needs work.
