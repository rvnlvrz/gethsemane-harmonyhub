using Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;
using Microsoft.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Gethsemane.MusicMinistry.HarmonyHub.Presentation;

public sealed partial class InventoryListControl : UserControl
{
    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(
            nameof(Source),
            typeof(IListFeed<InventoryItem>),
            typeof(InventoryListControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty MainCountFormatterProperty =
        DependencyProperty.Register(
            nameof(MainCountFormatter),
            typeof(IValueConverter),
            typeof(InventoryListControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty SubCountFormatterProperty =
        DependencyProperty.Register(
            nameof(SubCountFormatter),
            typeof(IValueConverter),
            typeof(InventoryListControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ItemLabelFormatterProperty =
        DependencyProperty.Register(
            nameof(ItemLabelFormatter),
            typeof(IValueConverter),
            typeof(InventoryListControl),
            new PropertyMetadata(null));

    public InventoryListControl()
    {
        InitializeComponent();
    }

    public IListFeed<InventoryItem> Source
    {
        get => (IListFeed<InventoryItem>)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public ValueTask<Option<IImmutableList<InventoryItem>>> Data => Source.Data();

    public IValueConverter MainCountFormatter
    {
        get => (IValueConverter)GetValue(MainCountFormatterProperty);
        set => SetValue(MainCountFormatterProperty, value);
    }

    public IValueConverter SubCountFormatter
    {
        get => (IValueConverter)GetValue(SubCountFormatterProperty);
        set => SetValue(SubCountFormatterProperty, value);
    }

    public IValueConverter ItemLabelFormatter
    {
        get => (IValueConverter)GetValue(ItemLabelFormatterProperty);
        set => SetValue(ItemLabelFormatterProperty, value);
    }
}
