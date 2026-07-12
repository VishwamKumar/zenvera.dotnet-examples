namespace Exp.Todo.Tests.UnitTests.Application.Commands;

public class CreateTodoCommandHandlerTests
{
    
    [Fact]
    public async Task Handle_ValidCommand_ReturnsTodoId()
    {
        // Arrange
        var mockRepo = new Mock<ITodoWriteRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<TodoEntity>()))
                .ReturnsAsync(1); // Simulate DB-generated ID = 1

        var handler = new CreateTodoCommandHandler(mockRepo.Object);

        var dto = new CreateTodoDto { TodoName = "Test Task" };
        var command = new CreateTodoCommand(dto);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        mockRepo.Verify(r => r.AddAsync(It.Is<TodoEntity>(t => t.TodoName == "Test Task")), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_InvalidTodoName_ThrowsDomainException(string? todoName)
    {
        // Arrange
        var mockRepo = new Mock<ITodoWriteRepository>();
        var handler = new CreateTodoCommandHandler(mockRepo.Object);
        var dto = new CreateTodoDto { TodoName = todoName! };
        var command = new CreateTodoCommand(dto);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }
}

