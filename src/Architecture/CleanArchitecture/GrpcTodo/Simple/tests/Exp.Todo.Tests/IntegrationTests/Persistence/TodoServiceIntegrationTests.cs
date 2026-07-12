namespace Exp.Todo.Tests.IntegrationTests.Persistence;

public class TodoServiceIntegrationTests
{
    private readonly AppDbContext _context;
    private readonly ITodoRepository _repository;
    private readonly TodoService _service;

    public TodoServiceIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TodoDb_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _repository = new TodoRepository(_context);
        
        // Set up AutoMapper - use mock for integration tests
        // The mapper is tested separately, here we focus on service/repository integration
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<TodoDto>(It.IsAny<TodoEntity>()))
            .Returns((TodoEntity todo) => new TodoDto { Id = todo.Id });
        mockMapper.Setup(m => m.Map<List<TodoDto>>(It.IsAny<IEnumerable<TodoEntity>>()))
            .Returns((IEnumerable<TodoEntity> todos) => todos.Select(t => new TodoDto { Id = t.Id }).ToList());
        var mapper = mockMapper.Object;
        
        // Set up validators
        var createValidator = new Exp.Todo.Application.Validators.CreateTodoDtoValidator();
        var updateValidator = new Exp.Todo.Application.Validators.UpdateTodoDtoValidator();
        
        _service = new TodoService(_repository, mapper, createValidator, updateValidator);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoTodosExist()
    {
        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTodos_WhenTodosExist()
    {
        // Arrange
        var todo1 = TodoEntity.Create("Task 1");
        var todo2 = TodoEntity.Create("Task 2");
        await _context.Todos.AddRangeAsync(todo1, todo2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTodo_WhenTodoExists()
    {
        // Arrange
        var todo = TodoEntity.Create("Test Task");
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetByIdAsync(todo.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(todo.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenTodoNotFound()
    {
        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesTodo_WhenTodoExists()
    {
        // Arrange
        var todo = TodoEntity.Create("Original Task");
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();
        
        // Detach the entity to simulate it being fetched fresh from the repository
        _context.Entry(todo).State = EntityState.Detached;

        var updateDto = new UpdateTodoDto { Id = todo.Id, TodoName = "Updated Task" };

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        result.Should().BeTrue();
        var updated = await _context.Todos.FindAsync([todo.Id]);
        updated.Should().NotBeNull();
        updated!.TodoName.Should().Be("Updated Task");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenTodoNotFound()
    {
        // Arrange
        var updateDto = new UpdateTodoDto { Id = 999, TodoName = "Updated Task" };

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_DeletesTodo_WhenTodoExists()
    {
        // Arrange
        var todo = TodoEntity.Create("Task to Delete");
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.DeleteAsync(todo.Id);

        // Assert
        result.Should().BeTrue();
        var deleted = await _context.Todos.FindAsync([todo.Id]);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenTodoNotFound()
    {
        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ThrowsAppValidationException_WhenDtoIsInvalid()
    {
        // Arrange
        var invalidDto = new CreateTodoDto { TodoName = "" };

        // Act & Assert
        await Assert.ThrowsAsync<AppValidationException>(() => _service.CreateAsync(invalidDto));
        
        var count = await _context.Todos.CountAsync();
        count.Should().Be(0);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsAppValidationException_WhenDtoIsInvalid()
    {
        // Arrange
        var todo = TodoEntity.Create("Original Task");
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();

        var invalidDto = new UpdateTodoDto { Id = todo.Id, TodoName = "" };

        // Act & Assert
        await Assert.ThrowsAsync<AppValidationException>(() => _service.UpdateAsync(invalidDto));
    }
}

