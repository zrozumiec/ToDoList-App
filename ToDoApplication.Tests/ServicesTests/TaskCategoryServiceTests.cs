using AutoMapper;
using Moq;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Services;
using ToDoApplication.Domain.Interfaces;

namespace ToDoApplication.Tests.Services
{
    public class TaskCategoryServiceTests
    {
        private Mock<ITaskCategoryRepository> mockRepository = new ();
        private Mock<IMapper> mockMapper = new ();
        private TaskCategoryService? service;
        private TaskCategoryDto categoryDto;
        private TaskCategoryDto categoryNewDataDto;
        private TaskCategoryDto categoryToAddDto;
        private List<TaskCategoryDto> listOfCategoriesDto;
        private List<TaskCategoryDto> listOfCategories2Dto;
        private TaskCategory category;
        private TaskCategory categoryNewData;
        private List<TaskCategory> listOfCategories2;

        [SetUp]
        public void Setup()
        {
            this.categoryDto = new TaskCategoryDto
            {
                Id = 1,
                Name = "taskCategory1",
                Description = "taskCategory1"
            };

            this.category = new TaskCategory
            {
                Id = 1,
                Name = "taskCategory1",
                Description = "taskCategory1"
            };

            this.categoryNewDataDto = new TaskCategoryDto
            {
                Id = 1,
                Name = "taskCategory1 New data",
                Description = "taskCategory1 Updated"
            };

            this.categoryNewData = new TaskCategory
            {
                Id = 1,
                Name = "taskCategory1 New data",
                Description = "taskCategory1 Updated"
            };

            this.categoryToAddDto = new TaskCategoryDto
            {
                Id = 15,
                Name = "taskCategory1",
                Description = "hjhjh"
            };

            this.listOfCategoriesDto = new List<TaskCategoryDto>()
            {
                this.categoryDto,
            };

            this.listOfCategories2 = new List<TaskCategory>()
            {
                this.category,
                this.categoryNewData,
            };

            this.listOfCategories2Dto = new List<TaskCategoryDto>()
            {
                this.categoryDto,
                this.categoryNewDataDto,
            };
        }

        [Test]
        public async Task AddAsync_ShouldAddTaskCategory_WhenValidTaskCategoryDtoIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskCategory>(It.IsAny<TaskCategoryDto>())).Returns(this.category);

            this.mockRepository.Setup(x => x.AddAsync(It.IsAny<TaskCategory>())).ReturnsAsync(this.categoryDto.Id);

            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.AddAsync(this.categoryDto);

            // Assert
            this.mockRepository.Verify(x => x.AddAsync(It.IsAny<TaskCategory>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.categoryDto.Id));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentNullException_WhenNullTaskCategoryDtoIsPassed()
        {
            // Arrange
            TaskCategoryDto? taskCategoryDtoNull = null;
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.AddAsync(taskCategoryDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Task category can not be null. (Parameter 'taskCategoryDto')"));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentException_WhenTaskCategoryDtoWithExistingNameIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskCategory>(It.IsAny<TaskCategoryDto>())).Returns(this.category);
            this.mockMapper.Setup(x => x.Map<TaskCategoryDto>(It.IsAny<TaskCategory>())).Returns(this.categoryDto);
            this.mockRepository.Setup(x => x.GetByNameAsync(this.categoryToAddDto.Name)).ReturnsAsync(this.category);
            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.AddAsync(this.categoryToAddDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByNameAsync(It.IsAny<string>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Task category already exist in database. (Parameter 'taskCategoryDto')"));
        }

        [Test]
        public async Task DeleteAsync_ValidId_ReturnsDeletedId()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskCategory>(It.IsAny<TaskCategoryDto>())).Returns(this.category);
            this.mockMapper.Setup(x => x.Map<TaskCategoryDto>(It.IsAny<TaskCategory>())).Returns(this.categoryDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.category);
            this.mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(this.categoryDto.Id);

            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.DeleteAsync(this.categoryDto.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.category.Id));
        }

