using AutoMapper;
using Moq;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Services;
using ToDoApplication.Domain.Interfaces;

namespace ToDoApplication.Tests.Services
{
    public class TaskStatusesServiceTests
    {
        private Mock<ITaskStatusesRepository> mockRepository = new ();
        private Mock<IMapper> mockMapper = new ();
        private TaskStatusesService? service;
        private TaskStatusesDto statusDto;
        private TaskStatusesDto statusNewDataDto;
        private TaskStatusesDto statusToAddDto;
        private List<TaskStatusesDto> listOfStatusesDto;
        private List<TaskStatusesDto> listOfStatuses2Dto;
        private TaskStatuses status;
        private TaskStatuses statusNewData;
        private List<TaskStatuses> listOfStatuses;
        private List<TaskStatuses> listOfStatuses2;

        [SetUp]
        public void Setup()
        {
            this.statusDto = new TaskStatusesDto
            {
                Id = 1,
                Name = "priority",
                Description = "Description",
            };

            this.status = new TaskStatuses
            {
                Id = 1,
                Name = "priority",
                Description = "Description",
            };

            this.statusNewDataDto = new TaskStatusesDto
            {
                Id = 1,
                Name = "priority New data",
                Description = "Description",
            };

            this.statusNewData = new TaskStatuses
            {
                Id = 1,
                Name = "priority New data",
                Description = "Description",
            };

            this.statusToAddDto = new TaskStatusesDto
            {
                Id = 15,
                Name = "priority",
                Description = "Description",
            };

            this.listOfStatuses = new List<TaskStatuses>()
            {
                this.status,
            };

            this.listOfStatusesDto = new List<TaskStatusesDto>()
            {
                this.statusDto,
            };

            this.listOfStatuses2 = new List<TaskStatuses>()
            {
                this.status,
                this.statusNewData,
            };

            this.listOfStatuses2Dto = new List<TaskStatusesDto>()
            {
                this.statusDto,
                this.statusNewDataDto,
            };
        }

        [Test]
        public async Task AddAsync_ShouldAddTaskStatus_WhenValidTaskStatusDtoIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskStatuses>(It.IsAny<TaskStatusesDto>())).Returns(this.status);

            this.mockRepository.Setup(x => x.AddAsync(It.IsAny<TaskStatuses>())).ReturnsAsync(this.statusDto.Id);

            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.AddAsync(this.statusDto);

