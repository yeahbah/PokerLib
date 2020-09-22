namespace Yeahbah.Poker
{
    public interface IDealer
    {
        Card[] DealHand();

        void Shuffle();
    }
}
