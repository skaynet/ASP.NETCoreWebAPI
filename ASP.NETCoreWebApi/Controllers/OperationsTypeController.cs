using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.NETCoreWebApi.Data;
using ASP.NETCoreWebApi.Models;

namespace ASP.NETCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsTypeController : ControllerBase
    {
        private readonly FinanceContext _context;

        public OperationsTypeController(FinanceContext context)
        {
            _context = context;
        }

        // GET: api/OperationsType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationType>>> GetOperationsType()
        {
            return await _context.OperationsType.ToListAsync();
        }

        // GET: api/OperationsType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationType>> GetOperationType(int id)
        {
            var operationType = await _context.OperationsType.FindAsync(id);

            if (operationType == null)
            {
                return NotFound();
            }

            return operationType;
        }

        // PUT: api/OperationsType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperationType(int id, OperationType operationType)
        {
            if (id != operationType.Id)
            {
                return BadRequest();
            }

            _context.Entry(operationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperationTypeExists(id))
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

        // POST: api/OperationsType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OperationType>> PostOperationType(OperationType operationType)
        {
            operationType.Id = NextOperationTypeID();
            _context.OperationsType.Add(operationType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOperationType), new { id = operationType.Id }, operationType);
        }

        // DELETE: api/OperationsType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperationType(int id)
        {
            var operationType = await _context.OperationsType.FindAsync(id);
            if (operationType == null)
            {
                return NotFound();
            }

            _context.OperationsType.Remove(operationType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OperationTypeExists(int id)
        {
            return (_context.OperationsType?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private int NextOperationTypeID() => _context.OperationsType.Count() == 0 ? 1 : _context.OperationsType.Max(t => t.Id) + 1;
    }
}
