using Microsoft.EntityFrameworkCore;
using ToDoApplication.Infrastructure;
using ToDoApplication.Infrastructure.Repositories;

namespace ToDoApplication.Tests.Repositories
{
    public class ToDoListRepositoryTests
    {
        private AppDbContext dbContext;
        private ToDoListRepository repository;

        private static IEnumerable<TestCaseData> ToDoListsToTests
        {
            get
            {
                yield return new TestCaseData(
                    new ToDoList()
                    {
                        Tile = "Test ToDoList",
                        Description = "Test ToDoList description",
                        IsHidden = false,
                        CreationDate = DateTimeOffset.UtcNow,
                        UserId = "user1",
                        Tasks = new List<ToDoTask>()
                        {
                            new ToDoTask()
                            {
                                Tile = "Test ToDoTask",
                                Description = "Test ToDoTask Description",
                            }
                        }
                    });
                yield return new TestCaseData(
                    new ToDoList()
                    {
                        Tile = "Shopping List",
                        Description = "List of grocery items to buy",
                        IsHidden = false,
                        CreationDate = DateTimeOffset.UtcNow,
                        UserId = "user12",
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
                    Tile = "ToDoList1 Db",
                    Description = "Test ToDoList1 Db description",
                    IsHidden = false,
                    CreationDate = DateTimeOffset.UtcNow,
                    UserId = "0",
                    User = new ApplicationUser()
                    {
                        UserName = "User1",
                    },
                    Tasks = new List<ToDoTask>()
                    {
                        new ToDoTask()
                        {
                            Tile = "ToDoTask1 Db",
                            Description = "Test ToDoTask1 Db Description",
                        },
                        new ToDoTask()
                        {
                            Tile = "ToDoTask2 Db",
                            Description = "Test ToDoTask2 Db Description",
                        },
                    }
                });
            this.dbContext.SaveChanges();

            this.repository = new ToDoListRepository(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
        }

        [TestCaseSource(nameof(ToDoListsToTests))]
        public async Task AddAsync_ShouldAddItemToDbContext(ToDoList toDoList)
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.ToDoLists.CountAsync();

            // Act
            var result = await this.repository.AddAsync(toDoList);
            var getToDoListFromDatabase = await this.dbContext.ToDoLists.FirstOrDefaultAsync(x => x.Id == toDoList.Id);
            var countItemsInDatabaseAfter = await this.dbContext.ToDoLists.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase + 1, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(toDoList.Id));
            Assert.That(getToDoListFromDatabase?.Id, Is.EqualTo(toDoList.Id));
            Assert.That(getToDoListFromDatabase?.Tile, Is.EqualTo(toDoList.Tile));
            Assert.That(getToDoListFromDatabase?.Description, Is.EqualTo(toDoList.Description));
            Assert.That(getToDoListFromDatabase?.CreationDate, Is.EqualTo(toDoList.CreationDate));
            Assert.That(getToDoListFromDatabase?.UserId, Is.EqualTo(toDoList.UserId));
            Assert.That(getToDoListFromDatabase?.Tasks, Is.EqualTo(toDoList.Tasks));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveItemFromDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.ToDoLists.CountAsync();
            var getToDoListFromDatabase = this.dbContext.ToDoLists.First();

            // Act
            var result = await this.repository.DeleteAsync(getToDoListFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.ToDoLists.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase - 1, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(getToDoListFromDatabase.Id));
            Assert.Null(this.dbContext.ToDoLists.Find(getToDoListFromDatabase.Id));
        }

        [Test]
        public void DeleteAsync_ThrowsArgumentNullExceptionWhenCategoryNotFound()
        {
            // Arrange
            var id = 1001;

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => this.repository.DeleteAsync(id),
                $"Given list with {id}, can not be found in database.");
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingItemInDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.ToDoLists.CountAsync();
            var newToDoList = new ToDoList()
            {
                Tile = "Updated title",
                Description = "Updated description",
                IsHidden = true
            };
            var getToDoListFromDatabase = this.dbContext.ToDoLists.First();

            // Act
            var result = await this.repository.UpdateAsync(getToDoListFromDatabase.Id, newToDoList);
            var retrievedToDoList = this.dbContext.ToDoLists.Find(getToDoListFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.ToDoLists.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(retrievedToDoList?.Id));
            Assert.That(newToDoList.Tile, Is.EqualTo(retrievedToDoList?.Tile));
            Assert.That(newToDoList.Description, Is.EqualTo(retrievedToDoList?.Description));
        }

        [Test]
        public void UpdateAsync_ThrowsNullReferenceExceptionWhenItemNotFound()
        {
            // Arrange
            var newToDoList = new ToDoList()
            {
                Tile = "Updated title",
                Description = "Updated description",
                IsHidden = true
            };
            var id = 1001;

            // Asserts
            Assert.ThrowsAsync<NullReferenceException>(
                () => this.repository.UpdateAsync(id, newToDoList),
                $"Given list with {id}, can not be found in database.");
        }

        [Test]
        public async Task GetByNameAsync_ShouldReturnItemWithMatchingName()
        {
            // Arrange
            var getToDoListFromDatabase = this.dbContext.ToDoLists.First();

            // Act
            var result = await this.repository.GetByNameAsync(getToDoListFromDatabase.Tile);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(getToDoListFromDatabase?.Id));
            Assert.That(result.Tile, Is.EqualTo(getToDoListFromDatabase?.Tile));
            Assert.That(result.Description, Is.EqualTo(getToDoListFromDatabase?.Description));
            Assert.That(result.IsHidden, Is.EqualTo(getToDoListFromDatabase?.IsHidden));
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
            var getToDoListFromDatabase = this.dbContext.ToDoLists.First();

            // Act
            var result = await this.repository.GetByIdAsync(getToDoListFromDatabase.Id);

            // Assert
            Assert.That(result, Is.EqualTo(getToDoListFromDatabase));
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