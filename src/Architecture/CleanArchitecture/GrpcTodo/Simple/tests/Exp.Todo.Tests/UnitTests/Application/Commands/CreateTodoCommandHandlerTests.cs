namespace Exp.Todo.Tests.UnitTests.Application.Commands;

public class CreateTodoCommandHandlerTests
{
    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsTodoId()
    {
        // Arrange
        var mockRepo = new Mock<ITodoRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<TodoEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TodoEntity todo, CancellationToken ct) =>
                {
                    // Simulate DB-generated ID = 1
                    return 1;
                });

        var mockMapper = new Mock<IMapper>();
        var createValidator = new Exp.Todo.Application.Validators.CreateTodoDtoValidator();
        var updateValidator = new Exp.Todo.Application.Validators.UpdateTodoDtoValidator();

        var service = new TodoService(mockRepo.Object, mockMapper.Object, createValidator, updateValidator);

        var dto = new CreateTodoDto { TodoName = "Test Task" };

        // Act
        var result = await service.CreateAsync(dto, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        mockRepo.Verify(r => r.AddAsync(It.Is<TodoEntity>(t => t.TodoName == "Test Task"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateAsync_InvalidTodoName_ThrowsAppValidationException(string? todoName)
    {
        // Arrange
        var mockRepo = new Mock<ITodoRepository>();
        var mockMapper = new Mock<IMapper>();
        var createValidator = new Exp.Todo.Application.Validators.CreateTodoDtoValidator();
        var updateValidator = new Exp.Todo.Application.Validators.UpdateTodoDtoValidator();

        var service = new TodoService(mockRepo.Object, mockMapper.Object, createValidator, updateValidator);
        var dto = new CreateTodoDto { TodoName = todoName! };

        // Act & Assert
        await Assert.ThrowsAsync<AppValidationException>(() => service.CreateAsync(dto, CancellationToken.None));
        mockRepo.Verify(r => r.AddAsync(It.IsAny<TodoEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

