namespace Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;

public partial record BorrowerDto
{
    public string? BorrowerId { get; set; }
    public int BorrowedQuantity { get; set; }
    public DateTime DueDate { get; set; }
}