            // Assert
            this.mockRepository.Verify(x => x.AddAsync(It.IsAny<TaskStatuses>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.statusDto.Id));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentNullException_WhenNullTaskStatusDtoIsPassed()
        {
            // Arrange
            TaskStatusesDto? taskStatusDtoNull = null;
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.AddAsync(taskStatusDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Task statuses can not be null. (Parameter 'taskStatusDto')"));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentException_WhenTaskStatusDtoWithExistingNameIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskStatuses>(It.IsAny<TaskStatusesDto>())).Returns(this.status);
            this.mockMapper.Setup(x => x.Map<TaskStatusesDto>(It.IsAny<TaskStatuses>())).Returns(this.statusDto);
            this.mockRepository.Setup(x => x.GetByNameAsync(this.statusToAddDto.Name)).ReturnsAsync(this.status);
            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.AddAsync(this.statusToAddDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByNameAsync(It.IsAny<string>()), Times.Once());
            Assert.That(
                exception.Message,
                Is.EqualTo("Task statuses already exist in database. (Parameter 'taskStatusDto')"));
        }

        [Test]
        public async Task DeleteAsync_ValidId_ReturnsDeletedId()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskStatuses>(It.IsAny<TaskStatusesDto>())).Returns(this.status);
            this.mockMapper.Setup(x => x.Map<TaskStatusesDto>(It.IsAny<TaskStatuses>())).Returns(this.statusDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.status);
            this.mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(this.statusDto.Id);

            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.DeleteAsync(this.statusDto.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.status.Id));
        }

        [Test]
        public void DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var id = 1011;

            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskStatuses)null!);

            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => this.service.DeleteAsync(id));
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(
                ex.Message,
                Is.EqualTo("Status with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task UpdateAsync_ValidId_ReturnsUpdateId()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskStatuses>(It.IsAny<TaskStatusesDto>())).Returns(this.statusNewData);
            this.mockMapper.Setup(x => x.Map<TaskStatusesDto>(It.IsAny<TaskStatuses>())).Returns(this.statusDto);
            this.mockMapper.Setup(x => x.Map<IEnumerable<TaskStatusesDto>>(It.IsAny<IEnumerable<TaskStatuses>>())).Returns(this.listOfStatusesDto.AsQueryable());
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskStatuses>())).ReturnsAsync(this.status.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.status);

            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);
            var result = await this.service.UpdateAsync(this.status.Id, this.statusNewDataDto);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskStatuses>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.status.Id));
        }

        [Test]
        public void UpdateAsync_WithExistingItemWithSameName_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskStatuses>(It.IsAny<TaskStatusesDto>())).Returns(this.status);
            this.mockMapper.Setup(x => x.Map<TaskStatusesDto>(It.IsAny<TaskStatuses>())).Returns(this.statusDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.status);
            this.mockRepository.Setup(x => x.CheckIfExistInDataBaseWithSameNameAsync(It.IsAny<string>())).ReturnsAsync(this.status);
            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.UpdateAsync(this.status.Id, this.statusDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.CheckIfExistInDataBaseWithSameNameAsync(It.IsAny<string>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Status with given name already exist in database. (Parameter 'id')"));
        }

        [Test]
        public void UpdateAsync_ShouldThrowArgumentNullException_WhenNullTaskStatusDtoIsPassed()
        {
            // Arrange
            var id = 1;
            TaskStatusesDto? taskStatusDtoNull = null;
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.UpdateAsync(id, taskStatusDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Status can not be null. (Parameter 'taskStatusDto')"));
        }

        [Test]
        public void UpdateAsync_StatusNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskStatuses>(It.IsAny<TaskStatusesDto >())).Returns(this.status);
            this.mockMapper.Setup(x => x.Map<TaskStatusesDto>(It.IsAny<TaskStatuses>())).Returns((TaskStatusesDto)null!);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskStatuses)null!);
            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.UpdateAsync(this.statusDto.Id, this.statusDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(
                exception.Message,
                Is.EqualTo("Status with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsStatus()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskStatuses>(It.IsAny<TaskStatusesDto>())).Returns(this.status);
            this.mockMapper.Setup(x => x.Map<TaskStatusesDto>(It.IsAny<TaskStatuses>())).Returns(this.statusDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.status);
            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.GetByIdAsync(this.status.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.Compare(result, this.status);
        }

        [Test]
        public void GetByIdAsync_StatusNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskStatuses>(It.IsAny<TaskStatusesDto>())).Returns((TaskStatuses)null!);
            this.mockMapper.Setup(x => x.Map<TaskStatusesDto>(It.IsAny<TaskStatuses>())).Returns((TaskStatusesDto)null!);
            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.GetByIdAsync(this.statusDto.Id));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Status with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public void GetAll_ValidCall_ReturnsAllStatuses()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskStatusesRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<IEnumerable<TaskStatusesDto>>(It.IsAny<IEnumerable<TaskStatuses>>())).Returns(this.listOfStatuses2Dto);
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfStatuses2.AsQueryable());
            this.service = new TaskStatusesService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = this.service.GetAll().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());

            for (int i = 0; i < result.Count; i++)
            {
                this.Compare(result[i], this.listOfStatuses2[i]);
            }
        }

        private void Compare(TaskStatusesDto statusDto, TaskStatuses status)
        {
            Assert.Multiple(() =>
            {
                Assert.That(statusDto.Id, Is.EqualTo(status.Id));
                Assert.That(statusDto.Name, Is.EqualTo(status.Name));
                Assert.That(statusDto.Description, Is.EqualTo(status.Description));
            });
        }
    }
}
