using Microsoft.EntityFrameworkCore;
using ToDoApplication.Infrastructure;
using ToDoApplication.Infrastructure.Repositories;

namespace ToDoApplication.Tests.Repositories
{
    public class TaskStatusesRepositoryTests
    {
        private AppDbContext dbContext;
        private TaskStatusesRepository repository;

        private static IEnumerable<TestCaseData> StatusesToTests
        {
            get
            {
                yield return new TestCaseData(
                    new TaskStatuses()
                    {
                        Name = "Not started",
                        Description = "Waiting to start.",
                    });
                yield return new TestCaseData(
                    new TaskStatuses()
                    {
                        Name = "In progress.",
                        Description = "Started but not ended.",
                    });
            }
        }

        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase");

            this.dbContext = new AppDbContext(dbContextOptions.Options);
            this.dbContext.Database.EnsureCreated();

            this.dbContext.TaskStatuses.AddRange(
                new TaskStatuses()
                {
                    Name = "Completed!",
                    Description = "Ended.",
                },
                new TaskStatuses()
                {
                    Name = "Abandoned",
                    Description = string.Empty,
                });
            this.dbContext.SaveChanges();

            this.repository = new TaskStatusesRepository(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
        }

        [TestCaseSource(nameof(StatusesToTests))]
        public async Task AddAsync_ShouldAddItemToDbContext(TaskStatuses status)
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskStatuses.CountAsync();

            // Act
            var result = await this.repository.AddAsync(status);
            var getStatusFromDatabase = await this.dbContext.TaskStatuses
                .FirstOrDefaultAsync(x => x.Id == status.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskStatuses.CountAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(countItemsInDatabase + 1, Is.EqualTo(countItemsInDatabaseAfter));
                Assert.That(result, Is.EqualTo(status.Id));
                Assert.That(getStatusFromDatabase?.Id, Is.EqualTo(status.Id));
                Assert.That(getStatusFromDatabase?.Name, Is.EqualTo(status.Name));
                Assert.That(getStatusFromDatabase?.Description, Is.EqualTo(status.Description));
            });
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveItemFromDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskStatuses.CountAsync();
            var getStatusFromDatabase = this.dbContext.TaskStatuses.First();

            // Act
            var result = await this.repository.DeleteAsync(getStatusFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskStatuses.CountAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(countItemsInDatabase - 1, Is.EqualTo(countItemsInDatabaseAfter));
                Assert.That(result, Is.EqualTo(getStatusFromDatabase.Id));
                Assert.That(this.dbContext.TaskStatuses.Find(getStatusFromDatabase.Id), Is.Null);
            });
        }

        [Test]
        public void DeleteAsync_ThrowsArgumentNullExceptionWhenItemNotFound()
        {
            // Arrange
            var id = 1001;

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => this.repository.DeleteAsync(id),
                $"Given status with {id}, can not be found in database.");
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingItemInDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskStatuses.CountAsync();
            var newStatus = new TaskStatuses()
            {
                Name = "Completed",
                Description = "Task done."
            };
            var getStatusFromDatabase = this.dbContext.TaskStatuses.First();

            // Act
            var result = await this.repository.UpdateAsync(getStatusFromDatabase.Id, newStatus);
            var retrievedStatusCategory = this.dbContext.TaskStatuses.Find(getStatusFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskStatuses.CountAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(countItemsInDatabase, Is.EqualTo(countItemsInDatabaseAfter));
                Assert.That(result, Is.EqualTo(retrievedStatusCategory?.Id));
                Assert.That(newStatus.Name, Is.EqualTo(retrievedStatusCategory?.Name));
                Assert.That(newStatus.Description, Is.EqualTo(retrievedStatusCategory?.Description));
            });
        }

        [Test]
        public void UpdateAsync_ThrowsNullReferenceExceptionWhenItemNotFound()
        {
            // Arrange
            var newStatus = new TaskStatuses()
            {
                Name = "Completed",
                Description = "Task done."
            };
            var id = 1001;

            // Asserts
            Assert.ThrowsAsync<NullReferenceException>(
                () => this.repository.UpdateAsync(id, newStatus),
                $"Given status with {id}, can not be found in database.");
        }

        [Test]
        public async Task GetByNameAsync_ShouldReturnTaskStatusWithMatchingName()
        {
            // Arrange
            var getStatusFromDatabase = this.dbContext.TaskStatuses.First();

            // Act
            var result = await this.repository.GetByNameAsync(getStatusFromDatabase.Name);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(getStatusFromDatabase.Id));
                Assert.That(result.Name, Is.EqualTo(getStatusFromDatabase.Name));
                Assert.That(result.Description, Is.EqualTo(getStatusFromDatabase.Description));
            });
        }

        [Test]
        public async Task GetByNameAsync_ShouldReturnNull()
        {
            // Arrange
            var name = "asdfsdf";

            // Act
            var result = await this.repository.GetByNameAsync(name);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectItem()
        {
            // Arrange
            var getStatusFromDatabase = this.dbContext.TaskStatuses.First();

            // Act
            var result = await this.repository.GetByIdAsync(getStatusFromDatabase.Id);

            // Assert
            Assert.That(result, Is.EqualTo(getStatusFromDatabase));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull()
        {
            // Arrange
            var id = 1001;

            // Act
            var result = await this.repository.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}