namespace RaceTrackApp.Business_Logic;

public class BetRecord
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string BettorName { get; set; } = "";
    public int RacerNo { get; set; }
    public double Amount { get; set; }
    public int WinnerRacerNo { get; set; }
    public bool Won => RacerNo == WinnerRacerNo;
    public double CashAfter { get; set; }

    public string Summary =>
        $"{Timestamp:HH:mm:ss} - {BettorName} bet ${Amount:0.00} on racer {RacerNo} | Winner: {WinnerRacerNo} | {(Won ? "WON" : "LOST")} | Cash: ${CashAfter:0.00}";
}
