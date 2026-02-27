namespace RaceTrackApp.Business_Logic;

public class Bettor
{
    public string Name { get; private set; }
    public double Cash { get; private set; }

    public bool HasPlacedBet => _hasPlacedBet;
    public Bet? CurrentBet => _hasPlacedBet ? _bet : null;
    public List<string> BetHistory => _betHistory;

    private Bet _bet;
    private bool _hasPlacedBet;
    private List<string> _betHistory = new(); //history lists so analyztics can recall

    public Bettor(string name, double cash)
    {
        Name = name;
        Cash = cash;
    }

    public void SetName(string name)
    {
        Name = string.IsNullOrWhiteSpace(name) ? Name : name.Trim();
    }

    public void ResetCash(double cash)
    {
        Cash = cash;
        ClearBet();
    }

    public void PlaceBet(double amount, int racerNo, double minBet)
    {
        if (racerNo < 1 || racerNo > 4)
            throw new SimulatorException("Select a racer (1–4).");

        if (amount <= 0)
            throw new SimulatorException("Bet amount must be positive.");

        if (amount < minBet)
            throw new SimulatorException($"Minimum bet is ${minBet:0.00}.");

        if (amount > Cash)
            throw new SimulatorException($"{Name} cannot bet more than their cash (${Cash:0.00}).");

        _bet = new Bet(amount, racerNo, this);
        _hasPlacedBet = true;
    }

    public void ClearBet()
    {
        _bet = default;
        _hasPlacedBet = false;
    }

    public double Collect(int winnerRacerNo)
    {
        if (!_hasPlacedBet)
            return 0;

        var delta = _bet.PayOut(winnerRacerNo);
        Cash += delta;
        _hasPlacedBet = false;
        _betHistory.Add($"{Name} bet on racer {_bet.RacerNo} — {(delta > 0 ? "Won" : "Lost")} — Cash: ${Cash:0}");
        return delta;
    }

}