using Microsoft.EntityFrameworkCore;
using ToDoApplication.Infrastructure;
using ToDoApplication.Infrastructure.Repositories;

namespace ToDoApplication.Tests.Repositories
{
    public class TaskCategoryRepositoryTests
    {
        private AppDbContext dbContext;
        private TaskCategoryRepository repository;

        private static IEnumerable<TestCaseData> CategoriesToTests
        {
            get
            {
                yield return new TestCaseData(
                    new TaskCategory()
                    {
                        Name = "Test category",
                        Description = "Test category description",
                    });
                yield return new TestCaseData(
                    new TaskCategory()
                    {
                        Name = "Black category",
                        Description = "Black category",
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

            this.repository = new TaskCategoryRepository(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
        }

        [TestCaseSource(nameof(CategoriesToTests))]
        public async Task AddAsync_ShouldAddItemToDbContext(TaskCategory category)
        {
            // Act
            var countItemsInDatabase = await this.dbContext.TaskCategories.CountAsync();

            // Act
            var result = await this.repository.AddAsync(category);
            var getCategoryFromDatabase = await this.dbContext.TaskCategories.FirstOrDefaultAsync(x => x.Id == category.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskCategories.CountAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(countItemsInDatabase + 1, Is.EqualTo(countItemsInDatabaseAfter));
                Assert.That(result, Is.EqualTo(category.Id));
                Assert.That(getCategoryFromDatabase?.Id, Is.EqualTo(category.Id));
                Assert.That(getCategoryFromDatabase?.Name, Is.EqualTo(category.Name));
                Assert.That(getCategoryFromDatabase?.Description, Is.EqualTo(category.Description));
            });
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveItemFromDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskCategories.CountAsync();
            var getCategoryFromDatabase = this.dbContext.TaskCategories.First();

            // Act
            var result = await this.repository.DeleteAsync(getCategoryFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskCategories.CountAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(countItemsInDatabase - 1, Is.EqualTo(countItemsInDatabaseAfter));
                Assert.That(result, Is.EqualTo(getCategoryFromDatabase.Id));
                Assert.That(this.dbContext.TaskCategories.Find(getCategoryFromDatabase.Id), Is.Null);
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
                $"Given category with {id}, can not be found in database.");
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingItemInDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskCategories.CountAsync();
            var newCategory = new TaskCategory()
            {
                Name = "Updated name",
                Description = "Updated desc."
            };
            var getCategoryFromDatabase = this.dbContext.TaskCategories.First();

            // Act
            var result = await this.repository.UpdateAsync(getCategoryFromDatabase.Id, newCategory);
            var retrievedTaskCategory = this.dbContext.TaskCategories.Find(getCategoryFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskCategories.CountAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(countItemsInDatabase, Is.EqualTo(countItemsInDatabaseAfter));
                Assert.That(result, Is.EqualTo(retrievedTaskCategory?.Id));
                Assert.That(newCategory.Name, Is.EqualTo(retrievedTaskCategory?.Name));
                Assert.That(newCategory.Description, Is.EqualTo(retrievedTaskCategory?.Description));
            });
        }

        [Test]
        public void UpdateAsync_ThrowsNullReferenceExceptionWhenItemNotFound()
        {
            // Arrange
            var newCategory = new TaskCategory()
            {
                Name = "Updated name",
                Description = "Updated desc."
            };
            var id = 1001;

            // Asserts
            Assert.ThrowsAsync<NullReferenceException>(
                () => this.repository.UpdateAsync(id, newCategory),
                $"Given category with {id}, can not be found in database.");
        }

        [Test]
        public async Task GetByNameAsync_ShouldReturnTaskCategoryWithMatchingName()
        {
            // Arrange
            var getCategoryFromDatabase = this.dbContext.TaskCategories.First();

            // Act
            var result = await this.repository.GetByNameAsync(getCategoryFromDatabase.Name);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(getCategoryFromDatabase.Id));
                Assert.That(result.Name, Is.EqualTo(getCategoryFromDatabase.Name));
                Assert.That(result.Description, Is.EqualTo(getCategoryFromDatabase.Description));
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
            var getCategoryFromDatabase = this.dbContext.TaskCategories.First();

            // Act
            var result = await this.repository.GetByIdAsync(getCategoryFromDatabase.Id);

            // Assert
            Assert.That(result, Is.EqualTo(getCategoryFromDatabase));
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

        [Test]
        public async Task CheckIfExistInDataBaseWithSameNameAsync_ShouldReturnItem()
        {
            // Arrange
            var cat = new TaskCategory()
            {
                Name = "Test category",
                Description = "Test category description",
            };

            this.dbContext.TaskCategories.Add(cat);
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.repository.CheckIfExistInDataBaseWithSameNameAsync(cat.Name);

            // Assert
            Assert.That(result, Is.EqualTo(cat));
        }

        [Test]
        public async Task CheckIfExistInDataBaseWithSameNameAsync_ShouldReturnNull()
        {
            // Arrange
            var cat = new TaskCategory()
            {
                Name = "Test category",
                Description = "Test category description",
            };

            // Act
            var result = await this.repository.CheckIfExistInDataBaseWithSameNameAsync(cat.Name);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}