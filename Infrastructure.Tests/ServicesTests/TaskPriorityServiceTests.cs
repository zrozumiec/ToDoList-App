using AutoMapper;
using Moq;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Application.Services;
using ToDoApplication.Domain.Interfaces;

namespace ToDoApplication.Tests.Services
{
    public class TaskPriorityServiceTests
    {
        private Mock<ITaskPriorityRepository> mockRepository = new ();
        private Mock<IMapper> mockMapper = new ();
        private TaskPriorityService? service;
        private TaskPriorityDto priorityDto;
        private TaskPriorityDto priorityNewDataDto;
        private TaskPriorityDto priorityToAddDto;
        private List<TaskPriorityDto> listOfPrioriteisDto;
        private List<TaskPriorityDto> listOfPrioriteis2Dto;
        private TaskPriority priority;
        private TaskPriority priorityNewData;
        private List<TaskPriority> listOfPrioriteis;
        private List<TaskPriority> listOfPrioriteis2;

        [SetUp]
        public void Setup()
        {
            this.priorityDto = new TaskPriorityDto
            {
                Id = 1,
                Name = "priority",
            };

            this.priority = new TaskPriority
            {
                Id = 1,
                Name = "priority",
            };

            this.priorityNewDataDto = new TaskPriorityDto
            {
                Id = 1,
                Name = "priority New data",
            };

            this.priorityNewData = new TaskPriority
            {
                Id = 1,
                Name = "priority New data",
            };

            this.priorityToAddDto = new TaskPriorityDto
            {
                Id = 15,
                Name = "priority",
            };

            this.listOfPrioriteis = new List<TaskPriority>()
            {
                this.priority,
            };

            this.listOfPrioriteisDto = new List<TaskPriorityDto>()
            {
                this.priorityDto,
            };

            this.listOfPrioriteis2 = new List<TaskPriority>()
            {
                this.priority,
                this.priorityNewData,
            };

            this.listOfPrioriteis2Dto = new List<TaskPriorityDto>()
            {
                this.priorityDto,
                this.priorityNewDataDto,
            };
        }

        [Test]
        public async Task AddAsync_ShouldAddTaskPriority_WhenValidTaskPriorityDtoIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskPriority>(It.IsAny<TaskPriorityDto>())).Returns(this.priority);

            this.mockRepository.Setup(x => x.AddAsync(It.IsAny<TaskPriority>())).ReturnsAsync(this.priorityDto.Id);

            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.AddAsync(this.priorityDto);

            // Assert
            this.mockRepository.Verify(x => x.AddAsync(It.IsAny<TaskPriority>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.priorityDto.Id));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentNullException_WhenNullTaskPriorityDtoIsPassed()
        {
            // Arrange
            TaskPriorityDto? taskPriorityDtoNull = null;
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.AddAsync(taskPriorityDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Task priority can not be null. (Parameter 'taskPriorityDto')"));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentException_WhenTaskPriorityDtoWithExistingNameIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskPriority>(It.IsAny<TaskPriorityDto>())).Returns(this.priority);
            this.mockMapper.Setup(x => x.Map<TaskPriorityDto>(It.IsAny<TaskPriority>())).Returns(this.priorityDto);
            this.mockRepository.Setup(x => x.GetByNameAsync(this.priorityToAddDto.Name)).ReturnsAsync(this.priority);
            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.AddAsync(this.priorityToAddDto));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Task priority already exist in database. (Parameter 'taskPriorityDto')"));
        }

