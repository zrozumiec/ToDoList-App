using AutoMapper;
using Moq;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Services;
using ToDoApplication.Domain.Interfaces;

namespace ToDoApplication.Tests.Services
{
    public class ToDoListServiceTests
    {
        private Mock<IToDoListRepository> mockRepository = new ();
        private Mock<IMapper> mockMapper = new ();
        private ToDoListService? service;
        private ToDoListDto listDto;
        private ToDoListDto listNewDataDto;
        private List<ToDoListDto> listOfToDoListDto;
        private List<ToDoListDto> listOfToDoList2Dto;
        private ToDoList list;
        private ToDoList listNewData;
        private List<ToDoList> listOfToDoList2;

        [SetUp]
        public void Setup()
        {
            this.listDto = new ToDoListDto
            {
                Id = 1,
                Name = "List 1",
                Description = "Description",
                CreationDate = DateTimeOffset.MinValue,
                IsHidden = false,
                UserId = "22",
                Tasks = new List<ToDoTaskDto>
                {
                    new ToDoTaskDto
                    {
                        Id = 1,
                        Name = "Task 1",
                        Description = "Description",
                        CreationDate = DateTimeOffset.MinValue,
                        ListId = 1,
                    },
                    new ToDoTaskDto
                    {
                        Id = 2,
                        Name = "Task 2",
                        Description = "Description",
                        CreationDate = DateTimeOffset.MinValue,
                        ListId = 1,
                    }
                }
            };

            this.list = new ToDoList
            {
                Id = 1,
                Tile = "List 1",
                Description = "Description",
                CreationDate = DateTimeOffset.MinValue,
                IsHidden = false,
                UserId = "22",
                Tasks = new List<ToDoTask>
                {
                    new ToDoTask
                    {
                        Id = 1,
                        Tile = "Task 1",
                        Description = "Description",
                        CreationDate = DateTimeOffset.MinValue,
                        ListId = 1,
                    },
                    new ToDoTask
                    {
                        Id = 2,
                        Tile = "Task 2",
                        Description = "Description",
                        CreationDate = DateTimeOffset.MinValue,
                        ListId = 1,
                    }
                }
            };

            this.listNewDataDto = new ToDoListDto
            {
                Id = 1,
                Name = "List 1 Updated",
                Description = "Description 213124",
                CreationDate = DateTimeOffset.MinValue,
                IsHidden = true,
                UserId = "22",
                Tasks = new List<ToDoTaskDto>
                {
                    new ToDoTaskDto
                    {
                        Id = 1,
                        Name = "Task 1",
                        Description = "Description",
                        CreationDate = DateTimeOffset.MinValue,
                        ListId = 1,
                    },
                    new ToDoTaskDto
                    {
                        Id = 2,
                        Name = "Task 2",
                        Description = "Description",
                        CreationDate = DateTimeOffset.MinValue,
                        ListId = 1,
                    }
                }
            };

            this.listNewData = new ToDoList
            {
                Id = 1,
                Tile = "List 1 Updated",
                Description = "Description 213124",
                CreationDate = DateTimeOffset.MinValue,
                IsHidden = true,
                UserId = "22",
                Tasks = new List<ToDoTask>
                {
                    new ToDoTask
                    {
                        Id = 1,
                        Tile = "Task 1",
                        Description = "Description",
                        CreationDate = DateTimeOffset.MinValue,
                        ListId = 1,
                    },
                    new ToDoTask
                    {
                        Id = 2,
                        Tile = "Task 2",
                        Description = "Description",
                        CreationDate = DateTimeOffset.MinValue,
                        ListId = 1,
                    }
                }
            };

            this.listOfToDoListDto = new List<ToDoListDto>()
            {
                this.listDto,
            };

            this.listOfToDoList2 = new List<ToDoList>()
            {
                this.list,
                this.listNewData,
            };

            this.listOfToDoList2Dto = new List<ToDoListDto>()
            {
                this.listDto,
                this.listNewDataDto,
            };
        }

        [Test]
        public async Task AddAsync_ShouldAddToDoList_WhenValidListDtoIsPassed()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoList>(It.IsAny<ToDoListDto>())).Returns(this.list);

            this.mockRepository.Setup(x => x.AddAsync(It.IsAny<ToDoList>())).ReturnsAsync(this.listDto.Id);

            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.AddAsync(this.listDto);

