namespace RaceTrackApp.Business_Logic;

public class RaceTrackSimulator
{
        //Internal list of all registered racers
        private List<Racer> _racers = new();

        // Internal list of all registered bettors
        private List<Bettor> _bettors = new();

        //The bettor currently selected in the UI
        private Bettor _currentBettor;

        // Gets the read-only list of bettors
        public IReadOnlyList<Bettor> Bettors => _bettors;

        //Gets the read-only list of racers
        public IReadOnlyList<Racer> Racers => _racers;

        //Gets or sets the currently selected bettor
        public Bettor CurrentBettor
        {
            get => _currentBettor;
            set => _currentBettor = value;
        }

       
        // Initializes a new RaceTrackSimulator with empty racer and bettor lists
        
        public RaceTrackSimulator()
        {
            _currentBettor = null!;
        }

        /// <summary>
        /// Registers a racer into the simulation.
        /// </summary>
        /// <param name="racer">The Racer object to add.</param>
        public void RegisterRacer(Racer racer)
        {
            _racers.Add(racer);
        }

        /// <summary>
        /// Adds a bettor to the simulation.
        /// </summary>
        /// <param name="bettor">The Bettor object to register.</param>
        public void AddBettor(Bettor bettor)
        {
            _bettors.Add(bettor);
        }
}