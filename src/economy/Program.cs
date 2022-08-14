namespace Economy;

public static class Program
{
    public static int Main()
    {
        Market market = new ();
        market.Run();
        return 0;
    }
}

internal class Market
{
    private List<IParticipant> participants = new ();
    private IBuyer buyer;
    private ISeller seller;

    public Market()
    {
        Buyer buyer = new ();
        participants.Add(buyer);
        this.buyer = buyer;

        Seller seller = new ();
        participants.Add(seller);
        this.seller = seller;
    }

    public void Run()
    {
        seller.BeginTransaction(buyer);
    }
}

internal interface IParticipant
{
    void DayEnd();
}

internal interface IBuyer
{
    void MakeOffer(IOffer offerer);
}

internal interface IOffer
{
    int Price { get; }
    void Accept();
    void Reject();
}

internal interface ISeller
{
    void BeginTransaction(IBuyer buyer);
}

internal class Seller : ISeller, IParticipant, IOffer
{
    private int widgets;
    private int money = 0;

    public Seller() =>
        (this as IParticipant).DayEnd();

    void IParticipant.DayEnd()
    {
        widgets = 100;
    }

    void ISeller.BeginTransaction(IBuyer buyer)
    {
        buyer.MakeOffer(this);
    }

    int IOffer.Price => 10;

     void IOffer.Accept()
    {
        widgets--;
        money += (this as IOffer).Price;
    }

    void IOffer.Reject() { }
}

internal class Buyer : IBuyer, IParticipant
{
    private int widgets = 0;
    private int money = 10;

    public Buyer() =>
        (this as IParticipant).DayEnd();

    void IParticipant.DayEnd() =>
        money = 10;

    void IBuyer.MakeOffer(IOffer offer)
    {
        if (offer.Price <= money)
        {
            offer.Accept();
            money -= offer.Price;
            widgets++;
        }
        else
            offer.Reject();
    }
}
