namespace RaceTrackApp.Business_Logic;
/// <summary>
/// Represents a racer in the race simulation
/// Tracks position on the racetrack and advances using random movement
/// </summary>
public class Racer
{
    private double _startPosition; //pixel start position. left side of the track
    private double _location; //horizontal location of the track
    private double _raceTrackLength; //length of the racetrack 
    private Random _randomizer; //random generator for movement
    private Image _racerUI; //racer control representing the racer on the screen
    public Racer(Image racerUI) //initialize new racer. the maui image displayed on the track
    {
        _racerUI = racerUI;
        _randomizer = new Random();
        _startPosition = 0;
        _location = 0;
        _raceTrackLength = 0;
    }
    
    public void TakeStartingPosition() //sets the racer up to the far left of the track.
    {
        _location = _startPosition; //called when a race begins or is restarted
        AbsoluteLayout.SetLayoutBounds(_racerUI, new Rect(_location, AbsoluteLayout.GetLayoutBounds(_racerUI).Y, 60, 40));
    } 
    public bool Run()
    {
        double step = _randomizer.NextDouble() * 15 + 1; 
        _location += step; //advances the racer a random distance forward on the track

        var bounds = AbsoluteLayout.GetLayoutBounds(_racerUI);
        AbsoluteLayout.SetLayoutBounds(_racerUI, new Rect(_location, bounds.Y, bounds.Width, bounds.Height));

        return _location >= _raceTrackLength; //returns true if the racer has crossed the finish line
    }
    public void SetTrackLength(double length) //sets the track length so the racer knows where the finish line is
    {
        _raceTrackLength = length;
    }


}