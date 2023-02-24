using AutoMapper;
using Moq;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Services;
using ToDoApplication.Domain.Interfaces;

namespace ToDoApplication.Tests.Services
{
    public class ToDoTaskServiceTests
    {
        private Mock<IToDoTaskRepository> mockRepository = new();
        private Mock<IMapper> mockMapper = new();
        private ToDoTaskService? service;
        private ToDoTaskDto toDoTaskDto;
        private ToDoTaskDto toDoTaskNewDataDto;
        private List<ToDoTaskDto> listOfToDoTasksDto;
        private List<ToDoTaskDto> listOfToDoTasks2Dto;
        private ToDoTask toDoTask;
        private ToDoTask toDoTaskNewData;
        private List<ToDoTask> listOfToDoTasks;
        private List<ToDoTask> listOfToDoTasks2;

        private static IEnumerable<TestCaseData> CheckIfReminderTimeOccurs
        {
            get
            {
                yield return new TestCaseData(
                    new DateTimeOffset(2002, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0)),
                    true);
                yield return new TestCaseData(
                    new DateTimeOffset(2032, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0)),
                    false);
            }
        }

        [SetUp]
        public void Setup()
        {
            this.toDoTaskDto = new ToDoTaskDto
            {
                Id = 1,
                Name = "Task 1",
                CategoryId = 1,
                CreationDate = DateTimeOffset.MinValue,
                Daily = true,
                Description = "Description T1",
                DueDate = DateTimeOffset.MaxValue,
                Important = false,
                IsCompleted = false,
                ListId = 1,
                PriorityId = 1,
                Reminder = false,
                StatusId = 1,
            };

            this.toDoTask = new ToDoTask
            {
                Id = 1,
                Tile = "Task 1",
                CategoryId = 1,
                CreationDate = DateTimeOffset.MinValue,
                Daily = true,
                Description = "Description T1",
                DueDate = DateTimeOffset.MaxValue,
                Important = false,
                IsCompleted = false,
                ListId = 1,
                PriorityId = 1,
                Reminder = false,
                StatusId = 1,
            };

            this.toDoTaskNewDataDto = new ToDoTaskDto
            {
                Id = 1,
                Name = "taskNote1 New data",
            };

            this.toDoTaskNewData = new ToDoTask
            {
                Id = 1,
                Tile = "taskNote1 New data",
            };

            this.listOfToDoTasks = new List<ToDoTask>()
            {
                this.toDoTask,
            };

            this.listOfToDoTasksDto = new List<ToDoTaskDto>()
            {
                this.toDoTaskDto,
            };

            this.listOfToDoTasks2 = new List<ToDoTask>()
            {
                this.toDoTask,
                this.toDoTaskNewData,
            };

            this.listOfToDoTasks2Dto = new List<ToDoTaskDto>()
            {
                this.toDoTaskDto,
                this.toDoTaskNewDataDto,
            };
        }

        [Test]
        public async Task AddAsync_ShouldAddTask_WhenValidTaskDtoIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockRepository.Setup(x => x.AddAsync(It.IsAny<ToDoTask>())).ReturnsAsync(this.toDoTaskDto.Id);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.AddAsync(this.toDoTaskDto);

            // Assert
            this.mockRepository.Verify(x => x.AddAsync(It.IsAny<ToDoTask>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.toDoTaskDto.Id));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentNullException_WhenNullTaskDtoIsPassed()
        {
            // Arrange
            ToDoTaskDto? task = null;
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.AddAsync(task!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Task to add can not be null. (Parameter 'taskDto')"));
        }

        [Test]
        public async Task DeleteAsync_ValidId_ReturnsDeletedId()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns(this.toDoTaskDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.toDoTask);
            this.mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(this.toDoTaskDto.Id);

            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.DeleteAsync(this.toDoTaskDto.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.toDoTask.Id));
        }

        [Test]
        public void DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var id = 1011;

            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ToDoTask)null!);

            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => this.service.DeleteAsync(id));
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(ex.Message, Is.EqualTo("Task with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task UpdateAsync_ValidId_ReturnsUpdateId()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTaskNewData);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns(this.toDoTaskDto);
            this.mockMapper.Setup(x => x.Map<IEnumerable<ToDoTaskDto>>(It.IsAny<IEnumerable<ToDoTask>>())).Returns(this.listOfToDoTasksDto.AsQueryable());
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoTask>())).ReturnsAsync(this.toDoTask.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.toDoTask);

            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);
            var result = await this.service.UpdateAsync(this.toDoTask.Id, this.toDoTaskNewDataDto);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoTask>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.toDoTask.Id));
        }

        [Test]
        public void UpdateAsync_ShouldThrowArgumentNullException_WhenNullTaskDtoIsPassed()
        {
            // Arrange
            var id = 1;
            ToDoTaskDto? taskDtoNull = null;
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.UpdateAsync(id, taskDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Task can not be null. (Parameter 'taskDto')"));
        }

        [Test]
        public void UpdateAsync_TaskNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns((ToDoTaskDto)null!);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ToDoTask)null!);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(
                () => this.service.UpdateAsync(this.toDoTaskDto.Id, this.toDoTaskDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Task with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsTask()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns(this.toDoTaskDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.toDoTask);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.GetByIdAsync(this.toDoTask.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.Compare(result, this.toDoTask);
        }

        [Test]
        public void GetByIdAsync_TaskNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns((ToDoTask)null!);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns((ToDoTaskDto)null!);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.GetByIdAsync(this.toDoTaskDto.Id));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Task with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public void GetAll_ValidCall_ReturnsAllTasks()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<IEnumerable<ToDoTaskDto>>(It.IsAny<IEnumerable<ToDoTask>>())).Returns(this.listOfToDoTasks2Dto);
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfToDoTasks2.AsQueryable());
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = this.service.GetAll().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());

            for (int i = 0; i < result.Count; i++)
            {
                this.Compare(result[i], this.listOfToDoTasks2[i]);
            }
        }

        [Test]
        public async Task TurnOnOffReminderAsync_ValidCall()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns(this.toDoTaskDto);
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoTask>())).ReturnsAsync(this.toDoTask.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.toDoTask);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.TurnOnOffReminderAsync(this.toDoTask.Id, true);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoTask>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.toDoTask.Id));
        }

        [Test]
        public void TurnOnOffReminderAsync_TaskNotFound_ThrowsArgumentExceptions()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns((ToDoTaskDto)null!);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ToDoTask)null!);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(
                () => this.service.TurnOnOffReminderAsync(this.toDoTask.Id, true));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Given task id does not exist in database! (Parameter 'id')"));
        }

        [Test]
        public async Task SetReminderTimeAsync_ValidCall()
        {
            // Arrange
            var reminderDate = new DateTimeOffset(2008, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0));
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns(this.toDoTaskDto);
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoTask>())).ReturnsAsync(this.toDoTask.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.toDoTask);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.SetReminderTimeAsync(this.toDoTask.Id, reminderDate);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoTask>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.toDoTask.Id));
        }

        [Test]
        public void SetReminderTimeAsync_TaskNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns((ToDoTaskDto)null!);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ToDoTask)null!);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(
                () => this.service.SetReminderTimeAsync(this.toDoTask.Id, DateTimeOffset.Now));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Given task id does not exist in database! (Parameter 'id')"));
        }

        [Test]
        public void SetReminderTimeAsync_ReminderDateLessThanCreateDate_ArgumentException()
        {
            // Arrange
            this.toDoTask.CreationDate = new DateTimeOffset(2009, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0));
            this.toDoTaskDto.CreationDate = new DateTimeOffset(2009, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0));
            var reminderDate = new DateTimeOffset(2008, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0));
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns(this.toDoTaskDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.toDoTask);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(
                () => this.service.SetReminderTimeAsync(this.toDoTask.Id, reminderDate));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Reminder date can not be less than create date. (Parameter 'reminderDate')"));
        }

        [TestCaseSource(nameof(CheckIfReminderTimeOccurs))]
        public async Task CheckIfReminderTimeOccursAsync_ValidCall(DateTimeOffset date, bool compareResult)
        {
            // Arrange
            this.toDoTask.ReminderDate = date;
            this.toDoTask.Reminder = true;
            this.toDoTaskDto.ReminderDate = date;
            this.toDoTaskDto.Reminder = true;
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoTask>(It.IsAny<ToDoTaskDto>())).Returns(this.toDoTask);
            this.mockMapper.Setup(x => x.Map<ToDoTaskDto>(It.IsAny<ToDoTask>())).Returns(this.toDoTaskDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.toDoTask);
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.CheckIfReminderTimeOccursAsync(this.toDoTask.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(result, Is.EqualTo(compareResult));
        }

        [Test]
        public void GetDailyTasks_ValidCall()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfToDoTasks2.AsQueryable());
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            this.service.GetDailyTasks().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [Test]
        public void GetImportantTasks_ValidCall()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfToDoTasks2.AsQueryable());
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            this.service.GetImportantTasks().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [Test]
        public void GetTaskForToday_ValidCall()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfToDoTasks2.AsQueryable());
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            this.service.GetTaskForToday().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [Test]
        public void GetCompletedTasks_ValidCall()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfToDoTasks2.AsQueryable());
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            this.service.GetCompletedTasks().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [Test]
        public void GetUncompletedTasks_ValidCall()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoTaskRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfToDoTasks2.AsQueryable());
            this.service = new ToDoTaskService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            this.service.GetUncompletedTasks().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());
        }

        private void Compare(ToDoTaskDto taskDto, ToDoTask task)
        {
            Assert.Multiple(() =>
            {
                Assert.That(taskDto.Id, Is.EqualTo(task.Id));
                Assert.That(taskDto.Name, Is.EqualTo(task.Tile));
                Assert.That(taskDto.Description, Is.EqualTo(task.Description));
                Assert.That(taskDto.CreationDate, Is.EqualTo(task.CreationDate));
                Assert.That(taskDto.CategoryId, Is.EqualTo(task.CategoryId));
                Assert.That(taskDto.Daily, Is.EqualTo(task.Daily));
                Assert.That(taskDto.DueDate, Is.EqualTo(task.DueDate));
                Assert.That(taskDto.Important, Is.EqualTo(task.Important));
                Assert.That(taskDto.IsCompleted, Is.EqualTo(task.IsCompleted));
                Assert.That(taskDto.ListId, Is.EqualTo(task.ListId));
                Assert.That(taskDto.Notes.Count, Is.EqualTo(task.Notes.Count));
                Assert.That(taskDto.PriorityId, Is.EqualTo(task.PriorityId));
                Assert.That(taskDto.Reminder, Is.EqualTo(task.Reminder));
                Assert.That(taskDto.ReminderDate, Is.EqualTo(task.ReminderDate));
                Assert.That(taskDto.StatusId, Is.EqualTo(task.StatusId));
            });
        }
    }
}
