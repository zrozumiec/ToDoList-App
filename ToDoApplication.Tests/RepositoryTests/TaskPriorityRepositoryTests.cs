using Microsoft.EntityFrameworkCore;
using ToDoApplication.Infrastructure;
using ToDoApplication.Infrastructure.Repositories;

namespace ToDoApplication.Tests.Repositories
{
    public class TaskPriorityRepositoryTests
    {
        private AppDbContext dbContext;
        private TaskPriorityRepository repository;

        private static IEnumerable<TestCaseData> PrioritiesToTests
        {
            get
            {
                yield return new TestCaseData(
                    new TaskPriority()
                    {
                        Name = "Low",
                    });
                yield return new TestCaseData(
                    new TaskPriority()
                    {
                        Name = "Medium",
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

            this.dbContext.TaskPriorities.AddRange(
                new TaskPriority()
                {
                    Name = "Very low",
                },
                new TaskPriority()
                {
                    Name = "Very hight",
                });
            this.dbContext.SaveChanges();

            this.repository = new TaskPriorityRepository(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
        }

        [TestCaseSource(nameof(PrioritiesToTests))]
        public async Task AddAsync_ShouldAddItemToDbContext(TaskPriority priority)
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskPriorities.CountAsync();

            // Act
            var result = await this.repository.AddAsync(priority);
            var getPriorityFromDatabase = await this.dbContext.TaskPriorities
                                                    .FirstOrDefaultAsync(x => x.Id == priority.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskPriorities.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase + 1, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(priority.Id));
            Assert.That(getPriorityFromDatabase?.Id, Is.EqualTo(priority.Id));
            Assert.That(getPriorityFromDatabase?.Name, Is.EqualTo(priority.Name));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveItemFromDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskPriorities.CountAsync();
            var getPriorityFromDatabase = this.dbContext.TaskPriorities.First();

            // Act
            var result = await this.repository.DeleteAsync(getPriorityFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskPriorities.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase - 1, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(getPriorityFromDatabase.Id));
            Assert.Null(this.dbContext.TaskPriorities.Find(getPriorityFromDatabase.Id));
        }

        [Test]
        public void DeleteAsync_ThrowsArgumentNullExceptionWhenItemNotFound()
        {
            // Arrange
            var id = 1001;

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => this.repository.DeleteAsync(id),
                $"Given priority with {id}, can not be found in database.");
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingItemInDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskPriorities.CountAsync();
            var newPriority = new TaskPriority()
            {
                Name = "High",
            };
            var getPriorityFromDatabase = this.dbContext.TaskPriorities.First();

            // Act
            var result = await this.repository.UpdateAsync(getPriorityFromDatabase.Id, newPriority);
            var retrievedTaskPriority = this.dbContext.TaskPriorities.Find(getPriorityFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskPriorities.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(retrievedTaskPriority?.Id));
            Assert.That(newPriority.Name, Is.EqualTo(retrievedTaskPriority?.Name));
        }

        [Test]
        public void UpdateAsync_ThrowsNullReferenceExceptionWhenItemNotFound()
        {
            // Arrange
            var newPriority = new TaskPriority()
            {
                Name = "High",
            };
            var id = 1001;

            // Asserts
            Assert.ThrowsAsync<NullReferenceException>(
                () => this.repository.UpdateAsync(id, newPriority),
                $"Given priority with {id}, can not be found in database.");
        }

        [Test]
        public async Task GetByNameAsync_ShouldReturnTaskPriorityWithMatchingName()
        {
            // Arrange
            var getPriorityFromDatabase = this.dbContext.TaskPriorities.First();

            // Act
            var result = await this.repository.GetByNameAsync(getPriorityFromDatabase.Name);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(getPriorityFromDatabase.Id));
            Assert.That(result.Name, Is.EqualTo(getPriorityFromDatabase.Name));
        }

        [Test]
        public async Task GetByNameAsync_ShouldReturnNull()
        {
            // Arrange
            var name = "asdfsdf";

            // Act
            var result = await this.repository.GetByNameAsync(name);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectItem()
        {
            // Arrange
            var getPriorityFromDatabase = this.dbContext.TaskPriorities.First();

            // Act
            var result = await this.repository.GetByIdAsync(getPriorityFromDatabase.Id);

            // Assert
            Assert.That(result, Is.EqualTo(getPriorityFromDatabase));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull()
        {
            // Arrange
            var id = 1001;

            // Act
            var result = await this.repository.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }
    }
}