        [Test]
        public async Task DeleteAsync_ValidId_ReturnsDeletedId()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskPriority>(It.IsAny<TaskPriorityDto>())).Returns(this.priority);
            this.mockMapper.Setup(x => x.Map<TaskPriorityDto>(It.IsAny<TaskPriority>())).Returns(this.priorityDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.priority);
            this.mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(this.priorityDto.Id);

            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.DeleteAsync(this.priorityDto.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.priority.Id));
        }

        [Test]
        public void DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var id = 1011;

            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskPriority)null!);

            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => this.service.DeleteAsync(id));
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(
                ex.Message,
                Is.EqualTo("Priority with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task UpdateAsync_ValidId_ReturnsUpdateId()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskPriority>(It.IsAny<TaskPriorityDto>())).Returns(this.priorityNewData);
            this.mockMapper.Setup(x => x.Map<TaskPriorityDto>(It.IsAny<TaskPriority>())).Returns(this.priorityDto);
            this.mockMapper.Setup(x => x.Map<IEnumerable<TaskPriorityDto>>(It.IsAny<IEnumerable<TaskPriority>>())).Returns(this.listOfPrioriteisDto.AsQueryable());
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskPriority>())).ReturnsAsync(this.priority.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.priority);

            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);
            var result = await this.service.UpdateAsync(this.priority.Id, this.priorityNewDataDto);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskPriority>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.priority.Id));
        }

        [Test]
        public void UpdateAsync_WithExistingItemWithSameName_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskPriority>(It.IsAny<TaskPriorityDto>())).Returns(this.priority);
            this.mockMapper.Setup(x => x.Map<TaskPriorityDto>(It.IsAny<TaskPriority>())).Returns(this.priorityNewDataDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.priorityNewData);
            this.mockRepository.Setup(x => x.CheckIfExistInDataBaseWithSameNameAsync(It.IsAny<string>())).ReturnsAsync(this.priority);
            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.UpdateAsync(this.priority.Id, this.priorityDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.CheckIfExistInDataBaseWithSameNameAsync(It.IsAny<string>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Priority with given name already exist in database. (Parameter 'id')"));
        }

        [Test]
        public void UpdateAsync_ShouldThrowArgumentNullException_WhenNullTaskPriorityDtoIsPassed()
        {
            // Arrange
            var id = 1;
            TaskPriorityDto? taskPriorityDtoNull = null;
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.UpdateAsync(id, taskPriorityDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Priority can not be null. (Parameter 'taskPriorityDto')"));
        }

        [Test]
        public void UpdateAsync_PriorityNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskPriority>(It.IsAny<TaskPriorityDto>())).Returns(this.priority);
            this.mockMapper.Setup(x => x.Map<TaskPriorityDto>(It.IsAny<TaskPriority>())).Returns((TaskPriorityDto)null!);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskPriority)null!);
            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.UpdateAsync(this.priorityDto.Id, this.priorityDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(
                exception.Message,
                Is.EqualTo("Priority with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsPriority()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskPriority>(It.IsAny<TaskPriorityDto>())).Returns(this.priority);
            this.mockMapper.Setup(x => x.Map<TaskPriorityDto>(It.IsAny<TaskPriority>())).Returns(this.priorityDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.priority);
            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.GetByIdAsync(this.priority.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.Compare(result, this.priority);
        }

        [Test]
        public void GetByIdAsync_PriorityNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskPriority>(It.IsAny<TaskPriorityDto>())).Returns((TaskPriority)null!);
            this.mockMapper.Setup(x => x.Map<TaskPriorityDto>(It.IsAny<TaskPriority>())).Returns((TaskPriorityDto)null!);
            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.GetByIdAsync(this.priorityDto.Id));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Priority with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public void GetAll_ValidCall_ReturnsAllPriorities()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskPriorityRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<IEnumerable<TaskPriorityDto>>(It.IsAny<IEnumerable<TaskPriority>>())).Returns(this.listOfPrioriteis2Dto);
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfPrioriteis2.AsQueryable());
            this.service = new TaskPriorityService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = this.service.GetAll().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());

            for (int i = 0; i < result.Count; i++)
            {
                this.Compare(result[i], this.listOfPrioriteis2[i]);
            }
        }

        private void Compare(TaskPriorityDto priorityDto, TaskPriority priority)
        {
            Assert.Multiple(() =>
            {
                Assert.That(priorityDto.Id, Is.EqualTo(priority.Id));
                Assert.That(priorityDto.Name, Is.EqualTo(priority.Name));
            });
        }
    }
}
