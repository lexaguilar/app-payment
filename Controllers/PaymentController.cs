using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apppayment.Models;

namespace apppayment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly AppDbContext _context;

    public PaymentController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/payment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        return await _context.Transactions.ToListAsync();
    }

    // POST: api/payment/{id}
    [HttpPost("{id}")]
    public async Task<ActionResult> ApplyPayment(int id, [FromBody] ApplyPaymentRequest request)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
            return NotFound("Transacción no encontrada.");

        if (transaction.IsPaid)
            return BadRequest("La transacción ya está pagada.");

        transaction.Paid += request.PaymentAmount;
        transaction.CreatedBy = request.CreatedBy;
        transaction.CreatedAt = DateTime.UtcNow;

        if (transaction.Paid > transaction.Amount)
            transaction.Paid = transaction.Amount;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Pago aplicado correctamente.",
            transaction
        });
    }
}

public class ApplyPaymentRequest
{
    public decimal PaymentAmount { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}
