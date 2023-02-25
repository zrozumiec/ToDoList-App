using Microsoft.EntityFrameworkCore;
using ToDoApplication.Infrastructure;
using ToDoApplication.Infrastructure.Repositories;

namespace ToDoApplication.Tests.Repositories
{
    public class ToDoTaskRepositoryTests
    {
        private AppDbContext dbContext;
        private ToDoTaskRepository repository;

        private static IEnumerable<TestCaseData> ToDoTasksToTests
        {
            get
            {
                yield return new TestCaseData(
                    new ToDoTask()
                    {
                        Tile = "Task to add",
                        Description = "Task to add",
                        DueDate = DateTimeOffset.UtcNow,
                        Reminder = false,
                        Daily = true,
                        Important = true,
                        IsCompleted = true,
                        ListId = 1,
                    });
                yield return new TestCaseData(
                    new ToDoTask()
                    {
                        Tile = "Task to add 2",
                        Description = "Task to add",
                        DueDate = DateTimeOffset.UtcNow,
                        Reminder = false,
                        Daily = false,
                        Important = false,
                        IsCompleted = false,
                        ListId = 1,
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
            this.dbContext.ToDoLists.Add(
                new ToDoList()
                {
                    Id = 100,
                    CreationDate = DateTimeOffset.UtcNow,
                    Tile = "My list",
                    Tasks = new List<ToDoTask>()
                    {
                        new ToDoTask()
                        {
                            Tile = "ToDoTask1 Db",
                            Description = "Test ToDoTask1 Db description",
                            DueDate = DateTimeOffset.UtcNow,
                            Reminder = false,
                            ReminderDate = DateTimeOffset.UtcNow,
                            Daily = true,
                            Important = true,
                            IsCompleted = true,
                            ListId = 100,
                        },
                        new ToDoTask()
                        {
                            Tile = "ToDoTask2 Db",
                            Description = "Test ToDoTask2 Db description",
                            DueDate = DateTimeOffset.UtcNow,
                            Reminder = false,
                            ReminderDate = DateTimeOffset.UtcNow,
                            Daily = true,
                            Important = true,
                            IsCompleted = true,
                            ListId = 100,
                        },
                        new ToDoTask()
                        {
                            Tile = "ToDoTask3 Db",
                            Description = "Test ToDoTask3 Db description",
                            DueDate = DateTimeOffset.UtcNow,
                            Reminder = false,
                            ReminderDate = DateTimeOffset.UtcNow,
                            Daily = true,
                            Important = true,
                            IsCompleted = true,
                            ListId = 100,
                        },
                    }
                });
            this.dbContext.SaveChanges();
            this.dbContext.ToDoTasks.AddRange(
                new ToDoTask()
                {
                    Tile = "ToDoTask4 Db",
                    Description = "Test ToDoTask4 Db description",
                    DueDate = DateTimeOffset.UtcNow,
                    Reminder = false,
                    ReminderDate = DateTimeOffset.UtcNow,
                    Daily = true,
                    Important = true,
                    IsCompleted = true,
                    ListId = 2,
                },
                new ToDoTask()
                {
                    Tile = "ToDoTask5 Db",
                    Description = "Test ToDoTask5 Db description",
                    DueDate = DateTimeOffset.UtcNow,
                    Reminder = false,
                    ReminderDate = DateTimeOffset.UtcNow,
                    Daily = true,
                    Important = true,
                    IsCompleted = true,
                    ListId = 3,
                });
            this.dbContext.SaveChanges();

            this.repository = new ToDoTaskRepository(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
        }

        [TestCaseSource(nameof(ToDoTasksToTests))]
        public async Task AddAsync_ShouldAddItemToDbContext(ToDoTask toDoTask)
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.ToDoTasks.CountAsync();

            // Act
            var result = await this.repository.AddAsync(toDoTask);
            var getToDoTaskFromDatabase = await this.dbContext.ToDoTasks.FirstOrDefaultAsync(x => x.Id == toDoTask.Id);
            var countItemsInDatabaseAfter = await this.dbContext.ToDoTasks.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase + 1, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(toDoTask.Id));
            Assert.That(getToDoTaskFromDatabase?.Id, Is.EqualTo(toDoTask.Id));
            Assert.That(getToDoTaskFromDatabase?.Tile, Is.EqualTo(toDoTask.Tile));
            Assert.That(getToDoTaskFromDatabase?.Description, Is.EqualTo(toDoTask.Description));
            Assert.That(getToDoTaskFromDatabase?.DueDate, Is.EqualTo(toDoTask.DueDate));
            Assert.That(getToDoTaskFromDatabase?.Reminder, Is.EqualTo(toDoTask.Reminder));
            Assert.That(getToDoTaskFromDatabase?.ReminderDate, Is.EqualTo(toDoTask.ReminderDate));
            Assert.That(getToDoTaskFromDatabase?.Daily, Is.EqualTo(toDoTask.Daily));
            Assert.That(getToDoTaskFromDatabase?.Important, Is.EqualTo(toDoTask.Important));
            Assert.That(getToDoTaskFromDatabase?.IsCompleted, Is.EqualTo(toDoTask.IsCompleted));
            Assert.That(getToDoTaskFromDatabase?.ListId, Is.EqualTo(toDoTask.ListId));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveItemFromDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.ToDoTasks.CountAsync();
            var getToDoTaskFromDatabase = this.dbContext.ToDoTasks.First();

            // Act
            var result = await this.repository.DeleteAsync(getToDoTaskFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.ToDoTasks.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase - 1, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(getToDoTaskFromDatabase.Id));
            Assert.Null(this.dbContext.ToDoTasks.Find(getToDoTaskFromDatabase.Id));
        }

        [Test]
        public void DeleteAsync_ThrowsArgumentNullExceptionWhenCategoryNotFound()
        {
            // Arrange
            var id = 1001;

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => this.repository.DeleteAsync(id),
                $"Given task with {id}, can not be found in database.");
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingItemInDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.ToDoTasks.CountAsync();
            var newToDoTask = new ToDoTask()
            {
                Tile = "Updated title",
                Description = "Updated description",
                DueDate = new DateTimeOffset(2023, 1, 10, 0, 0, 0, TimeSpan.Zero),
                Reminder = true,
                ReminderDate = new DateTimeOffset(2023, 1, 10, 0, 0, 0, TimeSpan.Zero),
                Daily = false,
                Important = false,
                IsCompleted = false,
            };
            var getToDoTaskFromDatabase = this.dbContext.ToDoTasks.First();
            var countItemsInDatabaseAfter = await this.dbContext.ToDoTasks.CountAsync();

            // Act
            var result = await this.repository.UpdateAsync(getToDoTaskFromDatabase.Id, newToDoTask);
            var retrievedToDoTask = this.dbContext.ToDoTasks.Find(getToDoTaskFromDatabase.Id);

            // Assert
            Assert.That(countItemsInDatabase, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(retrievedToDoTask?.Id));
            Assert.That(newToDoTask.Tile, Is.EqualTo(retrievedToDoTask?.Tile));
            Assert.That(newToDoTask.Description, Is.EqualTo(retrievedToDoTask?.Description));
            Assert.That(newToDoTask.DueDate, Is.EqualTo(retrievedToDoTask?.DueDate));
            Assert.That(newToDoTask.Reminder, Is.EqualTo(retrievedToDoTask?.Reminder));
            Assert.That(newToDoTask.ReminderDate, Is.EqualTo(retrievedToDoTask?.ReminderDate));
            Assert.That(newToDoTask.Daily, Is.EqualTo(retrievedToDoTask?.Daily));
            Assert.That(newToDoTask.Important, Is.EqualTo(retrievedToDoTask?.Important));
            Assert.That(newToDoTask.IsCompleted, Is.EqualTo(retrievedToDoTask?.IsCompleted));
        }

        [Test]
        public void UpdateAsync_ThrowsNullReferenceExceptionWhenItemNotFound()
        {
            // Arrange
            var newToDoTask = new ToDoTask()
            {
                Tile = "Updated title",
                Description = "Updated description",
                DueDate = new DateTimeOffset(2023, 1, 10, 0, 0, 0, TimeSpan.Zero),
                Reminder = true,
                ReminderDate = new DateTimeOffset(2023, 1, 10, 0, 0, 0, TimeSpan.Zero),
                Daily = false,
                Important = false,
                IsCompleted = false,
            };
            var id = 1001;

            // Asserts
            Assert.ThrowsAsync<NullReferenceException>(
                () => this.repository.UpdateAsync(id, newToDoTask),
                $"Given task with {id}, can not be found in database.");
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectItem()
        {
            // Arrange
            var getToDoTaskFromDatabase = this.dbContext.ToDoTasks.First();

            // Act
            var result = await this.repository.GetByIdAsync(getToDoTaskFromDatabase.Id);

            // Assert
            Assert.That(result, Is.EqualTo(getToDoTaskFromDatabase));
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

        [Test]
        [TestCase(100, 3)]
        [TestCase(101, 1)]
        [TestCase(102, 1)]
        public void GetAll_ShouldReturnAllListTasks(int id, int count)
        {
            // Act
            var result = this.repository.GetAll(id);

            // Assert
            Assert.True(result.Count() == count);
        }
    }
}