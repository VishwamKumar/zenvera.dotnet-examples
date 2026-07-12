namespace Exp.Todo.Tests.IntegrationTests.Persistence;

public class CreateTodoCommandHandlerTests
{
    private readonly AppDbContext _context;
    private readonly ITodoRepository _repository;
    private readonly TodoService _service;

    public CreateTodoCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TodoDb_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _repository = new TodoRepository(_context);
        
        // Set up AutoMapper - for integration tests, we'll use a simple mock since mapper isn't used in CreateAsync
        var mockMapper = new Mock<IMapper>();
        var mapper = mockMapper.Object;
        
        // Set up validators
        var createValidator = new Exp.Todo.Application.Validators.CreateTodoDtoValidator();
        var updateValidator = new Exp.Todo.Application.Validators.UpdateTodoDtoValidator();
        
        _service = new TodoService(_repository, mapper, createValidator, updateValidator);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTodoAndReturnId()
    {
        // Arrange
        var dto = new CreateTodoDto { TodoName = "Test Task" };

        // Act
        var result = await _service.CreateAsync(dto, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);

        var created = await _context.Todos.FindAsync([result]);
        created.Should().NotBeNull();
        created!.TodoName.Should().Be("Test Task");
    }
}
