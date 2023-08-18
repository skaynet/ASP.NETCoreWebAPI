using ASP.NETCoreWebApi.Data;
using ASP.NETCoreWebApi.Models;
using ASP.NETCoreWebApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ASP.NETCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly FinanceContext _context;

        public ReportsController(FinanceContext context)
        {
            _context = context;
        }

        // GET api/reports/daily
        // Генерируем ежедневный отчет на основе указанной даты
        [HttpGet("daily")]
        public async Task<ActionResult<DailyReport>> GetDailyReport(string dateString)
        {
            DateTime date;
            try
            {
                date = DateTime.ParseExact(dateString, JsonDateConverter.DateFormat, CultureInfo.InvariantCulture);
            }
            catch(Exception e)
            {
                return BadRequest(new { MessageError = e.Message} );
            }

            List<FinancialTransaction> financialOperations = await _context.FinanciaTransactions.Include(f => f.Type).Where(f => f.Date == date).ToListAsync();
            
            DailyReport dailyReport = new DailyReport();
            dailyReport.Date = date;

            foreach (FinancialTransaction operation in financialOperations)
            {
                if (operation.Amount > 0)
                {
                    dailyReport.TotalIncome += operation.Amount;
                }
                else
                {
                    dailyReport.TotalExpenses += operation.Amount;
                }
                dailyReport.Transactions.Add(operation);
            }
            return dailyReport;
        }

        // GET api/reports/period
        // Генерируем отчет на основе указанного диапазона дат
        [HttpGet("period")]
        public async Task<ActionResult<PeriodReport>> GetPeriodReport(string startDateString, string endDateString)
        {
            DateTime startDate;
            DateTime endDate;

            try
            {
                startDate = DateTime.ParseExact(startDateString, JsonDateConverter.DateFormat, CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(endDateString, JsonDateConverter.DateFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return BadRequest(new { MessageError = e.Message });
            }

            List<FinancialTransaction> financialOperations = await _context.FinanciaTransactions.Include(f => f.Type).Where(f => f.Date >= startDate && f.Date <= endDate).ToListAsync();

            PeriodReport periodReport = new PeriodReport();
            periodReport.StartDate = startDate;
            periodReport.EndDate = endDate;

            foreach (FinancialTransaction operation in financialOperations)
            {
                if (operation.Amount > 0)
                {
                    periodReport.TotalIncome += operation.Amount;
                }
                else
                {
                    periodReport.TotalExpenses += operation.Amount;
                }
                periodReport.Transactions.Add(operation);
            }
            return periodReport;
        }
    }
}