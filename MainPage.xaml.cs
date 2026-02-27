using RaceTrackApp.Business_Logic; //need this to access the classes in the business logic folder

namespace RaceTrackApp;

public partial class MainPage : ContentPage
{
    internal RaceTrackSimulator _sim = null!; //declared sim as field so all the methods can accessthe simulator. null tells the program the user will set it before using it
    private IDispatcherTimer? _timer; //timer runs the animation, it moves the dogs. the "?" is so that it starts as null untul the race starts
    private double _minBet = 5.0; //$5 is the minimum bet amount 
    public MainPage()
    {
        InitializeComponent(); //creates all the controls defined in mainpage.xaml
        CreateRacers(); //set up the 4 racers
        CreateBettors(); //set up the raccers who will bet
    }

    /// <summary>
    /// Creates 4 racer objects and registers them in the simulator
    /// </summary>
    private void CreateRacers()
    {
        _sim = new RaceTrackSimulator(); //create a new simualtor object
        //pass each image into the constructor so the racer knows which image to move when run is instructed
        _sim.RegisterRacer(new Racer(_imgRacer1));
        _sim.RegisterRacer(new Racer(_imgRacer2));
        _sim.RegisterRacer(new Racer(_imgRacer3));
        _sim.RegisterRacer(new Racer(_imgRacer4));
    }

    
    //creates 3 bettor objects and adds them to the simulator
    private void CreateBettors() //each bettor starts with $100
    {
        _sim.AddBettor(new Bettor("Joe",  100)); 
        _sim.AddBettor(new Bettor("Sam",  100));
        _sim.AddBettor(new Bettor("Alex", 100));

        // shows radio button labels to show names
        UpdateBettorLabels();

        //select joe as bettor by default
        _rbtnBettor1.IsChecked = true;
        _sim.CurrentBettor = _sim.Bettors[0];
    }
    
    // Updates the radio button labels to show each bettor's name and cash
   
    private void UpdateBettorLabels() //:0 rounds the cash to a whole number
    { //access each bettor by index from the simulator bettors list
        _rbtnBettor1.Content = $"{_sim.Bettors[0].Name} (${_sim.Bettors[0].Cash:0})";
        _rbtnBettor2.Content = $"{_sim.Bettors[1].Name} (${_sim.Bettors[1].Cash:0})";
        _rbtnBettor3.Content = $"{_sim.Bettors[2].Name} (${_sim.Bettors[2].Cash:0})";
    }

    // Fires when user selects a different bettor with the radio buttons
    
    private void OnSelectBettor(object sender, EventArgs e) //moves when user clocks button to select different bettor
    {  //check which button is clicked and match the bettor as current
        if (_rbtnBettor1.IsChecked)   
            _sim.CurrentBettor = _sim.Bettors[0];
        else if (_rbtnBettor2.IsChecked)
            _sim.CurrentBettor = _sim.Bettors[1];
        else                            
            _sim.CurrentBettor = _sim.Bettors[2];
    }

    
    //moves when user clicks place bet
    //reads the amount and racer picker for the selected bettor and places the bet
   
    private async void OnPlaceBet(object sender, EventArgs e)
    {
        try
        { //figure out which bettor index (0, 1, or 2) is selected ie. the first second or third bettor
            int index = _rbtnBettor1.IsChecked ? 0 : _rbtnBettor2.IsChecked ? 1 : 2;
            var bettor = _sim.Bettors[index];
            //arryas to acces the right controls using same index instead of writing if and else statements for each player
            Entry[] entries   = { _txtBet1, _txtBet2, _txtBet3 };
            Picker[] pickers  = { _pickerRacer1, _pickerRacer2, _pickerRacer3 };
            Label[] descLabels = { _lblBetDesc1, _lblBetDesc2, _lblBetDesc3 };
            
            //validate the user typed a number — tryParse returns false if it's not valid
            if (!double.TryParse(entries[index].Text, out double amount))
                throw new SimulatorException("Please enter a valid amount.");
            //make sure the user picked a racer 
            if (pickers[index].SelectedIndex < 0)
                throw new SimulatorException("Please select a racer.");
            
            //picker index are 0-based but racer numbers are 1-based, so I add 1
            int racerNo = pickers[index].SelectedIndex + 1;

            bettor.PlaceBet(amount, racerNo, _minBet);

            //show bet description label
            descLabels[index].Text = bettor.CurrentBet?.Description ?? "";
            UpdateBettorLabels();
            _lblStatus.Text = $"{bettor.Name} placed a bet!";
        }
        catch (SimulatorException ex)
        { //custom exception. sends user friendly message
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    
    //moves when the user clicks the start button
    //checks all bets are placed then starts the race timer
    
    private async void OnStartRace(object sender, EventArgs e)
    {
        try
        {
            //make sure all 3 bettors have placed a bet by looping through all of them
            foreach (var bettor in _sim.Bettors)
                if (!bettor.HasPlacedBet)
                    throw new SimulatorException($"{bettor.Name} hasn't placed a bet yet!");

            //calculate tracklength from actual width of absolute layout. subtract 80 to leave room for finish line
            double trackLength = _trackLayout.Width - 80;
            //tell each racer how long the track is and move them to the starting line
            foreach (var racer in _sim.Racers)
            {
                racer.SetTrackLength(trackLength);
                racer.TakeStartingPosition();
            }
           
            //create a timer that fires every 50ms making the dogs move smoothly accross the line
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += OnTimerTick;
            _timer.Start();

            _lblStatus.Text = "Race started!";
        }
        catch (SimulatorException ex)
        {
            DisplayAlert("Cannot Start", ex.Message, "OK");
        }
    }

    
    //moves for every timer tick. moves each racer forward
    //stops when the first racer crosses the finish line
   
    private void OnTimerTick(object? sender, EventArgs e)
    {
        int winner = -1; // -1 means no winner yet
        //move every racer forward — Run() returns true if that racer finished
        for (int i = 0; i < _sim.Racers.Count; i++)
        {
            bool finished = _sim.Racers[i].Run();
            //only record the first racer to finish winner stays -1 after that
            if (finished && winner < 0)
                winner = i;
        }
        // if there is a winner, stop the race and calculate results
        if (winner >= 0)
        {
            _timer!.Stop();
            int winnerNo = winner + 1;

            // Collect winnings for all bettors
            foreach (var bettor in _sim.Bettors)
                bettor.Collect(winnerNo);

            UpdateBettorLabels();
            _lblStatus.Text = $"🏁 Racer {winnerNo} wins!";
        }
    }

    //resets all racers and clears all bets so a new race can begin
    private void OnRestart(object sender, EventArgs e)
    {
        _timer?.Stop();

        foreach (var racer in _sim.Racers)
            racer.TakeStartingPosition();

        foreach (var bettor in _sim.Bettors)
            bettor.ClearBet();
        
        // clear the bet description labels
        _lblBetDesc1.Text = "";
        _lblBetDesc2.Text = "";
        _lblBetDesc3.Text = "";

        UpdateBettorLabels();
        _lblStatus.Text = "Place your bets!";
    }

    //navigates to the Analytics page. use shell to move between pages
    private async void OnGoToAnalytics(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AnalyticsPage));
    }
}

//first commit