            // Assert
            this.mockRepository.Verify(x => x.AddAsync(It.IsAny<ToDoList>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.listDto.Id));
        }

        [Test]
        public void AddAsync_ShouldThrowArgumentNullException_WhenNullListDtoIsPassed()
        {
            // Arrange
            ToDoListDto? listDtoNull = null;
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.AddAsync(listDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("List can not be null. (Parameter 'toDoListDto')"));
        }

        [Test]
        public async Task DeleteAsync_ValidId_ReturnsDeletedId()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoList>(It.IsAny<ToDoListDto>())).Returns(this.list);
            this.mockMapper.Setup(x => x.Map<ToDoListDto>(It.IsAny<ToDoList>())).Returns(this.listDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.list);
            this.mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(this.listDto.Id);

            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.DeleteAsync(this.listDto.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.list.Id));
        }

        [Test]
        public void DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var id = 1011;

            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ToDoList)null!);

            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => this.service.DeleteAsync(id));
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(
                ex.Message,
                Is.EqualTo("List with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task UpdateAsync_ValidId_ReturnsUpdateId()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoList>(It.IsAny<ToDoListDto>())).Returns(this.listNewData);
            this.mockMapper.Setup(x => x.Map<ToDoListDto>(It.IsAny<ToDoList>())).Returns(this.listDto);
            this.mockMapper.Setup(x => x.Map<IEnumerable<ToDoListDto>>(It.IsAny<IEnumerable<ToDoList>>())).Returns(this.listOfToDoListDto.AsQueryable());
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoList>())).ReturnsAsync(this.list.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.list);

            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);
            var result = await this.service.UpdateAsync(this.list.Id, this.listNewDataDto);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoList>()), Times.Once());
            Assert.That(result, Is.EqualTo(this.list.Id));
        }

        [Test]
        public void UpdateAsync_WithExistingItemWithSameName_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoList>(It.IsAny<ToDoListDto>())).Returns(this.list);
            this.mockMapper.Setup(x => x.Map<ToDoListDto>(It.IsAny<ToDoList>())).Returns(this.listNewDataDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.listNewData);
            this.mockRepository.Setup(x => x.CheckIfExistInDataBaseWithSameNameAsync(It.IsAny<string>())).ReturnsAsync(this.list);
            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.UpdateAsync(this.list.Id, this.listDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.CheckIfExistInDataBaseWithSameNameAsync(It.IsAny<string>()), Times.Once());
            Assert.That(exception.Message, Is.EqualTo("List with given name already exist in database. (Parameter 'id')"));
        }

        [Test]
        public void UpdateAsync_ShouldThrowArgumentNullException_WhenNullListDtoIsPassed()
        {
            // Arrange
            var id = 1;
            ToDoListDto? listDtoNull = null;
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentNullException>(
                () => this.service.UpdateAsync(id, listDtoNull!));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("List can not be null. (Parameter 'toDoListDto')"));
        }

        [Test]
        public void UpdateAsync_ListNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoList>(It.IsAny<ToDoListDto>())).Returns(this.list);
            this.mockMapper.Setup(x => x.Map<ToDoListDto>(It.IsAny<ToDoList>())).Returns((ToDoListDto)null!);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ToDoList)null!);
            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.UpdateAsync(this.listDto.Id, this.listDto));

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(
                exception.Message,
                Is.EqualTo("List with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsList()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoList>(It.IsAny<ToDoListDto>())).Returns(this.list);
            this.mockMapper.Setup(x => x.Map<ToDoListDto>(It.IsAny<ToDoList>())).Returns(this.listDto);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.list);
            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = await this.service.GetByIdAsync(this.list.Id);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            Compare(result, this.list);
        }

        [Test]
        public void GetByIdAsync_ListNotFound_ThrowsArgumentException()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoList>(It.IsAny<ToDoListDto>())).Returns((ToDoList)null!);
            this.mockMapper.Setup(x => x.Map<ToDoListDto>(It.IsAny<ToDoList>())).Returns((ToDoListDto)null!);
            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.service.GetByIdAsync(this.listDto.Id));

            // Assert
            Assert.That(
                exception.Message,
                Is.EqualTo("List with given id does not exist in database. (Parameter 'id')"));
        }

        [Test]
        public void GetAll_ValidCall_ReturnsAllLists()
        {
            // Arrange
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<IEnumerable<ToDoListDto>>(It.IsAny<IEnumerable<ToDoList>>())).Returns(this.listOfToDoList2Dto);
            this.mockRepository.Setup(x => x.GetAll()).Returns(this.listOfToDoList2.AsQueryable());
            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);

            // Act
            var result = this.service.GetAll().ToList();

            // Assert
            this.mockRepository.Verify(x => x.GetAll(), Times.Once());

            for (int i = 0; i < result.Count; i++)
            {
                Compare(result[i], this.listOfToDoList2[i]);
            }
        }

        [Test]
        public async Task ChangeListVisibility_ValidCall()
        {
            // Arrange
            var hiddenList = this.list;
            hiddenList.IsHidden = true;
            this.mockRepository = new Mock<IToDoListRepository>();
            this.mockMapper = new Mock<IMapper>();
            this.mockMapper.Setup(x => x.Map<ToDoList>(It.IsAny<ToDoListDto>())).Returns(hiddenList);
            this.mockMapper.Setup(x => x.Map<ToDoListDto>(It.IsAny<ToDoList>())).Returns(this.listDto);
            this.mockRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoList>())).ReturnsAsync(this.list.Id);
            this.mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.list);

            this.service = new ToDoListService(this.mockMapper.Object, this.mockRepository.Object);
            var result = await this.service.ChangeVisibility(this.list.Id, false);

            // Assert
            this.mockRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            this.mockRepository.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoList>()), Times.Once());
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(this.list.Id));
                Assert.That(this.list.IsHidden, Is.EqualTo(true));
            });
        }

        private static void Compare(ToDoListDto listDto, ToDoList list)
        {
            Assert.Multiple(() =>
            {
                Assert.That(listDto.Id, Is.EqualTo(list.Id));
                Assert.That(listDto.Name, Is.EqualTo(list.Tile));
                Assert.That(listDto.CreationDate, Is.EqualTo(list.CreationDate));
                Assert.That(listDto.Description, Is.EqualTo(list.Description));
                Assert.That(listDto.IsHidden, Is.EqualTo(list.IsHidden));
                Assert.That(listDto.Tasks.Count, Is.EqualTo(list.Tasks.Count));
                Assert.That(listDto.UserId, Is.EqualTo(list.UserId));
            });
        }
    }
}
