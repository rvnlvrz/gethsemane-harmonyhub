namespace Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;

public class Borrower
{
    public string Id;
    public int BorrowedQuantity;
    public DateTime DueDate;

    public Borrower(string id, int borrowedQuantity, DateTime? dueDate)
    {
        Id = id;
        BorrowedQuantity = borrowedQuantity;
        DueDate = dueDate ??= DateTime.Now.AddDays(14);
    }
}
