using System.Timers;
using Timer = System.Timers.Timer;

namespace Gethsemane.MusicMinistry.HarmonyHub.Converters;

public class TimeUpdater
{
    private Timer _timer;

    public event Action TimeUpdated;

    public TimeUpdater()
    {
        // Initialize the timer to tick every second
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.Start();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // Trigger an update
        TimeUpdated?.Invoke();
    }
}
