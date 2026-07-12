namespace Exp.Todo.Tests.UnitTests.Application.Services;

public class TodoServiceTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TodoService _service;

    public TodoServiceTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _mockMapper = new Mock<IMapper>();
        var createValidator = new Exp.Todo.Application.Validators.CreateTodoDtoValidator();
        var updateValidator = new Exp.Todo.Application.Validators.UpdateTodoDtoValidator();

        _service = new TodoService(_mockRepository.Object, _mockMapper.Object, createValidator, updateValidator);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoTodosExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<TodoEntity>?)new List<TodoEntity>());
        _mockMapper.Setup(m => m.Map<List<TodoDto>>(It.IsAny<IEnumerable<TodoEntity>>()))
            .Returns(new List<TodoDto>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTodos_WhenTodosExist()
    {
        // Arrange
        var todos = new List<TodoEntity>
        {
            TodoEntity.Create("Task 1"),
            TodoEntity.Create("Task 2")
        };
        var todoDtos = new List<TodoDto>
        {
            new TodoDto { Id = 1 },
            new TodoDto { Id = 2 }
        };
        // Note: TodoName is read-only, so we can't set it directly in tests
        // The mapper will handle the mapping from TodoEntity to TodoDto

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<TodoEntity>?)todos);
        _mockMapper.Setup(m => m.Map<List<TodoDto>>(todos))
            .Returns(todoDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _mockMapper.Verify(m => m.Map<List<TodoDto>>(todos), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenRepositoryReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<TodoEntity>?)null);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTodo_WhenTodoExists()
    {
        // Arrange
        var todo = TodoEntity.Create("Test Task");
        var todoDto = new TodoDto { Id = 1 };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todo);
        _mockMapper.Setup(m => m.Map<TodoDto>(todo))
            .Returns(todoDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _mockMapper.Verify(m => m.Map<TodoDto>(todo), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenTodoNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoEntity?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsTrue_WhenUpdateSucceeds()
    {
        // Arrange
        var existingTodo = TodoEntity.Create("Original Task");
        var updateDto = new UpdateTodoDto { Id = 1, TodoName = "Updated Task" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTodo);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TodoEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TodoEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenTodoNotFound()
    {
        // Arrange
        var updateDto = new UpdateTodoDto { Id = 999, TodoName = "Updated Task" };

        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoEntity?)null);

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TodoEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateAsync_InvalidTodoName_ThrowsAppValidationException(string? todoName)
    {
        // Arrange
        var updateDto = new UpdateTodoDto { Id = 1, TodoName = todoName! };

        // Act & Assert
        await Assert.ThrowsAsync<AppValidationException>(() => _service.UpdateAsync(updateDto));
        _mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleteSucceeds()
    {
        // Arrange
        var todo = TodoEntity.Create("Task to Delete");
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todo);
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenTodoNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoEntity?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

