using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceTrackApp;

public partial class AnalyticsPage : ContentPage
{
    public AnalyticsPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Every time this page opens, reload the bet history from the main page
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Get the main page so we can read the simulator's bettor list
        var mainPage = Application.Current?.Windows[0]?.Page as MainPage;
        if (mainPage == null) return;

        // Build a flat list of all bets from all bettors
        var items = new List<BetHistoryItem>();

        foreach (var bettor in mainPage._sim.Bettors)
        
            foreach (var record in bettor.BetHistory)
            {
                items.Add(new BetHistoryItem
                {
                    Description = record,
                    Result      = "",
                    CashAfter   = ""
                });
            }
        

        if (items.Count == 0)
            items.Add(new BetHistoryItem { Description = "No bets yet.", Result = "", CashAfter = "" });

        _historyList.ItemsSource = items;
    }

    private async void OnBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}

/// <summary>
/// Simple data model for one row in the bet history list
/// </summary>
public class BetHistoryItem
{
    public string Description { get; set; } = "";
    public string Result      { get; set; } = "";
    public string CashAfter   { get; set; } = "";
}



