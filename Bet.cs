namespace RaceTrackApp.Business_Logic;
/// <summary>
/// single bet placed by a bettor on a specific racer
/// store amount wagered the racer number chosen and a reference to the bettor
/// </summary>
public struct Bet //struct because it's holding data
{
    //amount wagered
    private double _amount;
    //racer number (1–4) the bettor is betting on
    private int _racerNo;
    // bettor who placed this bet.
    private Bettor _bettor;
    //description of the bet
    public string Description => $"{_bettor.Name} bets ${_amount:F0} on racer {_racerNo}."; 
    public int RacerNo => _racerNo; //_racerNo was private so it couldn't be accessed from outside the struct
    /// <summary>
    /// create a new bet
    /// </summary>
    /// <param name="amount"> wager in dollars</param>
    /// <param name="raceNo"> racer 1-4 being bet on</param>
    /// <param name="bettor">the bettor placing on the wager </param>

    public Bet(double amount, int raceNo, Bettor bettor)
    {
        _amount = amount;
        _racerNo = raceNo;
        _bettor = bettor;
    }
    public double PayOut(int winner)
    //calculate payout for this bet
    //returns double the amount if the racer won, otherwise 0
    {
        return _racerNo == winner ? _amount * 2 : 0;
    }
    
}