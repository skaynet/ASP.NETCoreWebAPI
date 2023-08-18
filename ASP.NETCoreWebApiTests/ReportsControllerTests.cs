using ASP.NETCoreWebApi.Controllers;
using ASP.NETCoreWebApi.Data;
using ASP.NETCoreWebApi.Models;
using ASP.NETCoreWebApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ASP.NETCoreWebApiTests
{
    public class ReportsControllerTests : IDisposable
    {
        private readonly DbContextOptions<FinanceContext> _dbContextOptions;

        public ReportsControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<FinanceContext>().UseInMemoryDatabase(databaseName: "FinanceTestDatabase3").Options;
        }

        [Fact]
        public async Task GetDailyReport_ReturnsDailyReport_WhenValidDateString()
        {
            // Arrange
            string dateString = "2023-06-01";
            DateTime date = DateTime.ParseExact(dateString, JsonDateConverter.DateFormat, CultureInfo.InvariantCulture);

            List<FinancialTransaction> FinancialTransactions = TestsData.GetTestFinancialTransactions();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                context.FinanciaTransactions.AddRange(FinancialTransactions);
                context.OperationsType.AddRange(TestsData.GetTestOperationTypes());
                context.SaveChanges();

                var controller = new ReportsController(context);

                // Act
                var result = await controller.GetDailyReport(dateString);

                // Assert
                var okResult = Assert.IsType<ActionResult<DailyReport>>(result);
                DailyReport dailyReport = Assert.IsType<DailyReport>(okResult.Value);

                Assert.Equal(date, dailyReport.Date);
                Assert.Equal(8000, dailyReport.TotalIncome);
                Assert.Equal(-500, dailyReport.TotalExpenses);
                Assert.Equal(2, dailyReport.Transactions.Count);
            }
        }

        [Fact]
        public async Task GetDailyReport_ReturnsBadRequest_WhenInvalidDateString()
        {
            // Arrange
            string dateString = "2023-15-01";

            using (var context = new FinanceContext(_dbContextOptions))
            {
                var controller = new ReportsController(context);

                // Act
                var result = await controller.GetDailyReport(dateString);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.NotNull(badRequestResult.Value);
            }
        }

        [Fact]
        public async Task GetPeriodReport_ReturnsPeriodReport_WhenValidDateString()
        {
            // Arrange
            string dateStartString = "2023-06-01";
            string dateEndString = "2023-06-07";
            DateTime startate = DateTime.ParseExact(dateStartString, JsonDateConverter.DateFormat, CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(dateEndString, JsonDateConverter.DateFormat, CultureInfo.InvariantCulture);

            List<FinancialTransaction> FinancialTransactions = TestsData.GetTestFinancialTransactions();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                context.FinanciaTransactions.AddRange(FinancialTransactions);
                context.OperationsType.AddRange(TestsData.GetTestOperationTypes());
                context.SaveChanges();

                var controller = new ReportsController(context);

                // Act
                var result = await controller.GetPeriodReport(dateStartString, dateEndString);

                // Assert
                var okResult = Assert.IsType<ActionResult<PeriodReport>>(result);
                PeriodReport periodReport = Assert.IsType<PeriodReport>(okResult.Value);

                Assert.Equal(startate, periodReport.StartDate);
                Assert.Equal(endDate, periodReport.EndDate);
                Assert.Equal(9800, periodReport.TotalIncome);
                Assert.Equal(-5000, periodReport.TotalExpenses);
                Assert.Equal(7, periodReport.Transactions.Count);
            }
        }

        [Fact]
        public async Task GetPeriodReport_ReturnsBadRequest_WhenInvalidDateString()
        {
            // Arrange
            string dateStartString = "2023-06-32";
            string dateEndString = "2023-15-07";

            using (var context = new FinanceContext(_dbContextOptions))
            {
                var controller = new ReportsController(context);

                // Act
                var result = await controller.GetPeriodReport(dateStartString, dateEndString);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.NotNull(badRequestResult.Value);
            }
        }

        public void Dispose()
        {
            using (var context = new FinanceContext(_dbContextOptions))
            {
                // Удаляем базу данных после выполнения каждого тестового метода
                context.Database.EnsureDeleted();
            }
        }
    }
}