        [Test]
        public void DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var id = 1011;

            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskCategory)null!);
            this.mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(id);

            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => this.service.DeleteAsync(id));
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(ex.Message, Is.EqualTo("Category with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task UpdateAsync_ValidId_ReturnsUpdateId()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskCategory>(It.IsAny<TaskCategoryDto>())).Returns(this.categoryNewData);
            this.mockMapper.Setup(x => x.Map<TaskCategoryDto>(It.IsAny<TaskCategory>())).Returns(this.categoryDto);
            this.mockMapper.Setup(x => x.Map<IEnumerable<TaskCategoryDto>>(It.IsAny<IEnumerable<TaskCategory>>())).Returns(this.listOfCategoriesDto.AsQueryable());
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskCategory>())).ReturnsAsync(this.category.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.category);

            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);
            var result = await this.service.UpdateAsync(this.category.Id, this.categoryNewDataDto);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskCategory>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.category.Id));
        }

        [Test]
        public void UpdateAsync_WithExistingItemWithSameName_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskCategory>(It.IsAny<TaskCategoryDto>())).Returns(this.category);
            this.mockMapper.Setup(x => x.Map<TaskCategoryDto>(It.IsAny<TaskCategory>())).Returns(this.categoryNewDataDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.categoryNewData);
            this.mockRepository.Setup(x => x.CheckIfExistInDataBaseWithSameNameAsync(It.IsAny<string>())).ReturnsAsync(this.category);
            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.UpdateAsync(this.category.Id, this.categoryDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.CheckIfExistInDataBaseWithSameNameAsync(It.IsAny<string>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Category with given name already exist in database. (Parameter 'id')"));
        }

        [Test]
        public void UpdateAsync_ShouldThrowArgumentNullException_WhenNullTaskCategoryDtoIsPassed()
        {
            // Arrange
            var id = 1;
            TaskCategoryDto? taskCategoryDtoNull = null;
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.UpdateAsync(id, taskCategoryDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("Category can not be null. (Parameter 'taskCategoryDto')"));
        }

        [Test]
        public void UpdateAsync_CategoryNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskCategory>(It.IsAny<TaskCategoryDto>())).Returns(this.category);
            this.mockMapper.Setup(x => x.Map<TaskCategoryDto>(It.IsAny<TaskCategory>())).Returns((TaskCategoryDto)null!);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskCategory)null!);
            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.UpdateAsync(this.categoryDto.Id, this.categoryDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("Category with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsCategory()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskCategory>(It.IsAny<TaskCategoryDto>())).Returns(this.category);
            this.mockMapper.Setup(x => x.Map<TaskCategoryDto>(It.IsAny<TaskCategory>())).Returns(this.categoryDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.category);
            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.GetByIdAsync(this.category.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Compare(result, this.category);
        }

        [Test]
        public void GetByIdAsync_CategoryNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<TaskCategory>(It.IsAny<TaskCategoryDto>())).Returns((TaskCategory)null!);
            this.mockMapper.Setup(x => x.Map<TaskCategoryDto>(It.IsAny<TaskCategory>())).Returns((TaskCategoryDto)null!);
            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.GetByIdAsync(this.categoryDto.Id));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Category with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public void GetAll_ValidCall_ReturnsAllCategories()
        {
            // Arrange
            this.mockRepository = new Mock<ITaskCategoryRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<IEnumerable<TaskCategoryDto>>(It.IsAny<IEnumerable<TaskCategory>>())).Returns(this.listOfCategories2Dto);
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfCategories2.AsQueryable());
            this.service = new TaskCategoryService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = this.service.GetAll().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());

            for (int i = 0; i < result.Count; i++)
            {
                Compare(result[i], this.listOfCategories2[i]);
            }
        }

        private static void Compare(TaskCategoryDto categoryDto, TaskCategory category)
        {
            Assert.Multiple(() =>
            {
                Assert.That(categoryDto.Id, Is.EqualTo(category.Id));
                Assert.That(categoryDto.Name, Is.EqualTo(category.Name));
                Assert.That(categoryDto.Description, Is.EqualTo(category.Description));
            });
        }
    }
}
