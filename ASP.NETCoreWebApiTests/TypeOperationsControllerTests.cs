using ASP.NETCoreWebApi.Controllers;
using ASP.NETCoreWebApi.Data;
using ASP.NETCoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreWebApiTests
{
    public class OperationTypesControllerTests : IDisposable
    {
        private readonly DbContextOptions<FinanceContext> _dbContextOptions;

        public OperationTypesControllerTests()
        {
            // —оздаем в пам€ти базу данных дл€ тестов
            _dbContextOptions = new DbContextOptionsBuilder<FinanceContext>().UseInMemoryDatabase(databaseName: "FinanceTestDatabase").Options;
        }

        [Fact]
        public async Task GetOperationsType_ReturnsActionResult_ListOfOperationsType()
        {
            // Arrange
            List<OperationType> OperationTypes = TestsData.GetTestOperationTypes();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                //«аполн€ем базу данных тестовыми данными
                context.OperationsType.AddRange(OperationTypes);
                context.SaveChanges();

                // —оздаем контроллер с использованием в пам€ти базы данных
                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.GetOperationsType();

                // Assert
                var okResult = Assert.IsType<ActionResult<IEnumerable<OperationType>>>(result);
                var OperationTypesResult = Assert.IsAssignableFrom<IEnumerable<OperationType>>(okResult.Value);
                Assert.Equal(OperationTypes.Count, OperationTypesResult.Count());
            }
        }

        [Fact]
        public async Task GetOperationsType_ReturnsActionResult_WhenNoOperationsTypeAreAvailable()
        {
            // Arrange
            List<OperationType> OperationTypes = new List<OperationType>();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                //«аполн€ем базу данных тестовыми данными
                context.OperationsType.AddRange(OperationTypes);
                context.SaveChanges();

                // —оздаем контроллер с использованием в пам€ти базы данных
                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.GetOperationsType();

                // Assert
                var okResult = Assert.IsType<ActionResult<IEnumerable<OperationType>>>(result);
                var OperationTypesResult = Assert.IsAssignableFrom<IEnumerable<OperationType>>(okResult.Value);
                Assert.Empty(OperationTypesResult);
            }
        }

        [Fact]
        public async Task GetOperationType_ReturnsActionResult_WithIdOperationType()
        {
            // Arrange
            List<OperationType> OperationTypes = TestsData.GetTestOperationTypes();
            int id = 1;

            using (var context = new FinanceContext(_dbContextOptions))
            {
                //«аполн€ем базу данных тестовыми данными
                context.OperationsType.AddRange(OperationTypes);
                context.SaveChanges();

                // —оздаем контроллер с использованием в пам€ти базы данных
                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.GetOperationType(id);

                // Assert
                var okResult = Assert.IsType<ActionResult<OperationType>>(result);
                var OperationTypesResult = Assert.IsAssignableFrom<OperationType>(okResult.Value);
                Assert.Equal(id, OperationTypesResult.Id);
            }
        }

        [Fact]
        public async Task GetOperationType_ReturnsActionResult_WithIdNotFound()
        {
            // Arrange
            List<OperationType> OperationTypes = TestsData.GetTestOperationTypes();
            int id = 10;

            using (var context = new FinanceContext(_dbContextOptions))
            {
                //«аполн€ем базу данных тестовыми данными
                context.OperationsType.AddRange(OperationTypes);
                context.SaveChanges();

                // —оздаем контроллер с использованием в пам€ти базы данных
                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.GetOperationType(id);

                // Assert
                var okResult = Assert.IsType<ActionResult<OperationType>>(result);
                var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
                var OperationTypesResult = okResult.Value;
                Assert.Null(OperationTypesResult);
            }
        }

        [Fact]
        public async Task PutOperationType_ReturnsNoContentResult_WhenIdAndOperationTypeIdMatch()
        {
            // Arrange
            List<OperationType> OperationTypes = TestsData.GetTestOperationTypes();
            int id = 1;
            OperationType OperationType = OperationTypes.First();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                // ƒобавл€ем тестовые данные в базу данных
                context.OperationsType.AddRange(OperationTypes);
                context.SaveChanges();

                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.PutOperationType(id, OperationType);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutOperationType_ReturnsBadRequestResult_WhenIdAndOperationTypeIdDoNotMatch()
        {
            // Arrange
            List<OperationType> OperationTypes = TestsData.GetTestOperationTypes();
            int id = 1;
            OperationType OperationType = OperationTypes.Last();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                // ƒобавл€ем тестовые данные в базу данных
                context.OperationsType.AddRange(OperationTypes);
                context.SaveChanges();

                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.PutOperationType(id, OperationType);

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutOperationType_ReturnsNotFoundResult_WhenOperationTypeDoesNotExist()
        {
            // Arrange
            int id = 1;
            OperationType OperationType = TestsData.GetTestOperationTypes().First();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.PutOperationType(id, OperationType);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PostOperationType_ReturnsCreatedAtActionResult_WithNewOperationType()
        {
            // Arrange
            OperationType OperationType = TestsData.GetTestOperationTypes().Last();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.PostOperationType(OperationType);

                // Assert
                CreatedAtActionResult createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                Assert.Equal(nameof(OperationsTypeController.GetOperationType), createdAtActionResult.ActionName);
                Assert.Equal(OperationType.Id, createdAtActionResult.RouteValues["id"]);

                OperationType createdOperationType = Assert.IsType<OperationType>(createdAtActionResult.Value);
                Assert.Equal(OperationType.Name, createdOperationType.Name);
            }
        }

        [Fact]
        public async Task DeleteOperationType_ReturnsNoContent_WhenOperationTypeExists()
        {
            // Arrange
            OperationType OperationType = TestsData.GetTestOperationTypes().First();

            using (var context = new FinanceContext(_dbContextOptions))
            {
                context.OperationsType.Add(OperationType);
                context.SaveChanges();

                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.DeleteOperationType(OperationType.Id);

                // Assert
                Assert.IsType<NoContentResult>(result);
                var deletedOperationType = await context.OperationsType.FindAsync(OperationType.Id);
                Assert.Null(deletedOperationType);
            }
        }

        [Fact]
        public async Task DeleteOperationType_ReturnsNotFound_WhenOperationTypeDoesNotExist()
        {
            // Arrange
            var OperationTypeId = 1;

            using (var context = new FinanceContext(_dbContextOptions))
            {
                var controller = new OperationsTypeController(context);

                // Act
                var result = await controller.DeleteOperationType(OperationTypeId);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        public void Dispose()
        {
            using (var context = new FinanceContext(_dbContextOptions))
            {
                // ”дал€ем базу данных после выполнени€ каждого тестового метода
                context.Database.EnsureDeleted();
            }
        }
    }
}