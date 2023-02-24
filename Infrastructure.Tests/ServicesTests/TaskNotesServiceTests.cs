using AutoMapper;
using Moq;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Services;
using ToDoApplication.Domain.Interfaces;

namespace ToDoApplication.Tests.Services
{
    public class TaskNotesServiceTests
    {
        private Mock<ITaskNotesRepository> mockRepository = new ();
        private Mock<IMapper> mockMapper = new ();
        private TaskNotesService? service;
        private TaskNotesDto noteDto;
        private TaskNotesDto noteNewDataDto;
        private List<TaskNotesDto> listOfNotesDto;
        private List<TaskNotesDto> listOfNotes2Dto;
        private TaskNotes note;
        private TaskNotes noteNewData;
        private List<TaskNotes> listOfNotes;
        private List<TaskNotes> listOfNotes2;

        [SetUp]
        public void Setup()
        {
            this.noteDto = new TaskNotesDto
            {
                Id = 1,
                Name = "taskNote1",
            };

            this.note = new TaskNotes
            {
                Id = 1,
                Description = "taskNote1",
            };

            this.noteNewDataDto = new TaskNotesDto
            {
                Id = 1,
                Name = "taskNote1 New data",
            };

            this.noteNewData = new TaskNotes
            {
                Id = 1,
                Description = "taskNote1 New data",
            };

            this.listOfNotes = new List<TaskNotes>()
            {
                this.note,
            };

            this.listOfNotesDto = new List<TaskNotesDto>()
            {
                this.noteDto,
            };

            this.listOfNotes2 = new List<TaskNotes>()
            {
                this.note,
                this.noteNewData,
            };

            this.listOfNotes2Dto = new List<TaskNotesDto>()
            {
                this.noteDto,
                this.noteNewDataDto,
            };
        }

        [Test]
        public async Task AddAsync_ShouldAddTaskNote_WhenValidTaskNoteDtoIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskNotes>(It.IsAny<TaskNotesDto>())).Returns(this.note);

            this.mockRepository.Setup(x => x.AddAsync(It.IsAny<TaskNotes>())).ReturnsAsync(this.noteDto.Id);

            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.AddAsync(this.noteDto);

            // Assert
            this.mockRepository.Verify(x => x.AddAsync(It.IsAny<TaskNotes>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.noteDto.Id));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentNullException_WhenNullTaskCategoryDtoIsPassed()
        {
            // Arrange
            TaskNotesDto? taskNoteDtoNull = null;
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.AddAsync(taskNoteDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Note to add can not be null. (Parameter 'taskNoteDto')"));
        }

        [Test]
        public async Task DeleteAsync_ValidId_ReturnsDeletedId()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskNotes>(It.IsAny<TaskNotesDto>())).Returns(this.note);
            this.mockMapper.Setup(x => x.Map<TaskNotesDto>(It.IsAny<TaskNotes>())).Returns(this.noteDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.note);
            this.mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(this.noteDto.Id);

            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.DeleteAsync(this.noteDto.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.note.Id));
        }

        [Test]
        public void DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var id = 1011;

            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskNotes)null!);
            this.mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(id);

            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => this.service.DeleteAsync(id));
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(ex.Message, Is.EqualTo("Note with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task UpdateAsync_ValidId_ReturnsUpdateId()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskNotes>(It.IsAny<TaskNotesDto>())).Returns(this.noteNewData);
            this.mockMapper.Setup(x => x.Map<TaskNotesDto>(It.IsAny<TaskNotes>())).Returns(this.noteDto);
            this.mockMapper.Setup(x => x.Map<IEnumerable<TaskNotesDto>>(It.IsAny<IEnumerable<TaskNotes>>())).Returns(this.listOfNotesDto.AsQueryable());
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskNotes>())).ReturnsAsync(this.note.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.note);

            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);
            var result = await this.service.UpdateAsync(this.note.Id, this.noteNewDataDto);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskNotes>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.note.Id));
        }

        [Test]
        public void UpdateAsync_ShouldThrowArgumentNullException_WhenNullTaskNoteDtoIsPassed()
        {
            // Arrange
            var id = 1;
            TaskNotesDto? noteCategoryDtoNull = null;
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.UpdateAsync(id, noteCategoryDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Note can not be null. (Parameter 'taskNoteDto')"));
        }

        [Test]
        public void UpdateAsync_NoteNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskNotes>(It.IsAny<TaskNotesDto>())).Returns(this.note);
            this.mockMapper.Setup(x => x.Map<TaskNotesDto>(It.IsAny<TaskNotes>())).Returns((TaskNotesDto)null!);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskNotes)null!);
            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(
                () => this.service.UpdateAsync(this.noteDto.Id, this.noteDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Note with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsNote()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskNotes>(It.IsAny<TaskNotesDto>())).Returns(this.note);
            this.mockMapper.Setup(x => x.Map<TaskNotesDto>(It.IsAny<TaskNotes>())).Returns(this.noteDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.note);
            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.GetByIdAsync(this.note.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.Compare(result, this.note);
        }

        [Test]
        public void GetByIdAsync_NoteNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskNotes>(It.IsAny<TaskNotesDto>())).Returns((TaskNotes)null!);
            this.mockMapper.Setup(x => x.Map<TaskNotesDto>(It.IsAny<TaskNotes>())).Returns((TaskNotesDto)null!);
            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.GetByIdAsync(this.noteDto.Id));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Note with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public void GetAll_ValidCall_ReturnsAllNotes()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskNotesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<IEnumerable<TaskNotesDto>>(It.IsAny<IEnumerable<TaskCategory>>())).Returns(this.listOfNotes2Dto);
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfNotes2.AsQueryable());
            this.service = new TaskNotesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = this.service.GetAll().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());

            for (int i = 0; i < result.Count; i++)
            {
                this.Compare(result[i], this.listOfNotes2[i]);
            }
        }

        private void Compare(TaskNotesDto notesDto, TaskNotes notes)
        {
            Assert.Multiple(() =>
            {
                Assert.That(notesDto.Id, Is.EqualTo(notes.Id));
                Assert.That(notesDto.Name, Is.EqualTo(notes.Description));
            });
        }
    }
}
