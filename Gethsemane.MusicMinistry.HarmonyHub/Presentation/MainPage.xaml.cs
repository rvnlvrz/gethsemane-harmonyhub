namespace Gethsemane.MusicMinistry.HarmonyHub.Presentation;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    private void OnInventoryButtonClick(object sender, RoutedEventArgs e)
    {
        // Navigate to the Inventory page
        _ = this.Navigator()?.NavigateViewAsync<InventoryPage>(this);
    }
}
