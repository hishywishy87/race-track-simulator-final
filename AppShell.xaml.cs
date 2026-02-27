namespace RaceTrackApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Register pages so Shell.GoToAsync  works
		Routing.RegisterRoute(nameof(OptionsPage),   typeof(OptionsPage));
		Routing.RegisterRoute(nameof(AnalyticsPage), typeof(AnalyticsPage));
	}
	
}
