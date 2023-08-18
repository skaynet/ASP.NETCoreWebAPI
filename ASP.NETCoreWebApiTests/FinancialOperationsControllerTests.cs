using ASP.NETCoreWebApi.Controllers;
using ASP.NETCoreWebApi.Data;
using ASP.NETCoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreWebApiTests
{
    public class FinancialTransactionsControllerTests : IDisposable
    {
        private readonly DbContextOptions<FinanceContext> _dbContextOptions;

        public FinancialTransactionsControllerTests()
        {
            // Создаем в памяти базу данных для тестов
            _dbContextOptions = new DbContextOptionsBuilder<FinanceContext>().UseInMemoryDatabase(databaseName: "FinanceTestDatabase2").Options;
        }

        [Fact]
        public async Task GetFinancialTransactions_ReturnsActionResult_ListOfFinancialTransactions()
        {
            // Arrange
            List<FinancialTransaction> FinancialTransactions = TestsData.GetTestFinancialTransactions();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                //Заполняем базу данных тестовыми данными
                context.FinanciaTransactions.AddRange(FinancialTransactions);
                context.OperationsType.AddRange(TestsData.GetTestOperationTypes());
                context.SaveChanges();

                // Создаем контроллер с использованием в памяти базы данных
                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.GetFinancialTransactions();

                // Assert
                var okResult = Assert.IsType<ActionResult<IEnumerable<FinancialTransaction>>>(result);
                var FinancialTransactionsResult = Assert.IsAssignableFrom<IEnumerable<FinancialTransaction>>(okResult.Value);
                Assert.Equal(FinancialTransactions.Count, FinancialTransactionsResult.Count());
            }
        }

        [Fact]
        public async Task GetFinancialTransactions_ReturnsActionResult_WhenNoFinancialTransactionsAreAvailable()
        {
            // Arrange
            List<FinancialTransaction> FinancialTransactions = new List<FinancialTransaction>();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                //Заполняем базу данных тестовыми данными
                context.FinanciaTransactions.AddRange(FinancialTransactions);
                context.SaveChanges();

                // Создаем контроллер с использованием в памяти базы данных
                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.GetFinancialTransactions();

                // Assert
                var okResult = Assert.IsType<ActionResult<IEnumerable<FinancialTransaction>>>(result);
                var FinancialTransactionsResult = Assert.IsAssignableFrom<IEnumerable<FinancialTransaction>>(okResult.Value);
                Assert.Empty(FinancialTransactionsResult);
            }
        }

        [Fact]
        public async Task GetFinancialTransaction_ReturnsActionResult_WithIdFinancialTransaction()
        {
            // Arrange
            List<FinancialTransaction> FinancialTransactions = TestsData.GetTestFinancialTransactions();
            int id = 1;

            using (var context = new FinanceContext(_dbContextOptions))
            {
                //Заполняем базу данных тестовыми данными
                context.FinanciaTransactions.AddRange(FinancialTransactions);
                context.OperationsType.AddRange(TestsData.GetTestOperationTypes());
                context.SaveChanges();

                // Создаем контроллер с использованием в памяти базы данных
                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.GetFinancialTransaction(id);

                // Assert
                var okResult = Assert.IsType<ActionResult<FinancialTransaction>>(result);
                var FinancialTransactionsResult = Assert.IsAssignableFrom<FinancialTransaction>(okResult.Value);
                Assert.Equal(id, FinancialTransactionsResult.Id);
            }
        }

        [Fact]
        public async Task GetFinancialTransaction_ReturnsActionResult_WithIdNotFound()
        {
            // Arrange
            List<FinancialTransaction> FinancialTransactions = TestsData.GetTestFinancialTransactions();
            int id = 10;

            using (var context = new FinanceContext(_dbContextOptions))
            {
                //Заполняем базу данных тестовыми данными
                context.FinanciaTransactions.AddRange(FinancialTransactions);
                context.OperationsType.AddRange(TestsData.GetTestOperationTypes());
                context.SaveChanges();

                // Создаем контроллер с использованием в памяти базы данных
                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.GetFinancialTransaction(id);

                // Assert
                var okResult = Assert.IsType<ActionResult<FinancialTransaction>>(result);
                var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
                var FinancialTransactionsResult = okResult.Value;
                Assert.Null(FinancialTransactionsResult);
            }
        }

        [Fact]
        public async Task PutFinancialTransaction_ReturnsNoContentResult_WhenIdAndPutFinancialTransactionIdMatch()
        {
            // Arrange
            List<FinancialTransaction> FinancialTransactions = TestsData.GetTestFinancialTransactions();
            int id = 1;
            FinancialTransaction FinancialTransaction = FinancialTransactions.First();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                // Добавляем тестовые данные в базу данных
                context.FinanciaTransactions.AddRange(FinancialTransactions);
                context.SaveChanges();

                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.PutFinancialTransaction(id, TestsData.FinancialTransactionToDTO(FinancialTransaction));

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutFinancialTransaction_ReturnsBadRequestResult_WhenIdAndFinancialTransactionIdDoNotMatch()
        {
            // Arrange
            List<FinancialTransaction> FinancialTransactions = TestsData.GetTestFinancialTransactions();
            int id = 1;
            FinancialTransaction FinancialTransaction = FinancialTransactions.Last();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                // Добавляем тестовые данные в базу данных
                context.FinanciaTransactions.AddRange(FinancialTransactions);
                context.SaveChanges();

                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.PutFinancialTransaction(id, TestsData.FinancialTransactionToDTO(FinancialTransaction));

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutFinancialTransaction_ReturnsNotFoundResult_WhenFinancialTransactionDoesNotExist()
        {
            // Arrange
            int id = 1;
            FinancialTransaction FinancialTransaction = TestsData.GetTestFinancialTransactions().First();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.PutFinancialTransaction(id, TestsData.FinancialTransactionToDTO(FinancialTransaction));

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PostFinancialTransaction_ReturnsCreatedAtActionResult_WithNewFinancialTransaction()
        {
            // Arrange
            FinancialTransaction FinancialTransaction = TestsData.GetTestFinancialTransactions().Last();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.PostFinancialTransaction(TestsData.FinancialTransactionToDTO(FinancialTransaction));

                // Assert
                CreatedAtActionResult createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                Assert.Equal(nameof(FinancialTransactionsController.GetFinancialTransaction), createdAtActionResult.ActionName);
                Assert.Equal(FinancialTransaction.Id, createdAtActionResult.RouteValues["id"]);

                FinancialTransaction createdFinancialTransaction = Assert.IsType<FinancialTransaction>(createdAtActionResult.Value);
                Assert.Equal(FinancialTransaction.Description, createdFinancialTransaction.Description);
            }
        }

        [Fact]
        public async Task DeleteFinancialTransaction_ReturnsNoContent_WhenFinancialTransactionExists()
        {
            // Arrange
            FinancialTransaction FinancialTransaction = TestsData.GetTestFinancialTransactions().First();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                context.FinanciaTransactions.Add(FinancialTransaction);
                context.SaveChanges();

                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.DeleteFinancialTransaction(FinancialTransaction.Id);

                // Assert
                Assert.IsType<NoContentResult>(result);
                var deletedOperationType = await context.OperationsType.FindAsync(FinancialTransaction.Id);
                Assert.Null(deletedOperationType);
            }
        }

        [Fact]
        public async Task DeleteFinancialTransaction_ReturnsNotFound_WhenFinancialTransactionDoesNotExist()
        {
            // Arrange
            var OperationTypeId = 1;

            using (var context = new FinanceContext(_dbContextOptions))
            {
                var controller = new FinancialTransactionsController(context);

                // Act
                var result = await controller.DeleteFinancialTransaction(OperationTypeId);

                // Assert
                Assert.IsType<NotFoundResult>(result);
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
