namespace apppayment.Models;
public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public decimal Paid { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public bool IsPaid => Paid >= Amount;
}