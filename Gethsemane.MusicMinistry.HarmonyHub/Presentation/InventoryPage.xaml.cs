using Windows.UI.Core;

namespace Gethsemane.MusicMinistry.HarmonyHub.Presentation;

/// <summary>
///     An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class InventoryPage : Page
{
    public InventoryPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        // Register the event when the page is navigated to
        var manager = SystemNavigationManager.GetForCurrentView();
        manager.BackRequested += OnBackRequested!;

        _ = this.Navigator()?.NavigateRouteAsync(this, "./All Items");
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        // Unregister the event when the page is navigated away from
        var manager = SystemNavigationManager.GetForCurrentView();
        manager.BackRequested -= OnBackRequested!;
    }

    private void OnBackRequested(object sender, BackRequestedEventArgs e)
    {
        if (!Frame.CanGoBack) return;

        Frame.GoBack();
        e.Handled = true; // Indicates that the back request has been handled
    }

    private void RefreshContainer_OnRefreshRequested(RefreshContainer sender,
        RefreshRequestedEventArgs args)
    {
        var refreshCommand = AllItemsFeedView.Refresh;
        refreshCommand.Execute(sender);
    }
}
