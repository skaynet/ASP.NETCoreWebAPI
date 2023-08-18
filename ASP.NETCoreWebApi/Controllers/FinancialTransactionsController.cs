using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.NETCoreWebApi.Data;
using ASP.NETCoreWebApi.Models;

namespace ASP.NETCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialTransactionsController : ControllerBase
    {
        private readonly FinanceContext _context;

        public FinancialTransactionsController(FinanceContext context)
        {
            _context = context;
        }

        // GET: api/FinancialTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialTransaction>>> GetFinancialTransactions()
        {
            return await _context.FinanciaTransactions.Include(f => f.Type).ToListAsync();
        }

        // GET: api/FinancialTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialTransaction>> GetFinancialTransaction(int id)
        {
            var financialTransaction = await _context.FinanciaTransactions.Include(f => f.Type).FirstOrDefaultAsync(f => f.Id == id);

            if (financialTransaction == null)
            {
                return NotFound();
            }

            return financialTransaction;
        }

        // PUT: api/FinancialTransactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinancialTransaction(int id, FinancialTransactionDTO financialTransactionDTO)
        {
            if (id != financialTransactionDTO.Id)
            {
                return BadRequest();
            }

            FinancialTransaction? financialTransaction = await _context.FinanciaTransactions.FindAsync(financialTransactionDTO.Id);
            if (financialTransaction == null)
            {
                return NotFound();
            }

            financialTransaction.Description = financialTransactionDTO.Description;
            financialTransaction.Amount = financialTransactionDTO.Amount;
            financialTransaction.Date = financialTransactionDTO.Date;
            financialTransaction.TypeId = financialTransactionDTO.TypeId;

            _context.Entry(financialTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancialTransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FinancialTransactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FinancialTransaction>> PostFinancialTransaction(FinancialTransactionDTO financialTransactionDTO)
        {
            FinancialTransaction financialTransaction = DTOToFinancialTransaction(financialTransactionDTO);

            _context.FinanciaTransactions.Add(financialTransaction);
            await _context.SaveChangesAsync();

            // Загружаем связанные данные (OperationType) для добавленной финансовой операции
            await _context.Entry(financialTransaction).Reference(f => f.Type).LoadAsync();

            return CreatedAtAction(nameof(GetFinancialTransaction), new { id = financialTransaction.Id }, financialTransaction);
        }

        // DELETE: api/FinancialTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancialTransaction(int id)
        {
            var financialTransaction = await _context.FinanciaTransactions.FindAsync(id);
            if (financialTransaction == null)
            {
                return NotFound();
            }

            _context.FinanciaTransactions.Remove(financialTransaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinancialTransactionExists(int id)
        {
            return (_context.FinanciaTransactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static FinancialTransaction DTOToFinancialTransaction(FinancialTransactionDTO financialTransactionDTO) =>
            new FinancialTransaction
            {
                Id = financialTransactionDTO.Id,
                Description = financialTransactionDTO.Description,
                Amount = financialTransactionDTO.Amount,
                Date = financialTransactionDTO.Date,
                TypeId = financialTransactionDTO.TypeId
            };
    }
}
