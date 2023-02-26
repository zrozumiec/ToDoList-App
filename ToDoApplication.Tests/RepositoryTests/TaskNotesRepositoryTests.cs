using Microsoft.EntityFrameworkCore;
using ToDoApplication.Infrastructure;
using ToDoApplication.Infrastructure.Repositories;

namespace ToDoApplication.Tests.Repositories
{
    public class TaskNotesRepositoryTests
    {
        private AppDbContext dbContext;
        private TaskNotesRepository repository;

        private static IEnumerable<TestCaseData> NotesToTests
        {
            get
            {
                yield return new TestCaseData(
                    new TaskNotes()
                    {
                        Description = "Notes 1",
                    });
                yield return new TestCaseData(
                    new TaskNotes()
                    {
                        Description = string.Empty,
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

            this.dbContext.TaskNotes.AddRange(
                new TaskNotes()
                {
                    Description = "Default Note 1",
                },
                new TaskNotes()
                {
                    Description = "Default Note 2",
                });
            this.dbContext.SaveChanges();

            this.repository = new TaskNotesRepository(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
        }

        [TestCaseSource(nameof(NotesToTests))]
        public async Task AddAsync_ShouldAddItemToDbContext(TaskNotes notes)
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskNotes.CountAsync();

            // Act
            var result = await this.repository.AddAsync(notes);
            var getNoteFromDatabase = await this.dbContext.TaskNotes.FirstOrDefaultAsync(x => x.Id == notes.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskNotes.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase + 1, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(notes.Id));
            Assert.That(getNoteFromDatabase?.Id, Is.EqualTo(notes.Id));
            Assert.That(getNoteFromDatabase?.Description, Is.EqualTo(notes.Description));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveItemFromDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskNotes.CountAsync();
            var getNoteFromDatabase = this.dbContext.TaskNotes.First();

            // Act
            var result = await this.repository.DeleteAsync(getNoteFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskNotes.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase - 1, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(getNoteFromDatabase.Id));
            Assert.Null(this.dbContext.TaskNotes.Find(getNoteFromDatabase.Id));
        }

        [Test]
        public void DeleteAsync_ThrowsArgumentNullExceptionWhenItemNotFound()
        {
            // Arrange
            var id = 1001;

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => this.repository.DeleteAsync(id),
                $"Given note with {id}, can not be found in database.");
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingItemInDbContext()
        {
            // Arrange
            var countItemsInDatabase = await this.dbContext.TaskNotes.CountAsync();
            var newNote = new TaskNotes()
            {
                Description = "Updated note."
            };
            var getNoteFromDatabase = this.dbContext.TaskNotes.First();

            // Act
            var result = await this.repository.UpdateAsync(getNoteFromDatabase.Id, newNote);
            var retrievedTaskNotes = this.dbContext.TaskNotes.Find(getNoteFromDatabase.Id);
            var countItemsInDatabaseAfter = await this.dbContext.TaskNotes.CountAsync();

            // Assert
            Assert.That(countItemsInDatabase, Is.EqualTo(countItemsInDatabaseAfter));
            Assert.That(result, Is.EqualTo(retrievedTaskNotes?.Id));
            Assert.That(newNote.Description, Is.EqualTo(retrievedTaskNotes?.Description));
        }

        [Test]
        public void UpdateAsync_ThrowsNullReferenceExceptionWhenItemNotFound()
        {
            // Arrange
            var newNote = new TaskNotes()
            {
                Description = "Note 1001",
            };
            var id = 1001;

            // Asserts
            Assert.ThrowsAsync<NullReferenceException>(
                () => this.repository.UpdateAsync(id, newNote),
                $"Given note with {id}, can not be found in database.");
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectItem()
        {
            // Arrange
            var getNoteFromDatabase = this.dbContext.TaskNotes.First();

            // Act
            var result = await this.repository.GetByIdAsync(getNoteFromDatabase.Id);

            // Assert
            Assert.That(result, Is.EqualTo(getNoteFromDatabase));
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